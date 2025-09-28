using DomainLayer.Contracts;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    abstract class BaseSpecifications<Tentity, Tkey> : ISpecifications<Tentity, Tkey> where Tentity : BaseEntity<Tkey>
    {
        protected BaseSpecifications(Expression<Func<Tentity, bool>>? CriteriaExpression)
        {
            Criteria = CriteriaExpression;
        }
        public Expression<Func<Tentity, bool>>? Criteria { get; private set; }

        #region Include
        public List<Expression<Func<Tentity, object>>> IncludeExpressions { get; } = [];
        //Function => Add Include of List :
        protected void AddInclude(Expression<Func<Tentity, object>> includeExpressions)
            => IncludeExpressions.Add(includeExpressions);
        #endregion

        #region Sorting
        public Expression<Func<Tentity, object>> OrderBy { get; private set; }
        public Expression<Func<Tentity, object>> OrderByDescending { get; private set; }
        //Function => Add OrderBy :
        protected void AddOrderBy(Expression<Func<Tentity, object>> OrderByExp) => OrderBy = OrderByExp;
        //Function => Add OrderByDescending :
        protected void AddOrderByDescending(Expression<Func<Tentity, object>> OrderByDescendingExp) => OrderByDescending = OrderByDescendingExp;
        #endregion

        #region Pagination

        public int Skip { get; private set; }
        public int Take { get; private set; }
        public bool IsPaginated { get; set; }
        //Function => Add Pagination :
        protected void ApplyPagination(int PageSize , int PageIndex)
        {
            IsPaginated = true;
            Take = PageSize;
            Skip = (PageIndex-1) * PageSize;
        }
        #endregion
    }
}
