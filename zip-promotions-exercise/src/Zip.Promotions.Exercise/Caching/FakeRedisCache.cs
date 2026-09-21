using System.Collections.Concurrent;
using System.Text.Json;

using Microsoft.Extensions.Logging;

namespace Zip.Promotions.Exercise.Caching;

public sealed class FakeRedisCache : ICache
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    private readonly ConcurrentDictionary<string, CacheEntry> entries = new(StringComparer.Ordinal);
    private readonly TimeProvider timeProvider;
    private readonly ILogger<FakeRedisCache> logger;

    private long hits;
    private long misses;

    public FakeRedisCache(TimeProvider timeProvider, ILogger<FakeRedisCache> logger)
    {
        this.timeProvider = timeProvider;
        this.logger = logger;
    }

    public long Hits => Interlocked.Read(ref this.hits);

    public long Misses => Interlocked.Read(ref this.misses);

    public int Count => this.entries.Count;

    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        if (this.entries.TryGetValue(key, out var entry))
        {
            if (entry.ExpiresAt > this.timeProvider.GetUtcNow())
            {
                Interlocked.Increment(ref this.hits);

                return Task.FromResult(JsonSerializer.Deserialize<T>(entry.Payload, SerializerOptions));
            }

            this.entries.TryRemove(new KeyValuePair<string, CacheEntry>(key, entry));
        }

        Interlocked.Increment(ref this.misses);
        this.logger.LogDebug("Cache miss for {CacheKey}", key);

        return Task.FromResult<T?>(default);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken cancellationToken = default)
    {
        var entry = new CacheEntry(
            JsonSerializer.Serialize(value, SerializerOptions),
            this.timeProvider.GetUtcNow() + ttl);

        this.entries[key] = entry;

        return Task.CompletedTask;
    }

    private sealed record CacheEntry(string Payload, DateTimeOffset ExpiresAt);
}
