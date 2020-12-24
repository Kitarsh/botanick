using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;


namespace botanick.Modules
{
    [Name("Commandes basiques")]
    public class BasicsModule : ModuleBase<SocketCommandContext>
    {
        [Command("say"), Alias("s")]
        [Summary("Fait dire quelque chose au bot")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public Task Say([Remainder] string text)
            => ReplyAsync(text);
    }
}
