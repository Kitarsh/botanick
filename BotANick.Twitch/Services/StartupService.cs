namespace BotANick.Twitch.Services;

public class StartupService
{
    private readonly IConfigurationRoot _config;
    private readonly TwitchClient _client;

    public StartupService(IConfigurationRoot config, TwitchClient client)
    {
        this._config = config;
        this._client = client;

        this.StartService();
    }

    public void OpenConnection()
    {
        this._client.Connect();
    }

    private void StartService()
    {
        ConnectionCredentials credentials = new(this._config["TwitchClient:Username"], this._config["TwitchClient:OAuthToken"]);
        this._client.Initialize(credentials, _config["TwitchClient:Channel"]);
        this._client.OnLog += Client_OnLog;
        this._client.OnConnected += Client_OnConnected;
        this._client.OnJoinedChannel += Client_OnJoinedChannel;
        this._client.OnWhisperReceived += Client_OnWhisperReceived;
        this._client.OnNewSubscriber += Client_OnNewSubscriber;
    }

    private void Client_OnLog(object? sender, OnLogArgs e)
    {
        Console.WriteLine($"Twitch Bot {e.DateTime}: {e.BotUsername} - {e.Data}");
    }

    private void Client_OnConnected(object? sender, OnConnectedArgs e)
    {
        Console.WriteLine($"Connected to {e.AutoJoinChannel}");
    }

    private void Client_OnJoinedChannel(object? sender, OnJoinedChannelArgs e)
    {
        Console.WriteLine("Hey guys! I am a bot connected via TwitchLib!");
        this._client.SendMessage(e.Channel, "Hello ! Ici BotANick pour vous servir ! Utilisez la commande !help pour apprendre à me connaître.");
    }

    private void Client_OnWhisperReceived(object? sender, OnWhisperReceivedArgs e)
    {
        throw new NotImplementedException();
    }

    private void Client_OnNewSubscriber(object? sender, OnNewSubscriberArgs e)
    {
        if (e.Subscriber.SubscriptionPlan == SubscriptionPlan.Prime)
            this._client.SendMessage(e.Channel, $"Incroyable ! {e.Subscriber.DisplayName} vient d'utiliser son prime sur Kitarsh ! Avec ça, l'éden sera sauf !");
        else
            this._client.SendMessage(e.Channel, $"Qu... QUOI ? MAIS GARDE TON ARGENT {e.Subscriber.DisplayName} ! Si tu veux avoir de l'influence sur ce streameur, va le contacter son Discord plutôt !");
    }
}
