using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts
{
    public interface ISpecifications<Tentity,Tkey> where Tentity : BaseEntity<Tkey>
    {
        //where الشرط بتاع ف ال sql
        Expression<Func<Tentity,bool>>? Criteria { get; }

        //Egger Loading زي ال join in sql كدا
        List<Expression<Func<Tentity,object>>> IncludeExpressions { get; }

        //Sorting :
        Expression<Func<Tentity, object>>? OrderBy { get; }
        Expression<Func<Tentity, object>>? OrderByDescending { get; }

        //Pagination :
        public int Skip { get;  }
        public int Take { get;  }
        public bool IsPaginated { get; set; }


    }
}
