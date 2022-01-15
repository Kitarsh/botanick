namespace BotANick.Discord.SlashCommands.Global;

public class GlobalService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DiscordSocketClient _discord;
    private readonly IConfigurationRoot _config;
    private readonly IDictionary<ICommand, Func<SocketSlashCommand, Task>> commands;

    public GlobalService(
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
            {GlobalCommandModel.Hug(), HugCmd },
            {GlobalCommandModel.Noraj(), Noraj },
            {GlobalCommandModel.GiveUp(), GiveUpCmd },
            {GlobalCommandModel.Indelivrables(), IndelivrablesCmd },
        };

        Register();

        discord.SlashCommandExecuted += GlobalSlhCmdHandler;
    }

    #region Commands

    private async Task HugCmd(SocketSlashCommand cmd)
    {
        int teddy_bear = 0x1F9F8; // U+1F9F8.
        var emojis = new List<string>
            {
                "\u2764",
                "\uD83D\uDD25",
                Char.ConvertFromUtf32(teddy_bear),
            };

        var rng = new Random();
        var emoji = emojis.OrderBy(e => rng.Next())
                          .FirstOrDefault();
        await cmd.RespondAsync(emoji);
    }

    private async Task GiveUpCmd(SocketSlashCommand cmd)
    {
        await cmd.RespondAsync("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
    }

    private async Task Noraj(SocketSlashCommand cmd)
    {
        await cmd.DeferAsync(true);
        var msgsEnumerable = await cmd.Channel.GetMessagesAsync(1, CacheMode.AllowDownload)
                                              .FlattenAsync();
        var msgs = msgsEnumerable.OrderByDescending(msg => msg.CreatedAt)
                                 .ToList();

        var messageToReact = msgs[0]; // Reacts to the message before the command.

        await messageToReact.AddReactionAsync(new Emoji("🇳"));
        await messageToReact.AddReactionAsync(new Emoji("🇴"));
        await messageToReact.AddReactionAsync(new Emoji("🇷"));
        await messageToReact.AddReactionAsync(new Emoji("🇦"));
        await messageToReact.AddReactionAsync(new Emoji("🇯"));

        await cmd.FollowupAsync("Done !", ephemeral: true);
    }

    private async Task IndelivrablesCmd(SocketSlashCommand cmd)
    {
        await cmd.RespondAsync("Allez tous regarder la chaîne YouTube des Indélivrables : https://www.youtube.com/channel/UCl7djHZZcnOt-t05QMYx90g");
    }

    #endregion Commands

    #region InternalServices

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
            slashSrv.applicationGlobalCommandProperties.Add(cmdDetail.GenerateSlashCommandBuilder().Build());
        }
    }

    private async Task GlobalSlhCmdHandler(SocketSlashCommand command)
    {
        if (!commands.Any(cmd => cmd.Key.Name == command.Data.Name))
        {
            return;
        }
        var cmd = commands.FirstOrDefault(cmd => cmd.Key.Name == command.Data.Name);

        await cmd.Value.Invoke(command);
    }

    #endregion InternalServices
}
