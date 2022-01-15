namespace BotANick.Twitch.Commands;

public class CommandService
{
    public DateTime? LastHydrate = null;
    private readonly TwitchClient _client;
    private string channel = string.Empty;

    public CommandService(TwitchClient client)
    {
        this._client = client;

        _client.OnMessageReceived += Client_OnMessageReceived;
    }

    public void WriteInChat(string msg)
    {
        _client.SendMessage(channel, msg);
    }

    public string GetCommand(string msg)
    {
        if (msg.StartsWith("!"))
        {
            return msg[1..];
        }

        return string.Empty;
    }

    private void DoAction(OnMessageReceivedArgs e)
    {
        string message = e.ChatMessage.Message;
        this.channel = e.ChatMessage.Channel;

        string command = GetCommand(message);

        if (command == null)
        {
            return;
        }

        //DiscordCommands.Execute(command, writeSrv);
        TextCommandModel textCommand = new TextCommandModel(this);
        textCommand.Execute(command);
    }

    private void Client_OnMessageReceived(object? sender, OnMessageReceivedArgs e)
    {
        var displayName = e.ChatMessage.DisplayName;
        var msg = e.ChatMessage.Message;
        _ = Discord.Services.TwitchLogsService.LogTwitchChat(displayName, msg);

        DoAction(e);
    }
}
