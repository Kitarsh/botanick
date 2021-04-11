using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BotANick.Core.Data;
using BotANick.Core.Data.Constantes;
using BotANick.Discord.Services;
using BotANick.Discord.Modeles;

namespace BotANick.Discord.Modules
{
    /// <summary>
    /// Les fonctions Discord pour la fonctionnalité de boîte à idées.
    /// </summary>
    [Group("idee")]
    [Summary("Les commandes Discord pour la boîte à idées.")]
    public class IdeeModule : ModuleBase<SocketCommandContext>
    {
        [Command("add")]
        [Summary("Ajoute une idée dans la boîte à idée.")]
        public async Task AddIdees(string descriptionIdee)
        {
            var idee = await BoiteAIdeeService.AddIdeeFromMessage(Context.Message, descriptionIdee);
            await (new GitHubService()).AddIssueBasedOnIdee(idee);
            await ReplyAsync("", false, idee.GetBuilder().Build());
        }

        [Command("update")]
        [Summary("Mets à jour la boîte à idée.")]
        public async Task UpdateBoiteIdees()
        {
            await BoiteAIdeeService.UpdateBoiteIdees();
        }
    }
}
