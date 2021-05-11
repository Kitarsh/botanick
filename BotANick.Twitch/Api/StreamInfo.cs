using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BotANick.Twitch.Api
{
    public static class StreamInfo
    {
        /// <summary>
        /// L'identifiant de la chaîne Twitch.
        /// </summary>
        private static string _streamId = "57882627";

        private static bool streamIsOn = false;

        /// <summary>
        /// Récupère l'information si le Stream est en ligne.
        /// </summary>
        /// <returns>Booléen indiquant si le stream est en ligne.</returns>
        public async static Task<bool> IsStreaming()
        {
            bool isStreaming = await CoreApi.ApiHandler.V5.Streams.BroadcasterOnlineAsync(_streamId);
            return isStreaming;
        }

        public async static Task<string> GetStreamTitle()
        {
            var channel = await CoreApi.ApiHandler.Helix.Channels.GetChannelInformationAsync(_streamId, CoreApi.ApiHandler.Settings.AccessToken);
            var title = channel.Data[0].Title;
            return title;
        }

        public async static Task CheckStreamStarted()
        {
            var streamState = await IsStreaming();
            if (streamIsOn != streamState)
            {
                streamIsOn = streamState;
                if (streamState)
                {
                    var streamTitle = await GetStreamTitle();
                    Modules.Pub.PubStreamStart(streamTitle);
                }
            }
        }
    }
}
