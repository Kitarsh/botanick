using BotANick.Twitch.Interfaces;
using TwitchLib.Client;

namespace BotANick.Twitch.Services
{
    public class WriteService : IWriteService
    {
        private readonly TwitchClient _client;

        private readonly string _channel;

        public WriteService(TwitchClient client, string channel)
        {
            this._client = client;
            this._channel = channel;
        }

        public void WriteInChat(string msg)
        {
            this._client.SendMessage(_channel, msg);
        }
    }
}
