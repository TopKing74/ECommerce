using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts.Specifications;
using ECommerce.Domain.Models;

namespace ECommerce.Domain.Contracts.Repos
{
    public interface IGenericRepository<TEntity,Tkey> where TEntity : BaseEntity<Tkey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(Tkey id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, Tkey> specifications);
        Task<TEntity> GetByIdWithSpecAsync(ISpecifications<TEntity, Tkey> specifications);
        Task<int> GetCountWithSpecAsync(ISpecifications<TEntity, Tkey> specifications);


    }
}
