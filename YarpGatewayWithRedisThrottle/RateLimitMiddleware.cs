using Microsoft.Extensions.Caching.Distributed;

namespace YarpGatewayWithRedisThrottle
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDistributedCache _cache;

        public RateLimitMiddleware(RequestDelegate next, IDistributedCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var key = $"ratelimit:{context.Connection.RemoteIpAddress}";
            var countStr = await _cache.GetStringAsync(key);
            var count = countStr != null ? int.Parse(countStr) : 0;

            if (count >= 100)
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsync("Rate limit exceeded.");
                return;
            }

            count++;
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            };

            await _cache.SetStringAsync(key, count.ToString(), options);

            await _next(context);
        }
    }
}
