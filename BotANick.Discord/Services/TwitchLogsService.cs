using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotANick.Discord.Services
{
    public static class TwitchLogsService
    {
        private static ulong idChannelTwitchChat = 793483776553779241;

        private static ulong idChannelInfoStream = 785206832867573812;

        private static DiscordSocketClient _discord;

        public static void SetDiscordClient(DiscordSocketClient client)
        {
            _discord = client;
        }

        /// <summary>
        /// Envoie un message dans le channel dédié.
        /// </summary>
        /// <param name="sendername">L'utilisateur ayant envoyé le message.</param>
        /// <param name="msg">Le message de l'utilisateur.</param>
        public static async Task LogTwitchChat(string sendername, string msg)
        {
            if (_discord == null)
            {
                Console.WriteLine("Le Bot discord n'est pas lancé ! Le chat Twitch ne peut pas être logé !");
                return;
            }

            var channel = _discord.GetChannel(idChannelTwitchChat) as SocketTextChannel;
            await channel.SendMessageAsync($"{sendername} : {msg}");
        }

        /// <summary>
        /// Envoie un message de démarrage de stream dans le channel dédié.
        /// </summary>
        public static async Task LogStreamStart(string msg = null)
        {
            if (_discord == null)
            {
                Console.WriteLine("Le Bot discord n'est pas lancé !");
                return;
            }

            if (msg == null)
            {
                msg = "Le stream de Kitarsh a démarré ! Rejoignez le sur : http://www.twitch.tv/kitarsh !!";
            }

            var channel = _discord.GetChannel(idChannelInfoStream) as SocketTextChannel;
            var discordMsg = await channel.SendMessageAsync(msg);
            await discordMsg.CrosspostAsync();
        }
    }
}
