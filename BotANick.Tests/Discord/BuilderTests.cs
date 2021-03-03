using System;
using System.Collections.Generic;
using System.Linq;
using BotANick.Core.Data;
using Discord;
using FluentAssertions;
using Xunit;
using mdl = BotANick.Discord.Modules;
using srv = BotANick.Discord.Services;

namespace BotANick.Tests.Discord
{
    public class BuilderTests
    {
        [Fact]
        public void ShouldReturnTopTenBuilder()
        {
            var args = new mdl.TopTenModule.TopTenModele();

            var builder = srv.EmbedBuilderService.InitBuilder(new List<EmbedFieldBuilder>(), args.ColorTopTen);

            var stringifiedBuilder = srv.ExtensionsEmbedBuilder.ToString(builder);
            stringifiedBuilder.Should().Be("TopTen, , #147786, , , ");
        }

        [Fact]
        public void ShouldReturnTopTenThemeBuilder()
        {
            var args = new mdl.TopTenModule.TopTenModele();
            var builder = srv.EmbedBuilderService.GenerateBuilderForNumberDisplay("toto", args.Users, args.IndexCapten, args.ColorTopTen);

            var stringifiedBuilder = srv.ExtensionsEmbedBuilder.ToString(builder);
            stringifiedBuilder.Should().Be("TopTen, , #147786, , Le thème est le suivant :, toto, Le Cap'TEN est :, No captain, Tirage des numéros :, No players, ");
        }

        [Fact]
        public void ShouldReturnTopTenPlayersBuilder()
        {
            var args = new mdl.TopTenModule.TopTenModele();
            var builder = srv.EmbedBuilderService.GenerateBuilderReadRegister(new List<string> { "toto" }, args.ColorTopTen);

            var stringifiedBuilder = srv.ExtensionsEmbedBuilder.ToString(builder);
            stringifiedBuilder.Should().Be("TopTen, , #147786, , Liste des joueurs :, toto, ");
        }
    }
}
