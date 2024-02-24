using netcore.fcimiddleware.fondos.domain.Common;

namespace netcore.fcimiddleware.fondos.application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IAsyncReadRepository<TEntity> RepositoryRead<TEntity>() where TEntity : BaseDomainModel;
        IAsyncWriteRepository<TEntity> RepositoryWrite<TEntity>() where TEntity : BaseDomainModel;
        Task<int> Complete();
    }
}
