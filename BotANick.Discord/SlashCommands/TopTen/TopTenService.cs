namespace BotANick.Discord.SlashCommands.TopTen;

public class TopTenService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDictionary<SubCommand, Func<SocketSlashCommand, Task>> commands;

    private TopTenGameModel _toptenGame;

    public TopTenService(
            IServiceProvider serviceProvider,
        DiscordSocketClient discord
        )
    {
        _serviceProvider = serviceProvider;

        commands = new Dictionary<SubCommand, Func<SocketSlashCommand, Task>>()
        {
            { TopTenGeneralCommandModel.GenerateGameStartSubCmd(), GameStartCmd},
            { TopTenGeneralCommandModel.GenerateRegisterSubCmd(), RegisterCmd},
            { TopTenGeneralCommandModel.GeneratePlaySubCmd(), PlayCmd},
            { TopTenGeneralCommandModel.GenerateResultsSubCmd(), ResultsCmd},
        };

        Register();

        discord.SlashCommandExecuted += TopTenCmdHandler;
    }

    #region Commands

    private async Task GameStartCmd(SocketSlashCommand cmd)
    {
        _toptenGame = new TopTenGameModel(cmd);

        await cmd.RespondAsync(TopTenGameModel.GameStartMsg);
    }

    private async Task RegisterCmd(SocketSlashCommand cmd)
    {
        var nickname = await GetNickname(cmd.Channel, cmd.User);
        _toptenGame.RegisterUser(nickname, cmd);

        await cmd.RespondAsync($"Tu es enregistré sous le nom {nickname} !", ephemeral: true);
        await _toptenGame.ChangeStartMsgWithRegistered();
    }

    private async Task PlayCmd(SocketSlashCommand cmd)
    {
        await cmd.DeferAsync();
        _toptenGame.NextCapten();

        if (_toptenGame.IndexCapten == null)
        {
            await cmd.RespondAsync("Personne ne joue !");
            return;
        }

        if (_toptenGame.Themes.Count == 0)
        {
            using (var dbContext = new BotANickContext())
            {
                _toptenGame.Themes = TopTenGameModel.GetRandomThemes(dbContext);
            }
        }
        var theme = _toptenGame.GetNextTheme();
        _toptenGame.GenerateNumbers();

        EmbedBuilder builder = EmbedBuilderService.GenerateBuilderForThemeAndCaptenDisplay(_toptenGame, theme);

        await cmd.FollowupAsync(embeds: new Embed[] { builder.Build() });
    }

    private async Task ResultsCmd(SocketSlashCommand cmd)
    {
        var results = _toptenGame.GetResults();

        var builder = EmbedBuilderService.GenerateBuilderForNumberDisplay(results);

        await cmd.RespondAsync(embed: builder.Build());
    }

    #endregion Commands

    private async Task<string> GetNickname(ISocketMessageChannel channel, IUser iu)
    {
        var user = await channel.GetUserAsync(iu.Id, CacheMode.AllowDownload);

        if (user is SocketGuildUser userSocket && !string.IsNullOrEmpty(userSocket.Nickname))
        {
            return userSocket.Nickname;
        }
        else
        {
            return iu.Username;
        }
    }

    #region InternalServices

    private void Register()
    {
        var slashSrv = _serviceProvider.GetService<RegisterSlashCommandService>();
        if (slashSrv == null)
        {
            throw new ArgumentException("RegisterSlashCommandService not launch");
        }
        var topTenCommand = TopTenGeneralCommandModel.GenerateTopTenCommands();
        slashSrv.applicationTopTenSrvCommandProperties.Add(topTenCommand.GenerateSlashCommandBuilder().Build());
    }

    private async Task TopTenCmdHandler(SocketSlashCommand command)
    {
        if (command.CommandName != "topten")
        {
            return;
        }
        var cmd = commands.FirstOrDefault(cmd => cmd.Key.Name == command.Data.Options.FirstOrDefault().Name);

        if (cmd.Equals(default(KeyValuePair<SubCommand, Func<SocketSlashCommand, Task>>)))
        {
            return;
        }

        await cmd.Value.Invoke(command);
    }

    #endregion InternalServices
}
