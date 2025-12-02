using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts.Specifications;
using ECommerce.Domain.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ECommerce.Persistence
{
    public static class SpecificationsEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity,Tkey>(IQueryable<TEntity> BaseQuery, ISpecifications<TEntity,Tkey> specifications) where TEntity : BaseEntity<Tkey>
        {
            var Query = BaseQuery;
            if(specifications.Criteria is not null)
            {
                Query = Query.Where(specifications.Criteria);
            }

            if(specifications.OrderBy is not null)
            {
                Query = Query.OrderBy(specifications.OrderBy);
            }
            if(specifications.OrderByDesc is not null)
            {
                Query = Query.OrderByDescending(specifications.OrderByDesc);
            }
            if (specifications.IsPaginated)
            {
                Query = Query.Skip(specifications.Skip).Take(specifications.Take);
            }
            if (specifications.Includes is not null && specifications.Includes.Any())
            {
                Query = specifications.Includes.Aggregate(Query, (CurrentQuery, Expression) => CurrentQuery.Include(Expression));
            }
            return Query;
        }
    }
}
