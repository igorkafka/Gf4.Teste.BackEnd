

using Microsoft.EntityFrameworkCore;
using PedidoStore.Core.SharedKernel;

namespace PedidoStore.Infrastructure.Data.Repositories.Common
{
    internal abstract class BaseWriteOnlyRepository<TEntity, TKey>(WriteDbContext dbContext) : IWriteOnlyRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        private static readonly Func<WriteDbContext, TKey, Task<TEntity>> GetByIdCompiledAsync =
            EF.CompileAsyncQuery((WriteDbContext dbContext, TKey id) =>
                dbContext
                    .Set<TEntity>()
                    .AsNoTrackingWithIdentityResolution()
                    .FirstOrDefault(entity => entity.Id.Equals(id)));

        private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();
        protected readonly WriteDbContext DbContext = dbContext;

        public void Add(TEntity entity) =>
            _dbSet.Add(entity);

        public virtual void Update(TEntity entity) =>
            _dbSet.Update(entity);

        public virtual void Remove(TEntity entity) =>
            _dbSet.Remove(entity);

        public async Task<TEntity> GetByIdAsync(TKey id) =>
            await GetByIdCompiledAsync(DbContext, id);

        #region IDisposable

        // To detect redundant calls.
        private bool _disposed;

        ~BaseWriteOnlyRepository() => Dispose(false);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            // Dispose managed state (managed objects).
            if (disposing)
                DbContext.Dispose();

            _disposed = true;
        }

        #endregion
    }
}
