using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface ICacheManagementService
    {
        void AddToCache<T>(string key, T value, TimeSpan timeSpan);
        T GetFromCache<T>(string key) where T : class;
        void DeleteCache(string key);
    }
}
