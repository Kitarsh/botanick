using BotANick.Core.Data;
using BotANick.Core.Data.Constantes;
using Discord;
using Discord.WebSocket;
using System.Linq;
using System;
using System.Threading.Tasks;
using Discord.Rest;

namespace BotANick.Discord.Services
{
    public static class IdeeService
    {
        private static Color _colorIdee = new Color(248, 100, 0);

        private static ulong _idBoiteChannel = 793819950355185694;

        private static Emoji _emoteUpVote = new Emoji("⬆️");

        private static Emoji _emoteEtatEnCours = new Emoji("🔧");

        private static Emoji _emoteEtatTermine = new Emoji("🏁");

        private static Emoji _emoteEtatRejete = new Emoji("❌");

        private static DiscordSocketClient _discord;

        public static void SetDiscordClient(DiscordSocketClient client)
        {
            _discord = client;
        }

        public static Idee AddIdeeFromMessage(SocketUserMessage msg, string descriptionIdee)
        {
            using (var dbcontext = new SqlLiteContext())
            {
                var newIdee = new Idee()
                {
                    Description = descriptionIdee,
                    EtatIdee = EtatsIdees.Soumise,
                    NombreVotes = 0,
                    Createur = msg.Author.Username,
                    DateCreation = msg.CreatedAt.DateTime,
                };

                dbcontext.Idee.Add(newIdee);
                dbcontext.SaveChanges();

                return newIdee;
            }
        }

        public static EmbedBuilder GetBuilderFromIdee(int ideeId)
        {
            using var dbcontext = new SqlLiteContext();
            var idee = dbcontext.Idee
                                .FirstOrDefault(i => i.IdeeId == ideeId);

            var builder = new EmbedBuilder()
            {
                Color = _colorIdee,
                Title = "Boîte à idées",
                Description = idee.Description,
            };

            builder.AddField(f =>
            {
                f.IsInline = true;
                f.Name = "Nombre de votes";
                f.Value = idee.NombreVotes;
            });

            builder.AddField(f =>
            {
                f.IsInline = true;
                f.Name = "État de l'idée";
                f.Value = idee.EtatIdee;
            });

            builder.AddField(f =>
            {
                f.IsInline = true;
                f.Name = "Initiateur de l'idée";
                f.Value = idee.Createur;
            });

            builder.AddField(f =>
            {
                f.IsInline = true;
                f.Name = "Date de création de l'idée";
                f.Value = idee.DateCreation.ToString("d");
            });

            return builder;
        }

        public static EmbedBuilder GetBuilderFromIdee(Idee idee)
        {
            var builder = new EmbedBuilder()
            {
                Color = _colorIdee,
                Title = "Boîte à idées",
                Description = idee.Description,
            };

            builder.AddField(f =>
            {
                f.IsInline = true;
                f.Name = "Nombre de votes";
                f.Value = idee.NombreVotes;
            });

            builder.AddField(f =>
            {
                f.IsInline = true;
                f.Name = "État de l'idée";
                f.Value = idee.EtatIdee;
            });

            builder.AddField(f =>
            {
                f.IsInline = true;
                f.Name = "Initiateur de l'idée";
                f.Value = idee.Createur;
            });

            builder.AddField(f =>
            {
                f.IsInline = true;
                f.Name = "Date de création de l'idée";
                f.Value = idee.DateCreation.ToString("d");
            });

            return builder;
        }

        public static async Task<ulong> ShowIdeeInBoite(Idee idee)
        {
            var boiteChannel = _discord.GetChannel(_idBoiteChannel) as SocketTextChannel;
            var builder = GetBuilderFromIdee(idee);

            var msg = await MessageService.WriteInChannel(boiteChannel, builder);

            await msg.AddReactionAsync(_emoteUpVote);
            return msg.Id;
        }

        public static async Task UpdateBoiteIdees()
        {
            using var dbContext = new SqlLiteContext();
            var boiteChannel = _discord.GetChannel(_idBoiteChannel) as SocketTextChannel;
            while (boiteChannel == null)
            {
                await Task.Delay(1000);
                Console.WriteLine("HaveWait before UpdateBoiteIdees.");
                boiteChannel = _discord.GetChannel(_idBoiteChannel) as SocketTextChannel;
            }

            if (dbContext.Idee.Count() > 100)
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
                idee.IdMsgDiscord = await ShowIdeeInBoite(idee);
            }
            dbContext.SaveChanges();

            foreach (var idee in idees)
            {
                var msgIdee = await boiteChannel.GetMessageAsync(idee.IdMsgDiscord.Value);
                var upvoteHasChanged = UpdateNombreVoteIdee(idee, msgIdee);
                var etatHasChanged = UpdateEtatIdee(idee, msgIdee);
                if (upvoteHasChanged || etatHasChanged)
                {
                    var msg = msgs.FirstOrDefault(msg => msg.Id == idee.IdMsgDiscord) as RestUserMessage;
                    await msg.ModifyAsync(m =>
                    {
                        m.Embed = GetBuilderFromIdee(idee).Build();
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
        public static bool UpdateNombreVoteIdee(Idee idee, IMessage messageIdee)
        {
            if (idee.IdMsgDiscord == null)
            {
                return false;
            }

            var boiteChannel = _discord.GetChannel(_idBoiteChannel) as SocketTextChannel;
            var upvoteReaction = messageIdee.Reactions.FirstOrDefault(r => r.Key.Name == _emoteUpVote.Name);
            var nbUpvote = upvoteReaction.Value.ReactionCount;
            if (nbUpvote == idee.NombreVotes)
            {
                return false;
            }

            idee.NombreVotes = nbUpvote;
            return true;
        }

        public static bool UpdateEtatIdee(Idee idee, IMessage messageIdee)
        {
            if (idee.IdMsgDiscord == null)
            {
                return false;
            }

            var boiteChannel = _discord.GetChannel(_idBoiteChannel) as SocketTextChannel;

            // Si l'idée est terminée.
            if (messageIdee.Reactions.Any(r => r.Key.Name == _emoteEtatTermine.Name))
            {
                if (idee.EtatIdee != EtatsIdees.Faite)
                {
                    idee.EtatIdee = EtatsIdees.Faite;
                    return true;
                }
                return false;
            }
            else if (messageIdee.Reactions.Any(r => r.Key.Name == _emoteEtatRejete.Name))
            {
                if (idee.EtatIdee != EtatsIdees.Rejetee)
                {
                    idee.EtatIdee = EtatsIdees.Rejetee;
                    return true;
                }
                return false;
            }
            else if (messageIdee.Reactions.Any(r => r.Key.Name == _emoteEtatEnCours.Name))
            {
                if (idee.EtatIdee != EtatsIdees.EnCours)
                {
                    idee.EtatIdee = EtatsIdees.EnCours;
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
