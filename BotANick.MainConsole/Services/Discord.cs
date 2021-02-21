using Dsc = BotANick.Discord;

namespace MainConsole.Services
{
    public static class Discord
    {
        /// <summary>
        /// Indique si le projet Discord a déjà été lancé.
        /// </summary>
        public static bool IsDiscordLaunched { get; set; } = false;

        /// <summary>
        /// Lance une alerte Discord de début de stream.
        /// </summary>
        public static void StreamStart()
        {
            if (!IsDiscordLaunched)
            {
                _ = Dsc.Services.TwitchLogs.LogStreamStart();
            }
        }
    }
}
