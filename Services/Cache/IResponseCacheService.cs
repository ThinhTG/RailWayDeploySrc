using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Cache
{
    public interface IResponseCacheService
    {
        Task SetCacheResponseAsync (string cacheKey, object response,TimeSpan timeOut);
        Task<string> GetCacheResponseAsync (string cacheKey);
        Task RemoveCacheResponseAsync (string partern);
    }
}
