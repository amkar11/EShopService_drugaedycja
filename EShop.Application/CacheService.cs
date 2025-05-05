using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace EShop.Application
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public async Task<T> GetOrAddValueAsync<T>(string key,
            Func<Task<T>> factory,
            TimeSpan? absoluteExpiration = null)
        {
            if (_memoryCache.TryGetValue(key, out T? cached) && cached is not null)
            {
                return cached;
            }
            T value = await factory();
            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(absoluteExpiration ?? TimeSpan.FromHours(24));
            _memoryCache.Set(key, value, options);
            return value;
        }

        public void Remove(string key_products) {
            
                if (_memoryCache.TryGetValue(key_products, out _))
                {
                    _memoryCache.Remove(key_products);
                }
            
        }

    }
}
