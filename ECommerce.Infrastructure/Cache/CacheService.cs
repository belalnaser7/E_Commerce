using ECommerce.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache cache;

        public CacheService(IMemoryCache cache)
        {
            this.cache = cache;
        }
        public T? Get<T>(string key) where T:class
        {
            if (cache.TryGetValue(key,out object?value))
            {
                return (T?)value;
            }
            
            return null;
        }

        public void Remove(string key)
        {
           
        }

        public void Set<T>(string key, TimeSpan Expiration, T Data)
        {
            cache.Set(key, Data,TimeSpan.FromHours(1));
        }
    }
}
