namespace BotANick.Discord.Services;

public class StartupService
{
    private readonly IServiceProvider _provider;
    private readonly DiscordSocketClient _discord;
    private readonly IConfigurationRoot _config;

    // DiscordSocketClient and IConfigurationRoot are injected automatically from the IServiceProvider
    public StartupService(
        IServiceProvider provider,
        DiscordSocketClient discord,
        IConfigurationRoot config)
    {
        _provider = provider;
        _config = config;
        _discord = discord;
    }

    public void Start()
    {
        string discordToken = _config["tokens:discord"];     // Get the discord token from the config file
        if (string.IsNullOrWhiteSpace(discordToken))
            throw new ArgumentException("Please enter your bot's token into the `_configuration.json` file found in the applications root directory.");
        Console.WriteLine("Starting Discord bot ...");

        //TwitchLogs.SetDiscordClient(_discord);
        //BoiteAIdeeService.SetDiscordClient(_discord);
        //GitHubService.InitConfig(_config);
        _ = StartAsync(discordToken);
    }

    public async Task StartAsync(string discordToken)
    {
        var projectAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.Contains("BotANick"));
        await _discord.LoginAsync(TokenType.Bot, discordToken);     // Login to discord
        await _discord.StartAsync();                                // Connect to the websocket

        //await BoiteAIdeeService.UpdateBoiteIdees();
        //_discord.Ready += this.RegisterSlashCommand;
    }

    //private async Task RegisterSlashCommand()
    //{
    //    var srv = _provider.GetService<RegisterSlashCommandService>();
    //    if (srv == null)
    //    {
    //        throw new ArgumentException("RegisterSlashCommandService is not initialized");
    //    }

    //    await srv.RegisterSlashCommand();
    //}
}
