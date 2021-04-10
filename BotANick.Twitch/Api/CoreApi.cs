using System.Collections.Generic;
using System.Threading.Tasks;

using TwitchLib.Api;
using TwitchLib.Api.V5.Models.Subscriptions;

namespace BotANick.Twitch.Api
{
    public static class CoreApi
    {
        /// <summary>
        /// La Twitch API.
        /// </summary>
        public static TwitchAPI ApiHandler { get; set; }

        /// <summary>
        /// Initialise la Twitch API.
        /// </summary>
        public static void InitApi()
        {
            ApiHandler = new TwitchAPI();
            ApiHandler.Settings.ClientId = Program.Configuration["tokens:Api:ClientId"];
            ApiHandler.Settings.AccessToken = Program.Configuration["tokens:Api:AccessToken"];
        }
    }
}
