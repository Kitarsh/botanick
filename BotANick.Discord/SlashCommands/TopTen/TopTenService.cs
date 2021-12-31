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
            { TopTenGeneralCommandModel.GenerateGameStartSubCmd(),  GameStartCmd},
            { TopTenGeneralCommandModel.GenerateRegisterSubCmd(),  RegisterCmd},
            { TopTenGeneralCommandModel.GeneratePlaySubCmd(),  PlayCmd},
        };

        Register();

        discord.SlashCommandExecuted += TopTenCmdHandler;
    }

    #region Commands

    private async Task GameStartCmd(SocketSlashCommand cmd)
    {
        _toptenGame = new TopTenGameModel();

        await cmd.RespondAsync("Inscrivez-vous avec la commande **/topten register** ! \n \n Les règles du jeu sont en ligne : https://www.cocktailgames.com/wp-content/uploads/2020/01/Top_ten_regles_BD.pdf'");
    }

    private async Task RegisterCmd(SocketSlashCommand cmd)
    {
        var nickname = await GetNickname(cmd.Channel, cmd.User);
        _toptenGame.RegisterUser(nickname);

        await cmd.RespondAsync($"Tu es enregistré sous le nom {nickname} !", ephemeral: true);
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
        EmbedBuilder builder = EmbedBuilderService.GenerateBuilderForNumberDisplay(_toptenGame, theme);

        await cmd.FollowupAsync(embeds: new Embed[] { builder.Build() });
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

        await cmd.Value.Invoke(command);
    }

    #endregion InternalServices
}
