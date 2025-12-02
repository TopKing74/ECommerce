using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts.Repos;
using ECommerce.Domain.Contracts.Specifications;
using ECommerce.Domain.Models;
using ECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistence.Repos
{
    public class GenericRepository<TEntity, Tkey>(StoreDbContext context) : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        public async Task<IEnumerable<TEntity>> GetAllAsync() => await context.Set<TEntity>().ToListAsync();

        public async Task<TEntity> GetByIdAsync(Tkey id) => await context.Set<TEntity>().FindAsync(id);
        public void Add(TEntity entity) => context.Set<TEntity>().Add(entity);  
        public void Update(TEntity entity) => context.Set<TEntity>().Update(entity);
        public void Delete(TEntity entity) => context.Set<TEntity>().Remove(entity);

        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, Tkey> specifications)
        {
            return await SpecificationsEvaluator.CreateQuery(context.Set<TEntity>(), specifications).ToListAsync();
        }

        public async Task<TEntity> GetByIdWithSpecAsync(ISpecifications<TEntity, Tkey> specifications)
        {
            return await SpecificationsEvaluator.CreateQuery(context.Set<TEntity>(), specifications).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountWithSpecAsync(ISpecifications<TEntity, Tkey> specifications)
        {
            return await SpecificationsEvaluator.CreateQuery(context.Set<TEntity>(), specifications).CountAsync();
        }
    }
}
