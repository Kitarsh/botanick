using System;
using System.Collections.Generic;
using System.Linq;
using BotANick.Core.Data;
using Discord;
using BotANick.Core.Data.Constantes;
using Microsoft.EntityFrameworkCore;

namespace BotANick.Discord.Modeles
{
    public static class IdeeExtension
    {
        public static string ToStringCustom(this Idee idee)
        {
            return $"{idee.IdeeId}, {idee.Createur}, {idee.DateCreation.Date}, {idee.Description}, {idee.EtatIdee}, {idee.IdMsgDiscord}, {idee.NombreVotes}";
        }

        public static void SetIdMsgDiscord(this Idee idee, ulong newIdMsgDiscord)
        {
            idee.IdMsgDiscord = newIdMsgDiscord;
            return;
        }

        public static EmbedBuilder GetBuilder(this Idee idee)
        {
            var ideeContext = new BoiteAIdee();
            var builder = new EmbedBuilder()
            {
                Color = ideeContext.ColorIdee,
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
                f.Value = idee.Createur ?? "Aucun créateur";
            });

            builder.AddField(f =>
            {
                f.IsInline = true;
                f.Name = "Date de création de l'idée";
                f.Value = idee.DateCreation.ToString("d");
            });

            return builder;
        }

        public static void SetEtatEnCours(this Idee idee)
        {
            idee.SetEtat(EtatsIdees.EnCours);
        }

        public static void SetEtatFaite(this Idee idee)
        {
            idee.SetEtat(EtatsIdees.Faite);
        }

        public static void SetEtatRejetee(this Idee idee)
        {
            idee.SetEtat(EtatsIdees.Rejetee);
        }

        public static void SetEtatBasedOnEmotes(this Idee idee, List<IEmote> reactionLists)
        {
            var ideeContext = new BoiteAIdee();
            if (reactionLists.Any(r => r.Name == ideeContext.EmoteEtatTermine.Name))
            {
                idee.SetEtatFaite();
            }
            else if (reactionLists.Any(r => r.Name == ideeContext.EmoteEtatRejete.Name))
            {
                idee.SetEtatRejetee();
            }
            else if (reactionLists.Any(r => r.Name == ideeContext.EmoteEtatEnCours.Name))
            {
                idee.SetEtatEnCours();
            }
        }

        public static void SetNbVote(this Idee idee, int newNbVotes)
        {
            if (idee.NombreVotes == newNbVotes)
            {
                return;
            }

            idee.NombreVotes = newNbVotes;
            idee.Modify();
        }

        public static void SetNbVotesBasedOnEmotes(this Idee idee, IReadOnlyDictionary<IEmote, ReactionMetadata> reactions)
        {
            var ideeContext = new BoiteAIdee();

            var reaction = reactions.FirstOrDefault(r => r.Key.Name == ideeContext.EmoteUpVote.Name);

            idee.SetNbVote(reaction.Value.ReactionCount);
        }

        public static Idee AddIdeeInDbContext(IDataContext dbContext, string description, string creator)
        {
            var idee = new Idee
            {
                Createur = creator,
                DateCreation = DateTime.Now,
                Description = description,
                EtatIdee = EtatsIdees.Soumise,
            };

            dbContext.Idee.Add(idee);
            return idee;
        }

        public static bool CheckIsOverLimit(this DbSet<Idee> dbSet)
        {
            return dbSet.Count() > 100;
        }

        private static void SetEtat(this Idee idee, EtatsIdees newEtat)
        {
            if (idee.EtatIdee == newEtat)
            {
                return;
            }

            idee.EtatIdee = newEtat;
            idee.Modify();
        }
    }
}
