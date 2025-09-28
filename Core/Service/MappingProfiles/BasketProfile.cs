using AutoMapper;
using DomainLayer.Models.BasketModule;
using Shared.DTOS.BasketModuleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket,BasketDto>().ReverseMap(); 
            CreateMap<BasketItem,BasketItemDto>().ReverseMap();
        }

    }
}
