using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.BasketModuleDTOs
{
    public class BasketDto
    {
        public string Id { get; set; } // Guid => Created from Client [Frontend]
        public ICollection<BasketItemDto> Items { get; set; } = [];


        public string? clientSecret { get; set; }
        public string? paymentIntentId { get; set; }
        public int? deliveryMethodId { get; set; }
        public decimal? shippingPrice { get; set; }
        

    }
}
