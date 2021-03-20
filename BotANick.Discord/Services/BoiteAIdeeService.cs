using BotANick.Core.Data;
using BotANick.Core.Data.Constantes;
using Discord;
using Discord.WebSocket;
using System.Linq;
using System;
using System.Threading.Tasks;
using Discord.Rest;
using BotANick.Discord.Modeles;
using System.Collections.Generic;

namespace BotANick.Discord.Services
{
    public static class BoiteAIdeeService
    {
        private readonly static BoiteAIdee _boiteAIdee = new BoiteAIdee();

        private static DiscordSocketClient _discord;

        public static void SetDiscordClient(DiscordSocketClient client)
        {
            _discord = client;
        }

        public static async Task<Idee> AddIdeeFromMessage(SocketUserMessage msg, string descriptionIdee)
        {
            using (var dbContext = new SqlLiteContext())
            {
                var newIdee = IdeeExtension.AddIdeeInDbContext(dbContext, descriptionIdee, msg.Author.Username);
                newIdee.SetIdMsgDiscord(await ShowIdeeInBoite(newIdee));
                dbContext.SaveChanges();
                return newIdee;
            }
        }

        public static async Task<ulong> ShowIdeeInBoite(Idee idee)
        {
            var boiteChannel = _discord.GetChannel(_boiteAIdee.IdBoiteChannel) as SocketTextChannel;
            var builder = idee.GetBuilder();

            var msg = await MessageService.WriteInChannel(boiteChannel, builder);

            await msg.AddReactionAsync(_boiteAIdee.EmoteUpVote);
            return msg.Id;
        }

        public static async Task UpdateBoiteIdees()
        {
            using var dbContext = new SqlLiteContext();
            var boiteChannel = _discord.GetChannel(_boiteAIdee.IdBoiteChannel) as SocketTextChannel;
            while (boiteChannel == null)
            {
                await Task.Delay(1000);
                Console.WriteLine("HaveWait before UpdateBoiteIdees.");
                boiteChannel = _discord.GetChannel(_boiteAIdee.IdBoiteChannel) as SocketTextChannel;
            }

            if (dbContext.Idee.CheckIsOverLimit())
            {
                await MessageService.WriteInChannel(boiteChannel, "Il y a trop d'idée dans la boîte à idée ! L'update ne fonctionne pas.");
                return;
            }

            var idees = dbContext.Idee.ToEnumerable()
                                      .ToList();

            // Les idées sans message Discord liés.
            var ideesSansMsgDiscord = dbContext.Idee.ToEnumerable()
                                                    .Where(i => i.IdMsgDiscord == null)
                                                    .ToList();

            var msgsEnumerable = await boiteChannel.GetMessagesAsync(100).FlattenAsync();
            var msgs = msgsEnumerable.OrderBy(msg => msg.CreatedAt)
                                     .ToList();

            var idMsgs = msgs.Select(msg => msg.Id)
                             .ToList();

            // Les idées déjà liés à des messages qui n'existent plus.
            var ideesSansMessage = idees.Where(idee => idee.IdMsgDiscord != null && !idMsgs.Contains(idee.IdMsgDiscord.Value))
                                        .ToList();

            ideesSansMsgDiscord = ideesSansMsgDiscord.Union(ideesSansMessage)
                                                     .ToList();

            foreach (var idee in ideesSansMsgDiscord)
            {
                idee.SetIdMsgDiscord(await ShowIdeeInBoite(idee));
            }
            dbContext.SaveChanges();

            foreach (var idee in idees)
            {
                var msgIdee = await boiteChannel.GetMessageAsync(idee.IdMsgDiscord.Value);
                UpdateNombreVoteIdee(idee, msgIdee);
                UpdateEtatIdee(idee, msgIdee);
                if (idee.IsModified())
                {
                    var msg = msgs.FirstOrDefault(msg => msg.Id == idee.IdMsgDiscord) as RestUserMessage;
                    await msg.ModifyAsync(m =>
                    {
                        m.Embed = idee.GetBuilder().Build();
                    });
                }
            }

            dbContext.SaveChanges();
        }

        /// <summary>
        /// Met à jour le nombre de vote pour l'idée dans la base de données.
        /// </summary>
        /// <param name="idee">L'idée.</param>
        /// <param name="messageIdee">Le message Discord de l'idée.</param>
        /// <returns>Indique si un update est nécessaire.</returns>
        public static void UpdateNombreVoteIdee(Idee idee, IMessage messageIdee)
        {
            if (idee.IdMsgDiscord == null)
            {
                return;
            }

            idee.SetNbVotesBasedOnEmotes(messageIdee.Reactions);
        }

        public static void UpdateEtatIdee(Idee idee, IMessage messageIdee)
        {
            if (idee.IdMsgDiscord == null)
            {
                return;
            }

            var emotes = messageIdee.Reactions.Select(r => r.Key).ToList();
            idee.SetEtatBasedOnEmotes(emotes);
        }
    }
}
