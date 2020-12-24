using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace botanick.Services
{
    public static class WriteInChannel
    {
        public static async Task WriteInBotSpam(SocketTextChannel channel, string msg)
        {
            await channel.SendMessageAsync(msg);
        }

        public static async Task ReactWithEmoteAsync(SocketUserMessage userMsg, string escapedEmote)
        {
            if (Emote.TryParse(escapedEmote, out var emote))
            {
                await userMsg.AddReactionAsync(emote);
            }
        }
    }
}
