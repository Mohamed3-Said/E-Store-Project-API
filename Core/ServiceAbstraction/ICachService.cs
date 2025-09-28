using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface ICachService
    {
        Task<string?> GetAsync(string Cachkey);
        Task SetAsync(string Cachkey, object CachValue, TimeSpan TimeToLive);
    }
}
