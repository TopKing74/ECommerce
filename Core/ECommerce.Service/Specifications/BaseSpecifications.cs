using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts.Specifications;
using ECommerce.Domain.Models;

namespace ECommerce.Service.Specifications
{
    public abstract class BaseSpecifications<TEntity, Tkey> : ISpecifications<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        #region Where
        protected BaseSpecifications(Expression<Func<TEntity, bool>> _Criteria)
        {
            Criteria = _Criteria;
        }
        public Expression<Func<TEntity, bool>> Criteria { get; protected set; }

        #endregion

        #region Ordering
        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        public Expression<Func<TEntity, object>> OrderByDesc { get; private set; }

        protected void AddOrderBy(Expression<Func<TEntity, object>> _OrderBy)
        {
            OrderBy = _OrderBy;
        }
        protected void AddOrderByDesc(Expression<Func<TEntity, object>> _OrderByDesc)
        {
            OrderByDesc = _OrderByDesc;
        }
        #endregion

        #region Include
        public List<Expression<Func<TEntity, object>>> Includes => [];
        protected void AddInclude(Expression<Func<TEntity, object>> IncludeExpression)
        {
            Includes.Add(IncludeExpression);
        }
        #endregion
        #region Pagination
        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPaginated { get; set; }
        public void ApplyPagination(int PageIndex, int PageSize)
        {
            IsPaginated = true;
            Take = PageSize;
            Skip = (PageIndex - 1) * PageSize;
        }

        #endregion
    }
}
