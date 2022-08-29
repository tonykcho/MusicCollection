using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.OutputCaching.Memory;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;

namespace MusicCollection.API.Caching;

/// <summary>
/// Extension methods for the OutputCaching middleware.
/// </summary>
public static class RedisOutputCacheServiceCollectionExtension
{
    public static IServiceCollection AddRedisOutputCache(this IServiceCollection services)
    {
        services.AddOutputCache();

        services.RemoveAll<IOutputCacheStore>();

        services.AddSingleton<IOutputCacheStore, RedisOutputCacheStore>();

        return services;
    }

    public static IServiceCollection AddRedisOutputCache(this IServiceCollection services, Action<OutputCacheOptions> configureOptions)
    {
        services.AddOutputCache(configureOptions);

        services.RemoveAll<IOutputCacheStore>();

        services.AddSingleton<IOutputCacheStore, RedisOutputCacheStore>();

        return services;
    }
}