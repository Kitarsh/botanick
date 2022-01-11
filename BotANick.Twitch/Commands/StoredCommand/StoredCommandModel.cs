namespace BotANick.Twitch.Commands.StoredCommand;

public class StoredCommandModel : TwitchCommand
{
    public StoredCommandModel(TwitchCommand tc)
    {
        this.TwitchCommandId = tc.TwitchCommandId;
        this.Command = tc.Command;
        this.Reponse = tc.Reponse;
    }

    public static List<StoredCommandModel> GetAllCommandsStored()
    {
        var dbContext = new BotANickContext();

        var storedCommands = dbContext.TwitchCommands
                                      .AsEnumerable()
                                      .Select(tc => new StoredCommandModel(tc))
                                      .ToList();

        return storedCommands;
    }
}
