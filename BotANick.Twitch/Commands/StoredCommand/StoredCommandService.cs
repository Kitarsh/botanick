namespace BotANick.Twitch.Commands.StoredCommand;

public class StoredCommandService
{
    private readonly TwitchClient _client;
    private readonly CommandService _commandService;

    public StoredCommandService(TwitchClient client, CommandService commandService)
    {
        this._client = client;
        this._commandService = commandService;
        _client.OnMessageReceived += Client_OnMessageReceivedAnswerStoredCommand;
    }

    private void Client_OnMessageReceivedAnswerStoredCommand(object? sender, OnMessageReceivedArgs e)
    {
        DoAction(e);
    }

    private void DoAction(OnMessageReceivedArgs e)
    {
        var command = _commandService.GetCommand(e.ChatMessage.Message);

        if (command == null)
        {
            return;
        }

        var storedCommands = StoredCommandModel.GetAllCommandsStored();

        foreach (var storedCommand in storedCommands)
        {
            if (storedCommand.Command.ToLowerInvariant() == command.ToLowerInvariant())
            {
                _commandService.WriteInChat(storedCommand.Reponse);
            }
        }
    }
}
