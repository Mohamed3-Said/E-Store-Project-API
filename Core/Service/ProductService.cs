using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.ProductModule;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using Shared.DTOS.ProdductModuleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var Repo = _unitOfWork.GetRepository<ProductBrand, int>();
            var Brands = await Repo.GetAllAsync();
            var BrandsDto = _mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandDto>>(Brands);
            return BrandsDto;

        }

        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
            var Repo = _unitOfWork.GetRepository<ProductType, int>();
            var Types = await Repo.GetAllAsync();
            var TypesDto = _mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(Types);
            return TypesDto;
        }


        //GetAllProductsWithSpecifications:
        public async Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams)
        {

            //Create object from class (ProductWithBrandandTypeSpecification):
            var specification = new ProductWithBrandandTypeSpecifications(queryParams);
            var Repo = _unitOfWork.GetRepository<Product, int>();
            var Products = await Repo.GetAllAsync(specification);
            var ProductsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(Products);
            var ProductsCount = Products.Count();
            var countSpecification = new ProductCountSpecifications(queryParams);
            var totalCount = await Repo.CountAsync(countSpecification);
            return new PaginatedResult<ProductDto>(
                pageSize: ProductsCount,
                pageIndex: queryParams.PageIndex,
                totalCount: totalCount,
                data: ProductsDto
            );
        }
        public async Task<ProductDto> GetProductByIdAsync(int Id)
        {
            //Create object from class (ProductWithBrandandTypeSpecification):
            var specification = new ProductWithBrandandTypeSpecifications(Id);
            var Product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(specification);
            if(Product is null)
            {
                throw new ProductNotFoundException(Id);
            }
            var ProductDto = _mapper.Map<Product, ProductDto>(Product);
            return ProductDto;
        }
    }
}
