using Shared;
using Shared.DTOS.ProdductModuleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IProductService
    {
        //Get All Products
        Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams);
        //Get Product ById
        Task<ProductDto> GetProductByIdAsync(int Id);
        //Get All Types 
        Task<IEnumerable<TypeDto>> GetAllTypesAsync();
        //Get All Brands
        Task<IEnumerable<BrandDto>> GetAllBrandsAsync();
    }
}
