using System;
using System.Collections.Generic;
using System.Text;
using Twt = BotANick.Twitch;

namespace MainConsole.Services
{
    public static class Twitch
    {
        /// <summary>
        /// Indique si le projet Twitch a déjà été lancé.
        /// </summary>
        public static bool IsTwitchLaunched { get; set; } = false;

        /// <summary>
        /// Initialise le projet Twitch.
        /// </summary>
        public static void LaunchTwitch()
        {
            if (!IsTwitchLaunched)
            {
                IsTwitchLaunched = true;
                var args = new string[0];
                Twt.Program.Main(args);
                Twt.Api.CoreApi.InitApi();
            }
        }
    }
}
