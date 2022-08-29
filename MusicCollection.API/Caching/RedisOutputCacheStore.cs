using Microsoft.AspNetCore.OutputCaching;
using StackExchange.Redis;

namespace MusicCollection.API.Caching;

public class RedisOutputCacheStore : IOutputCacheStore
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisOutputCacheStore(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async ValueTask EvictByTagAsync(string tag, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(tag);

        var db = _connectionMultiplexer.GetDatabase();
        RedisValue[] RedisSetMembers = await db.SetMembersAsync(tag);

        RedisKey[] keys = RedisSetMembers
            .Select(member => (RedisKey)member.ToString())
            .Concat(new[] { (RedisKey)tag })
            .ToArray();

        await db.KeyDeleteAsync(keys);
    }

    public async ValueTask<byte[]?> GetAsync(string key, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(key);

        var db = _connectionMultiplexer.GetDatabase();
        return await db.StringGetAsync(key);
    }

    public async ValueTask SetAsync(string key, byte[] value, string[]? tags, TimeSpan validFor, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        var db = _connectionMultiplexer.GetDatabase();

        foreach (var tag in tags ?? Array.Empty<string>())
        {
            await db.SetAddAsync(tag, key);
        }

        await db.StringSetAsync(key, value, validFor);
    }
}