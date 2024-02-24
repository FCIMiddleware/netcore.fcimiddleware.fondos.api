using Microsoft.EntityFrameworkCore;
using Moq;
using netcore.fcimiddleware.fondos.infrastructure.Persistence;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;

namespace netcore.fcimiddleware.fondos.application.unittest.Mocks
{
    public static class MockUnitOfWork
    {
        public static Mock<UnitOfWork> GetUnitOfWork()
        {
            Guid dbContextId = Guid.NewGuid();
            var optionsWrite = new DbContextOptionsBuilder<ApplicationWriteDbContext>()
                .UseInMemoryDatabase(databaseName: $"ApplicationWriteDbContext-{dbContextId}")
                .Options;

            var optionsRead = new DbContextOptionsBuilder<ApplicationReadDbContext>()
                .UseInMemoryDatabase(databaseName: $"ApplicationWriteDbContext-{dbContextId}")
                .Options;

            var applicationDbContextReadFake = new ApplicationReadDbContext(optionsRead);
            var applicationDbContextWriteFake = new ApplicationWriteDbContext(optionsWrite);

            applicationDbContextWriteFake.Database.EnsureDeleted();
            applicationDbContextReadFake.Database.EnsureDeleted();

            var mockUnitOfWork = new Mock<UnitOfWork>(applicationDbContextReadFake, applicationDbContextWriteFake);

            return mockUnitOfWork;
        }
    }
}
