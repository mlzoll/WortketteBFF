using StackExchange.Redis;

namespace Game;

public class GameConfiguration
{
    public string ConnectionString { get; set; } = "";
    public string MongoDbName { get; set; } = "";

    public string GameCollectionName { get; set; } = "";
    public ConfigurationOptions? RedisConfigurationOptions { get; set; } = null;
}