using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DTOS.OrderModuleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController(IServiceManager _serviceManager) : ControllerBase
    {
        //Create Orde Endpoint :
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto OrderDto)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email); //Get Email From Token 
            var Order = await _serviceManager.OrderService.CreateOrder(OrderDto,Email!);
            return Ok(Order);
        }
       //Get Delivery Methods Endpoint :
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetDeliveryMethods()
        {
            var DeliveryMethods = await _serviceManager.OrderService.GetDeliveryMethodsAsync();
            return Ok(DeliveryMethods);
        }
        //Get All orders By Email Endpoint :
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetAllOrders()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email); //Get Email From Token
            var Orders = await _serviceManager.OrderService.GetAllOrdersByEmailAsync(Email!);
            return Ok(Orders);
        }
        //Get Order By Id Endpoint :
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(Guid id)
        {
            var Order = await  _serviceManager.OrderService.GetOrderByIdAsync(id);
            return Ok(Order);
        }
    }
}
