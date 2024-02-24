using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.domain.Common;
using netcore.fcimiddleware.fondos.infrastructure.Persistence;
using System.Collections;

namespace netcore.fcimiddleware.fondos.infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable _repositoriesRead;
        private Hashtable _repositoriesWrite;
        private readonly ApplicationReadDbContext _contextRead;
        private readonly ApplicationWriteDbContext _contextWrite;

        public ApplicationReadDbContext ApplicationReadDbContext => _contextRead;
        public ApplicationWriteDbContext ApplicationWriteDbContext => _contextWrite;

        public UnitOfWork(
            ApplicationReadDbContext contextRead,
            ApplicationWriteDbContext contextWrite)
        {
            _contextRead = contextRead;
            _contextWrite = contextWrite;
        }

        public async Task<int> Complete()
        {
            try
            {
                return await _contextWrite.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar salvar en la base de datos");
            }
        }

        public void Dispose()
        {
            _contextRead.Dispose();
            _contextWrite.Dispose();
        }

        public IAsyncReadRepository<TEntity> RepositoryRead<TEntity>() where TEntity : BaseDomainModel
        {
            if (_repositoriesRead == null)
            {
                _repositoriesRead = new Hashtable();
            }

            var type = typeof(TEntity).Name;

            if (!_repositoriesRead.ContainsKey(type))
            {
                var repositoryType = typeof(RepositoryReadBase<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _contextRead);
                _repositoriesRead.Add(type, repositoryInstance);
            }

            return (IAsyncReadRepository<TEntity>)_repositoriesRead[type];
        }

        public IAsyncWriteRepository<TEntity> RepositoryWrite<TEntity>() where TEntity : BaseDomainModel
        {
            if (_repositoriesWrite == null)
            {
                _repositoriesWrite = new Hashtable();
            }

            var type = typeof(TEntity).Name;

            if (!_repositoriesWrite.ContainsKey(type))
            {
                var repositoryType = typeof(RepositoryWriteBase<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _contextWrite);
                _repositoriesWrite.Add(type, repositoryInstance);
            }

            return (IAsyncWriteRepository<TEntity>)_repositoriesWrite[type];
        }
    }
}
