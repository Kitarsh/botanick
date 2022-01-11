namespace BotANick.Twitch;

public class Startup
{
    public Startup()
    {
        this.Run();
    }

    private void Run()
    {
        ServiceCollection services = ConfigureServices();
        var provider = services.BuildServiceProvider();

        var startupSrv = provider.GetRequiredService<StartupService>(); // Call constructor to initialize service.

        provider.GetRequiredService<CommandService>();
        provider.GetRequiredService<StoredCommandService>();
        startupSrv.OpenConnection();

        provider.GetRequiredService<TimerService>();
        provider.GetRequiredService<TwitchApiService>();
    }

    private ServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton(BuildConfiguration());
        services.AddSingleton(GenerateTwitchClient());
        services.AddSingleton<StartupService>();
        services.AddSingleton<TimerService>();
        services.AddSingleton<CommandService>();
        services.AddSingleton<StoredCommandService>();
        services.AddSingleton<TwitchApiService>();
        return services;
    }

    private TwitchClient GenerateTwitchClient()
    {
        var clientOptions = new ClientOptions
        {
            MessagesAllowedInPeriod = 750,
            ThrottlingPeriod = TimeSpan.FromSeconds(30)
        };
        WebSocketClient customClient = new WebSocketClient(clientOptions);
        return new TwitchClient(customClient);
    }

    private IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddYamlFile("twitch-config-prod.yml");
        return builder.Build();
    }
}
