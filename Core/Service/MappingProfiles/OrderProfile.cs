using AutoMapper;
using DomainLayer.Models.OrderModule;
using Shared.DTOS.IdentityModuleDTOs;
using Shared.DTOS.OrderModuleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Mapping من AddressDto لـ OrderAddress والعكس
            CreateMap<AddressDto, OrderAddress>().ReverseMap();

            // Mapping من Order لـ OrderToReturnDto
            //CreateMap<Order, OrderToReturnDto>()
            //    .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
            //    .ForMember(d => d.OrderStatus, o => o.MapFrom(s => s.OrderStatus.ToString())) // Enum → String
            //    .ForMember(d => d.Total, o => o.MapFrom(s => s.GetTotal())) // حساب الـ Total
            //    .ForMember(d => d.Items, o => o.MapFrom(s => s.Items)); // ربط Items

            CreateMap<Order, OrderToReturnDto>()
              .ForMember(d => d.OrderStatus, o => o.MapFrom(s => s.OrderStatus.ToString()))
              .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
              .ForMember(d => d.Total, o => o.MapFrom(s => s.GetTotal()))
              .ForMember(d => d.Items, o => o.MapFrom(s => s.Items.Select(i => new OrderItemDto
              {
                  ProductName = i.Product.ProductName,
                  PictureUrl = i.Product.PictureUrl,
                  Price = i.Price,
                  Quantity = i.Quantity
              }).ToList()));


            // Mapping من OrderItem لـ OrderItemDto
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(S => S.Product.ProductName)) // فحص null لـ Product
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>()); // استخدام Resolver لـ PictureUrl

            CreateMap<DeliveryMethod, DeliveryMethodDto>();
        }
    }
}

