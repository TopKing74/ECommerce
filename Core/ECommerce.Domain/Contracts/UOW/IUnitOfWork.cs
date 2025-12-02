using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts.Repos;
using ECommerce.Domain.Models;

namespace ECommerce.Domain.Contracts.UOW
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity,Tkey> GetRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>;
        Task<int> SaveChangesAsync();
    }
}
