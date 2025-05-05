using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using static System.Formats.Asn1.AsnWriter;
namespace EShop.Application
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _db;
        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer, IConfiguration configuration, IHostApplicationLifetime applicationLifetime) {
            var redisConfig = configuration.GetSection("Redis");
            var dbIndex = redisConfig["DefaultDatabase"] ?? "0";
            _db = connectionMultiplexer.GetDatabase(int.Parse(dbIndex));
            
        }

        public async Task<T> GetOrAddValueAsync<T>(string key,
            Func<Task<T>> factory,
            TimeSpan? absoluteExpiration = null)
        {
            var cached = await _db.StringGetAsync(key);
            if (cached.HasValue) {
                return JsonSerializer.Deserialize<T>(cached!)!;
            }

            T value = await factory();
            var json = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, json, absoluteExpiration);
            return value;
        }

        public async Task Delete(string key)
        {
            var cached = await _db.StringGetAsync(key);
            if (cached.HasValue) { 
                await _db.KeyDeleteAsync(key);
            }
        }

      
    }
}
