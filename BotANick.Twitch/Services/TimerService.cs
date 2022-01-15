namespace BotANick.Twitch.Services;

public class TimerService
{
    private readonly TwitchClient _client;
    private readonly TwitchApiService _apiService;

    public TimerService(TwitchClient client, TwitchApiService apiSrv)
    {
        this._client = client;
        this._apiService = apiSrv;
        StartClocks();
    }

    private void StartClocks()
    {
        var timeSpanPub = new TimeSpan(0, 30, 0);

        _ = new Clock(PubDiscord, timeSpanPub, "Pub Discord");

        var timeSpanStreamStarted = new TimeSpan(0, 5, 0);

        _ = new Clock(_apiService.CheckStreamStarted, timeSpanStreamStarted, "Stream Start");
    }

    private void PubDiscord()
    {
        var channel = _client.GetJoinedChannel("Kitarsh");
        _client.SendMessage(channel, "Rejoignez le Discord de la communauté : https://discord.gg/PjNqJSY9E6. On y parle de Guild Wars ! Kappa");
    }
}
