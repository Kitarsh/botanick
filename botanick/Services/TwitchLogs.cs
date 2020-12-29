using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace BotANick.Services
{
    /// <summary>
    /// Permet d'afficher les informations twitch dans le chat Discord.
    /// </summary>
    public static class TwitchLogs
    {
        private static ulong idChannelTwitchChat = 793483776553779241;

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
            await MessageService.WriteInChannel(channel, $"{sendername} : {msg}");
        }
    }
}
