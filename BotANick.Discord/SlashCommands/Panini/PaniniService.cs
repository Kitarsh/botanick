namespace BotANick.Discord.SlashCommands.Panini;

public class PaniniService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DiscordSocketClient _discord;
    private readonly IConfigurationRoot _config;
    private readonly IDictionary<ICommand, Func<SocketSlashCommand, Task>> commands;

    public PaniniService(
            IServiceProvider provider,
            IConfigurationRoot config,
        DiscordSocketClient discord
        )
    {
        _serviceProvider = provider;
        _discord = discord;
        _config = config;

        commands = new Dictionary<ICommand, Func<SocketSlashCommand, Task>>()
        {
            {PaniniCommandModel.PaniniServerPub(), PaniniServerPubCmd },
        };

        Register();

        discord.SlashCommandExecuted += PaniniSlhCmdHandler;
    }

    private async Task PaniniServerPubCmd(SocketSlashCommand cmd)
    {
        await cmd.RespondAsync(_config["commands:panini"]);
    }

    private void Register()
    {
        var slashSrv = _serviceProvider.GetService<RegisterSlashCommandService>();
        if (slashSrv == null)
        {
            throw new ArgumentException("RegisterSlashCommandService not launch");
        }

        foreach (var cmd in commands)
        {
            ICommand cmdDetail = cmd.Key;
            slashSrv.applicationPaniniSrvCommandProperties.Add(cmdDetail.GenerateSlashCommandBuilder().Build());
        }
    }

    private async Task PaniniSlhCmdHandler(SocketSlashCommand command)
    {
        if (!commands.Any(cmd => cmd.Key.Name == command.Data.Name))
        {
            return;
        }
        var cmd = commands.FirstOrDefault(cmd => cmd.Key.Name == command.Data.Name);

        await cmd.Value.Invoke(command);
    }
}
