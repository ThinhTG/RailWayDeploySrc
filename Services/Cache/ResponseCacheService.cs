using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Cache
{
    public class ResponseCacheService : IResponseCacheService
    {

        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public ResponseCacheService(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _distributedCache = distributedCache;        }
        public async Task<string> GetCacheResponseAsync(string cacheKey)
        {
            var cacheResponse = await _distributedCache.GetStringAsync(cacheKey);
            return string.IsNullOrEmpty(cacheResponse) ? null : cacheResponse;
        }

        public  async Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut)
        {
            if (response == null)
                return;

            var serializerResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            await _distributedCache.SetStringAsync(cacheKey, serializerResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeOut
            });
        }

    
      

        public async Task RemoveCacheResponseAsync(string partern)
        {
           if(string.IsNullOrEmpty(partern))
                throw new ArgumentNullException("Value Can Not Be Null or WhiteSpace");

           await foreach(var Key in GetKeyAsync(partern + "*"))
            {
                await _distributedCache.RemoveAsync(Key);
            }

        }

        private async IAsyncEnumerable<string> GetKeyAsync(string parttern)
        {
            if (string.IsNullOrEmpty(parttern))
            {
                throw new ArgumentNullException("Value Can not be null orr White Space");
            }
            foreach(var endPoint in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(endPoint);
                foreach(var key in server.Keys(pattern : parttern))
                {
                    yield return key.ToString();
                }
            }
        }


    }
}
