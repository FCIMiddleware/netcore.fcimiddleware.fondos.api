using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using netcore.fcimiddleware.fondos.api.Middleware;
using netcore.fcimiddleware.fondos.application;
using netcore.fcimiddleware.fondos.infrastructure;
using netcore.fcimiddleware.fondos.infrastructure.Persistence;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add bootstrap Infrastructure
builder.Services.AddInfrastructureServices(builder.Configuration);
// Add bootstrap Application
builder.Services.AddApplicationServices();

// Add Cors Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    );
});

builder.Services.AddHealthChecks()
    .AddCheck("netcore.fcimiddleware.fondos.api:WebAPI", () => HealthCheckResult.Healthy())
    .AddNpgSql(builder.Configuration.GetValue<string>("ConnectionStrings:ReadPostgreSQL")!,
        name: "netcore.fcimiddleware.fondos.api:CheckDBRead",
        failureStatus: HealthStatus.Unhealthy)
    .AddNpgSql(builder.Configuration.GetValue<string>("ConnectionStrings:WritePostgreSQL")!,
        name: "netcore.fcimiddleware.fondos.api:CheckDBWrite",
        failureStatus: HealthStatus.Unhealthy);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsStaging() || app.Environment.IsProduction())
{
    using (var scope = app.Services.CreateScope())
    {
        var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationWriteDbContext>();
        dataContext.Database.Migrate();
    }
}

app.UseAuthorization();

app.UseCors("CorsPolicy");

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

app.Run();
