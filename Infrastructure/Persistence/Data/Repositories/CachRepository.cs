using DomainLayer.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Repositories
{
    public class CachRepository(IConnectionMultiplexer _connection) : ICachRepository
    {
        private readonly IDatabase _database = _connection.GetDatabase();
        public async Task<string?> GetAsync(string Cachkey)
        {
            var CachValue = await _database.StringGetAsync(Cachkey);
            return CachValue.IsNullOrEmpty ? null: CachValue.ToString();
        }

        public async Task SetAsync(string Cachkey, string CachValue, TimeSpan TimeToLive)
        {
           await _database.StringSetAsync(Cachkey, CachValue, TimeToLive);
        }
    }
}
