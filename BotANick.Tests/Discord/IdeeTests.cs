using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;
using BotANick.Discord.Modeles;
using BotANick.Discord.Services;
using BotANick.Core.Data.Constantes;
using BotANick.Core.Data;
using Discord;
using Discord.WebSocket;
using System.Linq;

namespace BotANick.Tests.Discord
{
    public class IdeeTests
    {
        private const int _defaultIdeeId = 0;
        private const int _defaultNombreVotes = 0;
        private const string _defaultDescription = null;
        private const string _defaultCreateurName = null;
        private const uint _defaultColorValue = 16278528u;
        private const ulong _defaultIdBoiteChannel = 793819950355185694UL;
        private const string _defaultUpVoteChar = "⬆️";
        private const string _defaultEnCoursChar = "🔧";
        private const string _defaultTermineChar = "🏁";
        private const string _defaultRejectedChar = "❌";
        private const string _expectedBuilder = "Boîte à idées, , #F86400, , Nombre de votes, 0, État de l'idée, 0, Initiateur de l'idée, Aucun créateur, Date de création de l'idée, 01/01/0001, ";
        private readonly ulong? _defaultIdMsgDiscord = null;
        private readonly DateTime _defaultDateCreation = new DateTime();
        private readonly EtatsIdees _defaultEtatIdee = 0;

        [Fact]
        public void ShouldHaveInheritedProperties()
        {
            var idee = new Idee();
            idee.IdeeId.Should().Be(_defaultIdeeId);
            idee.Description.Should().Be(_defaultDescription);
            idee.DateCreation.Should().Be(_defaultDateCreation);
            idee.EtatIdee.Should().Be(_defaultEtatIdee);
            idee.NombreVotes.Should().Be(_defaultNombreVotes);
            idee.Createur.Should().Be(_defaultCreateurName);
            idee.IdMsgDiscord.Should().Be(_defaultIdMsgDiscord);
        }

        [Fact]
        public void ShouldHaveDiscordProperties()
        {
            var ideeContext = new BoiteAIdee();

            ideeContext.ColorIdee.RawValue.Should().Be(_defaultColorValue);
            ideeContext.IdBoiteChannel.Should().Be(_defaultIdBoiteChannel);
            ideeContext.EmoteUpVote.ToString().Should().Be(_defaultUpVoteChar);
            ideeContext.EmoteEtatEnCours.ToString().Should().Be(_defaultEnCoursChar);
            ideeContext.EmoteEtatTermine.ToString().Should().Be(_defaultTermineChar);
            ideeContext.EmoteEtatRejete.ToString().Should().Be(_defaultRejectedChar);
        }

        [Fact]
        public void ShouldUpdateIdMsgDiscord()
        {
            var idee = new Idee();
            const int expectedIdMsgDiscord = 0;
            idee.SetIdMsgDiscord(expectedIdMsgDiscord);

            idee.IdMsgDiscord.Should().Be(expectedIdMsgDiscord);
        }

        [Fact]
        public void ShouldClearIdMsgDiscord()
        {
            const int initialIdMsgDiscord = 0;
            ulong? expectedIdMsgDiscord = null;
            var idee = new Idee()
            {
                IdMsgDiscord = initialIdMsgDiscord,
            };
            idee.ClearIdMsgDiscord();

            idee.IdMsgDiscord.Should().Be(expectedIdMsgDiscord);
        }

        [Fact]
        public void ShouldCreateBuilderFromIdee()
        {
            var idee = new Idee();

            var builder = idee.GetBuilder();
            builder.ToStringCustom().Should().Be(_expectedBuilder);
        }

        [Fact]
        public void ShouldSetEtatEnCours()
        {
            var idee = new Idee();

            idee.SetEtatEnCours();

            idee.EtatIdee.Should().Be(EtatsIdees.EnCours);
        }

        [Fact]
        public void ShouldSetEtatEnCoursMarkAsModified()
        {
            var idee = new Idee();

            idee.SetEtatEnCours();

            idee.IsModified().Should().BeTrue();
        }

        [Fact]
        public void ShouldSetEtatFaite()
        {
            var idee = new Idee();

            idee.SetEtatFaite();

            idee.EtatIdee.Should().Be(EtatsIdees.Faite);
        }

        [Fact]
        public void ShouldSetEtatFaiteMarkAsModified()
        {
            var idee = new Idee();

            idee.SetEtatFaite();

            idee.IsModified().Should().BeTrue();
        }

        [Fact]
        public void ShouldSetEtatRejetee()
        {
            var idee = new Idee();

            idee.SetEtatRejetee();

            idee.EtatIdee.Should().Be(EtatsIdees.Rejetee);
        }

        [Fact]
        public void ShouldSetEtatRejeteeMarkAsModified()
        {
            var idee = new Idee();

            idee.SetEtatRejetee();

            idee.IsModified().Should().BeTrue();
        }

        [Fact]
        public void ShouldSetSameEtatNotMarkAsModified()
        {
            var idee = new Idee
            {
                EtatIdee = EtatsIdees.EnCours,
            };

            idee.SetEtatEnCours();

            idee.IsModified().Should().BeFalse();
        }

        [Fact]
        public void ShouldSetEtatEnCoursBasedOnEmote()
        {
            var idee = new Idee();
            var ideeContext = new BoiteAIdee();

            var emotes = new List<IEmote>
            {
                ideeContext.EmoteEtatEnCours
            };

            idee.SetEtatBasedOnEmotes(emotes);

            idee.EtatIdee.Should().Be(EtatsIdees.EnCours);
        }

        [Fact]
        public void ShouldSetEtatRejeteeBasedOnEmote()
        {
            var idee = new Idee();
            var ideeContext = new BoiteAIdee();

            var emotes = new List<IEmote>
            {
                ideeContext.EmoteEtatRejete
            };

            idee.SetEtatBasedOnEmotes(emotes);

            idee.EtatIdee.Should().Be(EtatsIdees.Rejetee);
        }

        [Fact]
        public void ShouldSetEtatTermineeBasedOnEmote()
        {
            var idee = new Idee();
            var ideeContext = new BoiteAIdee();

            var emotes = new List<IEmote>
            {
                ideeContext.EmoteEtatTermine
            };

            idee.SetEtatBasedOnEmotes(emotes);

            idee.EtatIdee.Should().Be(EtatsIdees.Faite);
        }

        [Fact]
        public void ShouldUpdateNbVote()
        {
            var idee = new Idee();
            const int expectedNbVote = 1;
            idee.SetNbVote(expectedNbVote);

            idee.NombreVotes.Should().Be(expectedNbVote);
        }

        [Fact]
        public void ShouldUpdateNbVoteMarkAsModified()
        {
            var idee = new Idee();
            const int expectedNbVote = 1;
            idee.SetNbVote(expectedNbVote);

            idee.IsModified().Should().BeTrue();
        }

        [Fact]
        public void ShouldUpdateSameNbVoteNotMarkAsModified()
        {
            const int expectedNbVote = 1;
            var idee = new Idee
            {
                NombreVotes = expectedNbVote,
            };
            idee.SetNbVote(expectedNbVote);

            idee.IsModified().Should().BeFalse();
        }

        [Fact]
        public void ShouldArchiveIdee()
        {
            var idee = new Idee();
            idee.IsArchived.Should().BeFalse();

            idee.Archive();

            idee.IsArchived.Should().BeTrue();
        }

        [Fact]
        public void ShouldIdeeNotHasToBeArchived()
        {
            var ideeEnCours = new Idee()
            {
                EtatIdee = EtatsIdees.EnCours,
            };

            var ideeSoumis = new Idee()
            {
                EtatIdee = EtatsIdees.Soumise,
            };

            ideeEnCours.HasToBeArchived().Should().BeFalse();
            ideeSoumis.HasToBeArchived().Should().BeFalse();
        }

        [Fact]
        public void ShouldIdeeHasToBeArchived()
        {
            var ideeRejetee = new Idee()
            {
                EtatIdee = EtatsIdees.Rejetee,
            };

            var ideeFaite = new Idee()
            {
                EtatIdee = EtatsIdees.Faite,
            };

            ideeRejetee.HasToBeArchived().Should().BeTrue();
            ideeFaite.HasToBeArchived().Should().BeTrue();
        }

        [Fact]
        public void ShouldIdeeNotBeArchived()
        {
            var ideeEnCours = new Idee()
            {
                EtatIdee = EtatsIdees.EnCours,
            };

            var ideeSoumis = new Idee()
            {
                EtatIdee = EtatsIdees.Soumise,
            };

            ideeEnCours.UpdateArchive();
            ideeSoumis.UpdateArchive();
            ideeEnCours.IsArchived.Should().BeFalse();
            ideeSoumis.IsArchived.Should().BeFalse();
        }

        [Fact]
        public void ShouldIdeeBeArchived()
        {
            var ideeRejetee = new Idee()
            {
                EtatIdee = EtatsIdees.Rejetee,
            };

            var ideeFaite = new Idee()
            {
                EtatIdee = EtatsIdees.Faite,
            };

            ideeRejetee.UpdateArchive();
            ideeFaite.UpdateArchive();
            ideeRejetee.IsArchived.Should().BeTrue();
            ideeFaite.IsArchived.Should().BeTrue();
        }

        [Fact]
        public void ShouldCreateIdeeFromMessage()
        {
            const string _expectedDescription = "This is the description.";
            const string _expectedCreatorName = "Creator's Name";
            string expectedIdeeString = $"1, Creator's Name, {DateTime.Now.Date}, This is the description., Soumise, , 0";

            using (var dbContext = new DataInMemoryContext())
            {
                Idee newIdee = null;
                newIdee = IdeeExtension.AddIdeeInDbContext(dbContext, _expectedDescription, _expectedCreatorName);
                dbContext.SaveChanges();

                var expectedIdee = dbContext.Idee.FirstOrDefault(i => i.IdeeId == newIdee.IdeeId);
                expectedIdee.ToStringCustom().Should().Be(expectedIdeeString);
            }
        }

        [Fact]
        public void ShouldGetAllIdeeNotArchivedFromBoite()
        {
            var boite = new BoiteAIdee();
            Idee newIdeeNotArchived = new Idee();
            Idee newIdeeArchived = new Idee()
            {
                IsArchived = true,
            };

            List<Idee> expectedIdeesList = new List<Idee> { newIdeeNotArchived };

            using (var dbContext = new DataInMemoryContext())
            {
                dbContext.Idee.Add(newIdeeArchived);
                dbContext.Idee.Add(newIdeeNotArchived);
                dbContext.SaveChanges();

                var ideesFromBoite = boite.GetAllIdees(dbContext).ToList();

                ideesFromBoite.Should().BeEquivalentTo(expectedIdeesList);
            }
        }
    }
}
