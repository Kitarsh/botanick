using System;
using System.Collections.Generic;
using System.Linq;
using BotANick.Core.Data;
using Discord;
using BotANick.Core.Data.Constantes;
using Microsoft.EntityFrameworkCore;
using BotANick.Discord.Services;

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
        }

        public static void ClearIdMsgDiscord(this Idee idee)
        {
            idee.IdMsgDiscord = null;
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

            builder.AddFieldWithValue("Nombre de votes", idee.NombreVotes.ToString());
            builder.AddFieldWithValue("État de l'idée", idee.EtatIdee.ToString());
            builder.AddFieldWithValue("Initiateur de l'idée", idee.Createur ?? "Aucun créateur");
            builder.AddFieldWithValue("Date de création de l'idée", idee.DateCreation.ToString("dd/MM/yyyy"));

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

        public static void Archive(this Idee idee)
        {
            idee.IsArchived = true;
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

        public static bool HasToBeArchived(this Idee idee)
        {
            switch (idee.EtatIdee)
            {
                case EtatsIdees.EnCours:
                case EtatsIdees.Soumise:
                    return false;

                case EtatsIdees.Rejetee:
                case EtatsIdees.Faite:
                    return true;

                default:
                    return false;
            }
        }

        public static bool CheckIsOverLimit(this DbSet<Idee> dbSet)
        {
            return dbSet.Count() > 100;
        }

        public static void UpdateIdee(this Idee idee, IMessage msgIdee)
        {
            if (msgIdee?.Id != idee.IdMsgDiscord)
            {
                return;
            }

            idee.UpdateNombreVoteIdee(msgIdee);
            idee.UpdateEtatIdee(msgIdee);
            idee.UpdateArchive();
        }

        public static void UpdateArchive(this Idee idee)
        {
            if (idee.HasToBeArchived())
            {
                idee.Archive();
            }
        }

        private static void UpdateNombreVoteIdee(this Idee idee, IMessage messageIdee)
        {
            if (idee.IdMsgDiscord == null)
            {
                return;
            }

            idee.SetNbVotesBasedOnEmotes(messageIdee.Reactions);
        }

        private static void UpdateEtatIdee(this Idee idee, IMessage messageIdee)
        {
            if (idee.IdMsgDiscord == null)
            {
                return;
            }

            var emotes = messageIdee.Reactions.Select(r => r.Key).ToList();
            idee.SetEtatBasedOnEmotes(emotes);
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
