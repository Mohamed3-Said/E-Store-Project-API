using Shared.DTOS.OrderModuleDTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IOrderService
    {
        //  Creating Order Will Take => [ Basket Id, Shipping Address , Delivery Method Id , Customer Email ]
        //  And Return => [ Order Details(Id , UserName , OrderDate , Items (Product Name - Picture Url - Price - Quantity)
        //  , Address , Delivery Method Name , Order Status Value , Sub Total, Total Price  ) ]

        Task<OrderToReturnDto> CreateOrder(OrderDto orderDto, string Email);
         Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync();
         Task<IEnumerable<OrderToReturnDto>> GetAllOrdersByEmailAsync(string Email);
        Task<OrderToReturnDto> GetOrderByIdAsync(Guid Id);


    }
}
