using DomainLayer.Models.ProductModule;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    //implment abstract class specification , No Interface:
    class ProductWithBrandandTypeSpecifications : BaseSpecifications<Product, int>
    {
        //1-Get All Products With Brand and Type:
        public ProductWithBrandandTypeSpecifications(ProductQueryParams queryParams)
            : base(P => (!queryParams.Brandid.HasValue || P.BrandId == queryParams.Brandid) && (!queryParams.Typeid.HasValue || P.TypeId == queryParams.Typeid)
            && (string.IsNullOrWhiteSpace(queryParams.SearchValue) || P.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))

        //criteria : where(P=P.BrandId==Brandid && P=>P.TypeId==Typeid)
        {
            // null => No Criteria 
            //Include :
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);

            //Sorting :
            switch (queryParams.sortingOption)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(P => P.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDescending(P => P.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(P => P.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDescending(P => P.Price);
                    break;
                default:
                    break;
            }

            //Pagination :
            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);

        }
        //2-Get Product By Id with Brand and Type :
        public ProductWithBrandandTypeSpecifications(int id) : base(P => P.Id == id)
        {
            //Criteria : P=>P.Id == id
            //Include:
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);

        }




    }
}
