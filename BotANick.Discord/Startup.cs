namespace BotANick.Discord;

public class Startup
{
    public Startup()
    {
        this.RunAsync();
    }

    private IServiceProvider RunAsync()
    {
        var services = new ServiceCollection();             // Create a new instance of a service collection
        ConfigureServices(services);

        var provider = services.BuildServiceProvider();     // Build the service provider
        provider.GetRequiredService<LoggingService>();      // Start the logging service

        provider.GetRequiredService<RegisterSlashCommandService>();   // Start the register slash command service

        provider.GetRequiredService<TopTenService>();
        provider.GetRequiredService<PaniniService>();
        provider.GetRequiredService<GlobalService>();

        provider.GetRequiredService<StartupService>().Start();       // Start the startup service
        return provider;
    }

    private void ConfigureServices(IServiceCollection services)
    {
        //services.AddSingleton(Client)
        services.AddSingleton(GenerateDiscordSocketClient())
                .AddSingleton<StartupService>()         // Add startupservice to the collection
                .AddSingleton<LoggingService>()         // Add loggingservice to the collection
                .AddSingleton<Random>()                 // Add random to the collection
                .AddSingleton(BuildConfiguration())           // Add the configuration to the collection
                .AddSingleton<RegisterSlashCommandService>()
                .AddSingleton<GlobalService>()
                .AddSingleton<TopTenService>()
                .AddSingleton<PaniniService>();
    }

    private IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddYamlFile("discord-config-prod.yml");
        return builder.Build();
    }

    private DiscordSocketClient GenerateDiscordSocketClient()
    {
        return new DiscordSocketClient(new DiscordSocketConfig
        {                                       // Add discord to the collection
            LogLevel = LogSeverity.Verbose,     // Tell the logger to give Verbose amount of info
            MessageCacheSize = 1000             // Cache 1,000 messages per channel
        });
    }
}
