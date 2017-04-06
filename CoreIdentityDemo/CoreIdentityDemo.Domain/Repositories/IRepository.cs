using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreIdentityDemo.Domain.Repositories
{
    public interface IRepository<TEntity, TKey>
    {
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();

        TEntity GetByKey(TKey key);
        Task<TEntity> GetByKeyAsync(TKey key);

        void Add(TEntity entity);
        Task AddAsync(TEntity entity);

        void Update(TEntity entity);
        Task UpdateAsync(TEntity entity);

        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
