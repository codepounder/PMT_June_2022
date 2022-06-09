using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using Ferrara.Compass.Abstractions.Interfaces;

namespace Ferrara.Compass.Services
{
    public class CacheManagementService : ICacheManagementService
    {
        public void AddToCache<T>(string key, T value, TimeSpan timeSpan)
        {
            DeleteCache(key);
            HttpRuntime.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, timeSpan, CacheItemPriority.High, null);
        }

        public void DeleteCache(string key)
        {
            try
            {
                HttpRuntime.Cache.Remove(key);
            }
            catch { }
        }

        public T GetFromCache<T>(string key) where T : class
        {
            try
            {
                if (HttpContext.Current.Cache[key] != null)
                {
                    var result = (T)HttpRuntime.Cache[key];
                    return result;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
