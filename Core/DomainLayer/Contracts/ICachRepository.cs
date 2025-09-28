using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts
{
    public interface ICachRepository
    {
        //Get
        Task<string?> GetAsync(string Cachkey);
        //Set
        Task SetAsync(string Cachkey, string CachValue , TimeSpan TimeToLive);
    }
}
