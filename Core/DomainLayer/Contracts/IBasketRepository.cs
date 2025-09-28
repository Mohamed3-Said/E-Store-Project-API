using DomainLayer.Models.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts
{
    public interface IBasketRepository
    {
        //GetBasketAsync:
        Task<CustomerBasket?> GetBasketAsync(string key);
        //CreateOrUpdateBasketAsync:
        Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket , TimeSpan? TimeToLive = null);
        //DeleteBasketAsync:
        Task<bool> DeleteBasketAsync(string id);
    }
}
