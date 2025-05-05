using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application
{
    public interface IRedisCacheService
    {
        Task<T> GetOrAddValueAsync<T>(string key,
            Func<Task<T>> factory,
            TimeSpan? absoluteExpiration = null);

        Task Delete(string key);
    }
}
