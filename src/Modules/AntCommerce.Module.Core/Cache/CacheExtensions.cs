using System.Text;
using Microsoft.Extensions.Caching.Distributed;

namespace AntCommerce.Module.Core.Cache
{
    public static class CacheExtensions
    {
        public static async Task<T?> GetAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default(CancellationToken)) where T : class
        {
            var result = await distributedCache.GetAsync(key, token);
            if (result is null) return default(T);

            var item = System.Text.Json.JsonSerializer.Deserialize<T>(result);
            if (item is null) return default(T);
            return item;
        }

        public static async Task SetAsync(this IDistributedCache distributedCache, string key, object data, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            if (data is not null)
            {
                var serializedItem = System.Text.Json.JsonSerializer.Serialize(data);
                var bytes = Encoding.UTF8.GetBytes(serializedItem);
                await distributedCache.SetAsync(key, bytes, options);
            }
        }
    }
}