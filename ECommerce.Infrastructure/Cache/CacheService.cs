using ECommerce.Application.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace ECommerce.Infrastructure.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IConnectionMultiplexer redis;
        private readonly IDatabase db;
        public CacheService(IConnectionMultiplexer redis)
        {
            this.redis = redis;
           db = redis.GetDatabase();
        }
        public T? Get<T>(string key) where T:class
        {
            RedisValue value = db.StringGet(key);
            if (value.IsNull)
            {
                return null;
            }
            var deserialize = JsonSerializer.Deserialize<T>(value);     
            return deserialize;
        }

        public void Remove(string key)
        {
            db.KeyDelete(key);
        }

        public void Set<T>(string key, TimeSpan Expiration, T Data)
        {
           
           var serialize = JsonSerializer.Serialize(Data);

            
            db.StringSet(key, serialize, Expiration);
        }
    }
}






//using ECommerce.Application.Interfaces;
//using Microsoft.Extensions.Caching.Memory;

//namespace ECommerce.Infrastructure.Cache
//{
//    public class CacheService : ICacheService
//    {
//        private readonly IMemoryCache cache;

//        public CacheService(IMemoryCache cache)
//        {
//            this.cache = cache;
//        }
//        public T? Get<T>(string key) where T : class
//        {
//            if (cache.TryGetValue(key, out object? value))
//            {
//                return (T?)value;
//            }

//            return null;
//        }

//        public void Remove(string key)
//        {

//        }

//        public void Set<T>(string key, TimeSpan Expiration, T Data)
//        {
//            cache.Set(key, Data, TimeSpan.FromHours(1));
//        }
//    }
//}
