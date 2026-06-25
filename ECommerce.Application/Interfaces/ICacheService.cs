using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface ICacheService
    {
        T? Get<T>(string key) where T : class;
        void Set<T>(string key, TimeSpan Expiration, T Data);
        void Remove(string key);


    }
}
