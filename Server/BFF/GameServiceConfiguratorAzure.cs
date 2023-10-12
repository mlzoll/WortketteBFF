using Game;

using MongoDB.Driver;
using System.Security.Authentication;

namespace BFF;

public static class GameServiceConfiguratorAzure
{
    public static GameConfiguration ConfigGameServiceAzure(GameConfiguration config, WebApplicationBuilder webApplicationBuilder)
    {
        //MongoUrlBuilder mongoUrlBuilder = GetMongoUrlBuilderAzure(webApplicationBuilder.Configuration);

        config.GameCollectionName = "Games";
        config.MongoDbName = "wortkettemongodb"; //mongoUrlBuilder.DatabaseName;
        config.ConnectionString = webApplicationBuilder.Configuration["AZURE_COSMOS_CONNECTIONSTRING"];
        //config.ConnectionString = @"mongodb://wortkettedb:gw5daA5834dRN0Xq4sO8wqIUMQ9NiCDiSR2aU7pLvWcfFYlN5Pw0YIit1oLI6McY3Cdf8D3wIUbTACDbyZJlng==@wortkettedb.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@wortkettedb@"; //mongoUrlBuilder.ToMongoUrl().ToString();
        return config;
    }

  //  public static MongoUrlBuilder GetMongoUrlBuilderAzure(IConfiguration configuration)
  //  {
  //      string connectionString =
  //@"mongodb://wortkettedb:gw5daA5834dRN0Xq4sO8wqIUMQ9NiCDiSR2aU7pLvWcfFYlN5Pw0YIit1oLI6McY3Cdf8D3wIUbTACDbyZJlng==@wortkettedb.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@wortkettedb@";
  //      MongoClientSettings settings = MongoClientSettings.FromUrl(
  //        new MongoUrl(connectionString)
  //      );

  //      //settings.SslSettings =
  //      //  new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
  //      //var mongoClient = new MongoClient(settings);

  //      return new(connectionString)
  //      {
  //          //DatabaseName = credential["default_database"].Value,
  //          ApplicationName = "WortketteBFF",
  //          RetryReads = true,
  //          RetryWrites = true,
  //          AllowInsecureTls = true
  //      };
  //  }
}