using Game.Business;
using Game.Data;
using Game.Hubs;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Game;

public static class GameServiceCollectionExtensions
{
    public static IServiceCollection AddGameServices(this IServiceCollection services, Action<GameConfiguration> configAction)
    {
        GameConfiguration config = new();
        configAction(config);
        //todo validierung
        services
            .AddSingleton<IDataAccess, DataAccess>()
            .AddSingleton<IGameBusiness, GameBusiness>()
            .AddSingleton(config);

        services
        .AddMvc()
            .AddApplicationPart(typeof(GameServiceCollectionExtensions).Assembly);

        ISignalRServerBuilder signalRServerBuilder = services.AddSignalR();
        if (config.RedisConfigurationOptions is not null)
            signalRServerBuilder.AddRedis(o => o.Configuration = config.RedisConfigurationOptions);
        return services;
    }

    public static WebApplication MapGameHubs(this WebApplication host)
    {
        //datev spezial lösung. weil geht sonst nicht ...  => https://learn.microsoft.com/en-us/aspnet/core/signalr/scale?view=aspnetcore-3.0#sticky-sessions
        host.MapHub<GameHub>("/GameHub", o => o.Transports = HttpTransportType.ServerSentEvents|HttpTransportType.LongPolling);
        return host;
    }
}