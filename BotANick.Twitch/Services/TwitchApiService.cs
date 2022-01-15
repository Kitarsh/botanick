namespace BotANick.Twitch.Services;

public class TwitchApiService
{
    private static bool streamIsOn = false;
    private readonly TwitchAPI _api;

    private readonly IConfigurationRoot _config;

    public TwitchApiService(IConfigurationRoot config)
    {
        this._config = config;
        this._api = InitApiConnection();
    }

    public async Task CheckStreamStarted()
    {
        var streamState = await IsStreaming();
        if (streamIsOn != streamState)
        {
            streamIsOn = streamState;
            if (streamState)
            {
                var streamTitle = await GetStreamTitle();
                var msg = $"{streamTitle} Rejoignez le stream sur http://www.twitch.tv/kitarsh !";
                _ = Discord.Services.TwitchLogsService.LogStreamStart();
            }
        }
    }

    private TwitchAPI InitApiConnection()
    {
        ApiSettings settings = new();
        settings.ClientId = _config["Api:ClientId"];
        settings.Secret = _config["Api:Secret"];
        TwitchAPI api = new(settings: settings);

        var scopes = new[] { TwitchLib.Api.Core.Enums.AuthScopes.Any };

        api.Settings.AccessToken = api.Auth.GetAccessToken();

        return api;
    }

    private async Task<bool> IsStreaming()
    {
        bool isStreaming = await _api.V5.Streams.BroadcasterOnlineAsync(_config["Api:StreamId"]);
        return isStreaming;
    }

    private async Task<string> GetStreamTitle()
    {
        var channel = await _api.Helix.Channels.GetChannelInformationAsync(_config["Api:StreamId"]);
        var title = channel.Data[0].Title;
        return title;
    }
}
