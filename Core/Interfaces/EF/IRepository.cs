using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces.EF
{
    public interface IRepository<TEntity> where TEntity : EntityBase, new()
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(int id);

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);
        Task<int> DeleteAsync(TEntity entity);
    }

}
