namespace BotANick.Discord.Services;

public class RegisterSlashCommandService
{
    public readonly List<ApplicationCommandProperties> applicationGlobalCommandProperties;
    public readonly List<ApplicationCommandProperties> applicationPaniniSrvCommandProperties;
    private readonly DiscordSocketClient _discord;
    private readonly IConfigurationRoot _config;

    // DiscordSocketClient, CommandService, and IConfigurationRoot are injected automatically from the IServiceProvider
    public RegisterSlashCommandService(
        DiscordSocketClient discord,
        IConfigurationRoot config)
    {
        _config = config;
        _discord = discord;

        applicationGlobalCommandProperties = new();
        applicationPaniniSrvCommandProperties = new();

        _discord.Ready += RegisterSlashCommand; // Comment this line in production
    }

    public async Task RegisterSlashCommand()
    {
        if (_discord == null)
        {
            throw new ArgumentNullException("Discord Client not initialized when registering Slash Commands");
        }

        //await RegisterCmdGlobal(applicationGlobalCommandProperties); // Comment in dev if you do not want to change the global commands

        await RegisterCmdPaniniServer(applicationPaniniSrvCommandProperties);
    }

    private async Task RegisterCmdGlobal(List<ApplicationCommandProperties> applicationGlobalCommandProperties)
    {
        try
        {
            Console.WriteLine($"Registering {applicationGlobalCommandProperties.Count()} slash commands.");
            await _discord.BulkOverwriteGlobalApplicationCommandsAsync(applicationGlobalCommandProperties.ToArray());
            Console.WriteLine("Slash commands successfully registered !");
        }
        catch (HttpException ex)
        {
            // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
            var json = JsonConvert.SerializeObject(ex.Errors, Formatting.Indented);

            // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
            Console.WriteLine(json);
        }
    }

    private async Task RegisterCmdPaniniServer(List<ApplicationCommandProperties> applicationPaniniSrvCommandProperties)
    {
        var guildId = Convert.ToUInt64(_config["guildsId:panini"]);
        var guild = _discord.GetGuild(guildId);
        if (guild == null)
        {
            Console.WriteLine("Did not find panini server !");
            return;
        }

        try
        {
            Console.WriteLine($"Registering {applicationPaniniSrvCommandProperties.Count()} slash commands to Panini server.");
            await guild.BulkOverwriteApplicationCommandAsync(applicationPaniniSrvCommandProperties.ToArray());
            Console.WriteLine("Slash commands for Panini server successfully registered !");
        }
        catch (HttpException ex)
        {
            // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
            var json = JsonConvert.SerializeObject(ex.Errors, Formatting.Indented);

            // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
            Console.WriteLine(json);
        }
    }
}
