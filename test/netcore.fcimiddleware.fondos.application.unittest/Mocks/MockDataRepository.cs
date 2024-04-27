using AutoFixture;
using netcore.fcimiddleware.fondos.domain;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;

namespace netcore.fcimiddleware.fondos.application.unittest.Mocks
{
    public static class MockDataRepository
    {
        public static void AddDataRepository(UnitOfWork _unitOfWork)
        {
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var r = new Random();

            var agColocadores = fixture.Build<AgColocador>()
                .With(x => x.IsDeleted, () => r.Next(0, 2) == 0)
                .With(x => x.IsSincronized, () => r.Next(0, 2) == 0)
                .CreateMany(30)
                .ToList();

            _unitOfWork.ApplicationWriteDbContext.AgColocadores!.AddRange(agColocadores);
            _unitOfWork.ApplicationWriteDbContext.SaveChanges();
            _unitOfWork.ApplicationReadDbContext.AgColocadores!.AddRange(agColocadores);
            _unitOfWork.ApplicationReadDbContext.SaveChanges();

            var condIngEgrFondo = fixture.Build<CondIngEgrFondo>()
                .With(x => x.IsDeleted, () => r.Next(0, 2) == 0)
                .With(x => x.IsSincronized, () => r.Next(0, 2) == 0)
                .CreateMany(30)
                .ToList();

            _unitOfWork.ApplicationWriteDbContext.CondIngEgrFondos!.AddRange(condIngEgrFondo);
            _unitOfWork.ApplicationWriteDbContext.SaveChanges();
            _unitOfWork.ApplicationReadDbContext.CondIngEgrFondos!.AddRange(condIngEgrFondo);
            _unitOfWork.ApplicationReadDbContext.SaveChanges();

            var fondos = fixture.Build<Fondo>()
                .Without(tr => tr.TpValorCptFondos)
                .With(x => x.IsDeleted, () => r.Next(0, 2) == 0)
                .With(x => x.IsSincronized, () => r.Next(0, 2) == 0)
                .CreateMany(30)
                .ToList();

            _unitOfWork.ApplicationWriteDbContext.Fondos!.AddRange(fondos);
            _unitOfWork.ApplicationWriteDbContext.SaveChanges();
            _unitOfWork.ApplicationReadDbContext.Fondos!.AddRange(fondos);
            _unitOfWork.ApplicationReadDbContext.SaveChanges();

            var monedas = fixture.Build<Moneda>()
                .Without(tr => tr.Fondos)
                .With(x => x.IsDeleted, () => r.Next(0, 2) == 0)
                .With(x => x.IsSincronized, () => r.Next(0, 2) == 0)
                .CreateMany(30)
                .ToList();

            _unitOfWork.ApplicationWriteDbContext.Monedas!.AddRange(monedas);
            _unitOfWork.ApplicationWriteDbContext.SaveChanges();
            _unitOfWork.ApplicationReadDbContext.Monedas!.AddRange(monedas);
            _unitOfWork.ApplicationReadDbContext.SaveChanges();

            var paises = fixture.Build<Pais>()
                .With(x => x.IsDeleted, () => r.Next(0, 2) == 0)
                .With(x => x.IsSincronized, () => r.Next(0, 2) == 0)
                .CreateMany(30)
                .ToList();

            _unitOfWork.ApplicationWriteDbContext.Paises!.AddRange(paises);
            _unitOfWork.ApplicationWriteDbContext.SaveChanges();
            _unitOfWork.ApplicationReadDbContext.Paises!.AddRange(paises);
            _unitOfWork.ApplicationReadDbContext.SaveChanges();

            var socDepositarias = fixture.Build<SocDepositaria>()
                .Without(tr => tr.Fondos)
                .With(x => x.IsDeleted, () => r.Next(0, 2) == 0)
                .With(x => x.IsSincronized, () => r.Next(0, 2) == 0)
                .CreateMany(30)
                .ToList();

            _unitOfWork.ApplicationWriteDbContext.SocDepositarias!.AddRange(socDepositarias);
            _unitOfWork.ApplicationWriteDbContext.SaveChanges();
            _unitOfWork.ApplicationReadDbContext.SocDepositarias!.AddRange(socDepositarias);
            _unitOfWork.ApplicationReadDbContext.SaveChanges();

            var socGerentes = fixture.Build<SocGerente>()
                .Without(tr => tr.Fondos)
                .With(x => x.IsDeleted, () => r.Next(0, 2) == 0)
                .With(x => x.IsSincronized, () => r.Next(0, 2) == 0)
                .CreateMany(30)
                .ToList();

            _unitOfWork.ApplicationWriteDbContext.SocGerentes!.AddRange(socGerentes);
            _unitOfWork.ApplicationWriteDbContext.SaveChanges();
            _unitOfWork.ApplicationReadDbContext.SocGerentes!.AddRange(socGerentes);
            _unitOfWork.ApplicationReadDbContext.SaveChanges();

            var tpValorCptFondo = fixture.Build<TpValorCptFondo>()
                .Without(tr => tr.CondIngEgrFondos)
                .With(x => x.IsDeleted, () => r.Next(0, 2) == 0)
                .With(x => x.IsSincronized, () => r.Next(0, 2) == 0)
                .CreateMany(30)
                .ToList();

            _unitOfWork.ApplicationWriteDbContext.TpValorCptFondos!.AddRange(tpValorCptFondo);
            _unitOfWork.ApplicationWriteDbContext.SaveChanges();
            _unitOfWork.ApplicationReadDbContext.TpValorCptFondos!.AddRange(tpValorCptFondo);
            _unitOfWork.ApplicationReadDbContext.SaveChanges();
        }
    }
}
