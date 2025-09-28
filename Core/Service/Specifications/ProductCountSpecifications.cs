using DomainLayer.Models.ProductModule;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
     class ProductCountSpecifications : BaseSpecifications<Product, int>
    {
        //ctor chaning of base class: BaseSpecifications
        public ProductCountSpecifications(ProductQueryParams queryParams) 
            : base(P => (!queryParams.Brandid.HasValue || P.BrandId == queryParams.Brandid) && (!queryParams.Typeid.HasValue || P.TypeId == queryParams.Typeid)
            && (string.IsNullOrWhiteSpace(queryParams.SearchValue) || P.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))

        {


        }
    }
}
