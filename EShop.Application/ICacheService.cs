using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain;

namespace EShop.Application
{
    public interface ICacheService
    {
         Task<T> GetOrAddValueAsync<T>(string key,
            Func<Task<T>> factory,
            TimeSpan? absoluteExpiration = null);
        void Remove(string key_products);
    }
}
