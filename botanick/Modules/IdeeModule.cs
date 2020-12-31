using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BotANick.Data;
using BotANick.Data.Constantes;
using BotANick.Services;

namespace BotANick.Modules
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
            var idee = IdeeService.AddIdeeFromMessage(Context.Message, descriptionIdee);
            var builder = IdeeService.GetBuilderFromIdee(idee.IdeeId);

            await ReplyAsync("", false, builder.Build());
        }

        [Command("update")]
        [Summary("Mets à jour la boîte à idée.")]
        public async Task UpdateBoiteIdees()
        {
            await IdeeService.UpdateBoiteIdees();
        }
    }
}
