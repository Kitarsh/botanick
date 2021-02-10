using Discord;
using Discord.Rest;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace BotANick.Discord.Services
{
    public static class MessageService
    {
        public static async Task<RestUserMessage> WriteInChannel(SocketTextChannel channel, string msg)
        {
            var restMsg = await channel.SendMessageAsync(msg);
            return restMsg;
        }

        public static async Task<RestUserMessage> WriteInChannel(SocketTextChannel channel, EmbedBuilder builder)
        {
            var restMsg = await channel.SendMessageAsync("", false, builder.Build());
            return restMsg;
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
