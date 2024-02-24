using netcore.fcimiddleware.fondos.domain.Common;

namespace netcore.fcimiddleware.fondos.application.Contracts.Persistence
{
    public interface IAsyncWriteRepository<T> where T : BaseDomainModel
    {
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        void AddEntity(T entity);
        void UpdateEntity(T entity);
        void DeleteEntity(T entity);
    }
}