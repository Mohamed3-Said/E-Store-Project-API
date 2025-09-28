using DomainLayer.Contracts;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service
{
    public class CachService(ICachRepository _cachRepository) : ICachService
    {
        public async Task<string?> GetAsync(string Cachkey) => await _cachRepository.GetAsync(Cachkey);

        public async Task SetAsync(string Cachkey, object CachValue, TimeSpan TimeToLive)
        {
            //Conver cachValue from C# object  to string
            var CachValueString = JsonSerializer.Serialize(CachValue);
            await _cachRepository.SetAsync(Cachkey, CachValueString, TimeToLive);
        }
    }
}
