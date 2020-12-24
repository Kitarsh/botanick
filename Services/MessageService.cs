using Discord;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace BotANick.Services
{
    public static class MessageService
    {
        public static async Task WriteInBotSpam(SocketTextChannel channel, string msg)
        {
            await channel.SendMessageAsync(msg);
        }

        public static async Task ReactWithEmoteAsync(SocketUserMessage userMsg, Emoji emote)
        {
            await userMsg.AddReactionAsync(emote);
        }

        public static async Task ReactWithEmoteAsync(SocketUserMessage userMsg, string emoteStrg)
        {
            if (emoteStrg.FirstOrDefault() == '<')
            {
                var emote = Emote.Parse(emoteStrg);
                await userMsg.AddReactionAsync(emote);
            }
            else
            {
                await userMsg.AddReactionAsync(new Emoji(emoteStrg));
            }
        }
    }
}
