#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/netcore.fcimiddleware.fondos.api/netcore.fcimiddleware.fondos.api.csproj", "src/netcore.fcimiddleware.fondos.api/"]
COPY ["src/netcore.fcimiddleware.fondos.application/netcore.fcimiddleware.fondos.application.csproj", "src/netcore.fcimiddleware.fondos.application/"]
COPY ["src/netcore.fcimiddleware.fondos.domain/netcore.fcimiddleware.fondos.domain.csproj", "src/netcore.fcimiddleware.fondos.domain/"]
COPY ["src/netcore.fcimiddleware.fondos.infrastructure/netcore.fcimiddleware.fondos.infrastructure.csproj", "src/netcore.fcimiddleware.fondos.infrastructure/"]
RUN dotnet restore "src/netcore.fcimiddleware.fondos.api/netcore.fcimiddleware.fondos.api.csproj"
COPY . .
WORKDIR "/src/src/netcore.fcimiddleware.fondos.api"
RUN dotnet build "netcore.fcimiddleware.fondos.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "netcore.fcimiddleware.fondos.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "netcore.fcimiddleware.fondos.api.dll"]