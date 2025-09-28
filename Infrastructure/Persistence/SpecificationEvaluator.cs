using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    static class SpecificationEvaluator
    {
        //Create Query :
        //1-Entry Point : _dbcintext.Set(Tentity)
        //2-specifications : where(p=p.id == id).Include(p=>p.ProductBrand).Include(p=>p.productType)
        public static IQueryable<Tentity> CreateQuery<Tentity, Tkey>(IQueryable<Tentity> EntryPoint, ISpecifications<Tentity, Tkey> specifications) where Tentity : BaseEntity<Tkey>
        {
            //Criteria :
            var Query = EntryPoint;
            if (specifications.Criteria is not null)
            {
                Query = Query.Where(specifications.Criteria);
            }


            //Sorting :
            if(specifications.OrderBy is not null)
            {
                Query = Query.OrderBy(specifications.OrderBy);
            }
            if (specifications.OrderByDescending is not null)
            {
                Query = Query.OrderByDescending(specifications.OrderByDescending);
            }


            //Include :
            if (specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Count > 0)
            {   //foreach (var exp in specifications.IncludeExpressions)
                //    Query = Query.Include(exp);
                Query = specifications.IncludeExpressions.Aggregate(Query, (CurrentQuery, includeExp) => CurrentQuery.Include(includeExp));
            }

            //Pagination :
            if(specifications.IsPaginated)
            {
                Query = Query.Skip(specifications.Skip).Take(specifications.Take);
            }
            return Query;
        }
    }
}
