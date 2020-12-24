using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using BotANick.Services;

namespace BotANick.Modules
{
    [Name("Informations sur les serveurs")]
    [Group("srv")]
    public class ServersModule : ModuleBase<SocketCommandContext>
    {
        [Command("panini")]
        [Summary("Lien vers le serveur Panini")]
        public Task PaniniServer()
            => ReplyAsync(":punch_tone2::100::100::100::thumbsup_tone2::100::100::100::punch_tone2:DISCORD https://discord.gg/2AG7zbr Le meilleur discord de panini oubliez pas:punch_tone2::100::100::100::thumbsup_tone2::100::100::100::punch_tone2:");
    }
}
