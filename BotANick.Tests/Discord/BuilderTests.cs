using System;
using System.Collections.Generic;
using System.Linq;
using BotANick.Core.Data;
using Discord;
using FluentAssertions;
using Xunit;
using mod = BotANick.Discord.Modeles;
using srv = BotANick.Discord.Services;

namespace BotANick.Tests.Discord
{
    public class BuilderTests
    {
        [Fact]
        public void ShouldReturnTopTenBuilder()
        {
            var args = new mod.TopTenGame();

            var builder = srv.EmbedBuilderService.InitBuilder(new List<EmbedFieldBuilder>(), args.ColorTopTen);

            var stringifiedBuilder = srv.ExtensionsEmbedBuilder.ToString(builder);
            stringifiedBuilder.Should().Be("TopTen, , #147786, , , ");
        }

        [Fact]
        public void ShouldReturnTopTenThemeBuilder()
        {
            var args = new mod.TopTenGame();
            var builder = srv.EmbedBuilderService.GenerateBuilderForNumberDisplay(args, "toto");

            var stringifiedBuilder = srv.ExtensionsEmbedBuilder.ToString(builder);
            stringifiedBuilder.Should().Be("TopTen, , #147786, , Le thème est le suivant :, toto, Le Cap'TEN est :, No Capten was selected, Tirage des numéros :, No players, ");
        }

        [Fact]
        public void ShouldReturnTopTenPlayersBuilder()
        {
            var args = new mod.TopTenGame();
            var builder = srv.EmbedBuilderService.GenerateBuilderReadRegister(new List<string> { "toto" }, args.ColorTopTen);

            var stringifiedBuilder = srv.ExtensionsEmbedBuilder.ToString(builder);
            stringifiedBuilder.Should().Be("TopTen, , #147786, , Liste des joueurs :, toto, ");
        }
    }
}
