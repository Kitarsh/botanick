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
            await UpdateIdees(dbContext, boiteChannel);
        }

        private static async Task UpdateIdees(SqlLiteContext dbContext, SocketTextChannel boiteChannel)
        {
            var idees = _boiteAIdee.GetAllIdees(dbContext);
            List<IMessage> msgs = await GetAllMsgs(boiteChannel);

            await AddMessagesForMissingIdee(idees, msgs);
            dbContext.SaveChanges();

            foreach (var idee in idees)
            {
                var msgIdee = msgs.FirstOrDefault(msg => msg.Id == idee.IdMsgDiscord);
                idee.UpdateIdee(msgIdee);
                await UpdateDiscordMessage(idee, msgIdee);
            }

            dbContext.SaveChanges();
        }

        private static async Task UpdateDiscordMessage(Idee idee, IMessage msgIdee)
        {
            if (idee.IsArchived && msgIdee is IUserMessage msgToDelete)
            {
                await msgToDelete.DeleteAsync();
                idee.ClearIdMsgDiscord();
            }
            else if (idee.IsModified() && msgIdee is IUserMessage msgToUpdate)
            {
                await msgToUpdate.ModifyAsync(m => { m.Embed = idee.GetBuilder().Build(); });
            }
        }

        private static async Task<List<IMessage>> GetAllMsgs(SocketTextChannel boiteChannel)
        {
            var msgsEnumerable = await boiteChannel.GetMessagesAsync(100).FlattenAsync();
            var msgs = msgsEnumerable.OrderBy(msg => msg.CreatedAt)
                                     .ToList();
            return msgs;
        }

        private static async Task AddMessagesForMissingIdee(IEnumerable<Idee> idees, IEnumerable<IMessage> msgs)
        {
            var ideesSansMsgDiscord = idees.Where(i => i.IdMsgDiscord == null)
                                           .ToList();

            var idMsgs = msgs.Select(msg => msg.Id).ToList();

            var ideesSansMessage = idees.Where(idee => idee.IdMsgDiscord != null && !idMsgs.Contains(idee.IdMsgDiscord.Value)).ToList();

            ideesSansMsgDiscord = ideesSansMsgDiscord.Union(ideesSansMessage).ToList();

            foreach (var idee in ideesSansMsgDiscord)
            {
                idee.SetIdMsgDiscord(await ShowIdeeInBoite(idee));
            }
        }
    }
}
