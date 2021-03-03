using System;
using System.Collections.Generic;
using System.Linq;
using BotANick.Core.Data;
using FluentAssertions;
using Xunit;
using srv = BotANick.Discord.Services;

namespace BotANick.Tests.Discord
{
    public class TopTenTests
    {
        [Fact]
        public void ShouldTopTenRandomListContainsAllNumbers()
        {
            var numbers = srv.TopTenService.GetRandomList();
            1.Should().BeOneOf(numbers);
            2.Should().BeOneOf(numbers);
            3.Should().BeOneOf(numbers);
            4.Should().BeOneOf(numbers);
            5.Should().BeOneOf(numbers);
            6.Should().BeOneOf(numbers);
            7.Should().BeOneOf(numbers);
            8.Should().BeOneOf(numbers);
            9.Should().BeOneOf(numbers);
            10.Should().BeOneOf(numbers);
        }

        [Fact]
        public void ShouldTopTenGenerateNotOrderedLists()
        {
            var numbers1 = srv.TopTenService.GetRandomList();
            numbers1.Should().NotBeInAscendingOrder();
        }

        [Fact]
        public void ShouldTopTenReturnTheme()
        {
            var themes = srv.TopTenService.GetRandomThemes();

            themes.Should().BeOfType(typeof(List<string>));
        }

        [Fact]
        public void ShouldGenerateEmptyForNoPlayers()
        {
            var emptyPlayerList = new List<string>() { };
            var generatedNumbers = srv.TopTenService.GenerateNumbers(emptyPlayerList);

            generatedNumbers.Should().Be(string.Empty);
        }

        [Fact]
        public void ShouldGenerateNumberForOnePlayer()
        {
            var playerList = new List<string>() { "toto" };
            var generatedNumbers = srv.TopTenService.GenerateNumbers(playerList);

            var possibleResultsList = Enumerable.Range(1, 10)
                                                .Select(n => $"\r\ntoto ||`{n:00}`||")
                                                .ToList();

            generatedNumbers.Should().BeOneOf(possibleResultsList);
        }
    }
}
