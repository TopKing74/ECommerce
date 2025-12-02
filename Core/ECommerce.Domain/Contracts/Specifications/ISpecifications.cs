using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Models;

namespace ECommerce.Domain.Contracts.Specifications
{
    public interface ISpecifications<TEntity,Tkey> where TEntity:BaseEntity<Tkey>
    {
        Expression<Func<TEntity,bool>> Criteria { get; } //Where
        List<Expression<Func<TEntity,object>>> Includes { get; } //Include
        Expression<Func<TEntity,object>> OrderBy { get; }
        Expression<Func<TEntity,object>> OrderByDesc { get; }
        int Take { get; }
        int Skip { get; }
        bool IsPaginated { get; set; }
    }
}
