using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowersApp.Shared.Redis
{
    public static class RedisServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(connectionString));
            services.AddSingleton<IRedisCacheService, RedisCacheService>();
            return services;
        }
    }
}
