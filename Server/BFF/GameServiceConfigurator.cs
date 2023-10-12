using Game;

using Microsoft.Extensions.Options;
using MongoDB.Driver;

using Steeltoe.Extensions.Configuration;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace BFF;

public static class GameServiceConfigurator
{
    public static GameConfiguration ConfigGameService(GameConfiguration config, WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Configuration.AddCloudFoundry();
        webApplicationBuilder.Host.AddCloudFoundryConfiguration();
        webApplicationBuilder.Services.AddOptions();
        webApplicationBuilder.Services.ConfigureCloudFoundryOptions(webApplicationBuilder.Configuration);

        //VCAP Cloud Foundry Options hinzufügen
        webApplicationBuilder.Configuration.AddCloudFoundry();
        webApplicationBuilder.Services.AddOptions();
        webApplicationBuilder.Services.ConfigureCloudFoundryOptions(webApplicationBuilder.Configuration);

        ServiceProvider? serviceProvider = webApplicationBuilder.Services.BuildServiceProvider();


        MongoUrlBuilder mongoUrlBuilder = serviceProvider.GetMongoUrlBuilder();

        config.GameCollectionName = "Games";
        config.MongoDbName = mongoUrlBuilder.DatabaseName;
        config.ConnectionString = mongoUrlBuilder.ToMongoUrl().ToString();
        return config;
    }

    public static MongoUrlBuilder GetMongoUrlBuilder(this ServiceProvider serviceProvider)
    {
        IOptions<CloudFoundryServicesOptions> cloudfoundryServiceOptions = serviceProvider.GetRequiredService<IOptions<CloudFoundryServicesOptions>>();

        Dictionary<string, Credential>? credential = cloudfoundryServiceOptions.Value?.GetServicesList()
            .FirstOrDefault(services => services.Name.Equals("WortKetteDb", StringComparison.InvariantCultureIgnoreCase))
            ?.Credentials;

        if (credential == null)
        {
            throw new NotSupportedException("Mongo Service nicht gefunden");
        }

        IOptions<CloudFoundryApplicationOptions> cloudfoundryApplicationOptions = serviceProvider.GetRequiredService<IOptions<CloudFoundryApplicationOptions>>();

        return new(credential["uri"].Value)
        {
            DatabaseName = credential["default_database"].Value,
            ApplicationName = cloudfoundryApplicationOptions.Value?.ApplicationName,
            RetryReads = true,
            RetryWrites = true,
            AllowInsecureTls = true
        };
    }
}