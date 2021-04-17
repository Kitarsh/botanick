using System;
using System.Collections.Generic;
using System.Linq;
using BotANick.Core.Data;
using FluentAssertions;
using Xunit;
using srv = BotANick.Discord.Services;
using mod = BotANick.Discord.Modeles;
using System.Reflection;

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

        [Fact]
        public void ShouldReturnTopTenGameProperties()
        {
            // Given
            var topTenGame = new mod.TopTenGame();

            const string expectedColor = "#147786";
            int[] expectationNumberList = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, };
            const string expectedEmote = "💯";
            int? expectedIndexCapten = null;
            const ulong expectedIdMsg = 0;

            // Then
            topTenGame.NumberList.Should().BeEquivalentTo(expectationNumberList);
            topTenGame.ColorTopTen.ToString().Should().Be(expectedColor);
            topTenGame.RegisterEmoteChar.ToString().Should().Be(expectedEmote);
            topTenGame.IndexCapten.Should().Be(expectedIndexCapten);
            topTenGame.RegisterMsgId.Should().Be(expectedIdMsg);
            topTenGame.Users.Should().BeEmpty();
            topTenGame.Themes.Should().BeEmpty();
        }

        [Fact]
        public void ShouldRegisterTopTenUser()
        {
            //Given
            var topTenGame = new mod.TopTenGame();

            const string player = "Player 1";
            List<string> playersList = new List<string> { player };

            //With
            topTenGame.RegisterUser(player);

            //Then
            topTenGame.Users.Should().BeEquivalentTo(playersList);
        }

        [Fact]
        public void ShouldClearRegisteredTopTenUser()
        {
            //Given
            var topTenGame = new mod.TopTenGame();

            const string player = "Player 1";
            List<string> playersList = new List<string> { player };
            topTenGame.RegisterUser(player);
            topTenGame.Users.Should().BeEquivalentTo(playersList);

            List<string> playersListExpected = new List<string> { };

            //With
            topTenGame.ClearUser();

            //Then
            topTenGame.Users.Should().BeEquivalentTo(playersListExpected);
        }

        [Fact]
        public void ShouldRegisterTopTenTheme()
        {
            //Given
            var topTenGame = new mod.TopTenGame();

            const string theme = "Thème 1";
            List<string> themesList = new List<string> { theme };

            //With
            topTenGame.RegisterTheme(theme);

            //Then
            topTenGame.Themes.Should().BeEquivalentTo(themesList);
        }

        [Fact]
        public void ShouldClearRegisteredTopTenTheme()
        {
            //Given
            var topTenGame = new mod.TopTenGame();

            const string theme = "Thème 1";
            List<string> themesList = new List<string> { theme };
            topTenGame.RegisterTheme(theme);
            topTenGame.Themes.Should().BeEquivalentTo(themesList);

            List<string> themesListExpected = new List<string> { };

            //With
            topTenGame.ClearTheme();

            //Then
            topTenGame.Themes.Should().BeEquivalentTo(themesListExpected);
        }

        [Fact]
        public void ShouldStoreTopTenRegisterMessage()
        {
            //Given
            var topTenGame = new mod.TopTenGame();
            const ulong idMsg = 25;

            //With
            topTenGame.StoreRegisterMsg(idMsg);

            //Then
            topTenGame.RegisterMsgId.Should().Be(idMsg);
        }

        [Fact]
        public void ShouldResetTopTenRegisterMessage()
        {
            //Given
            var topTenGame = new mod.TopTenGame();
            const ulong idMsg = 25;
            topTenGame.StoreRegisterMsg(idMsg);

            const ulong expectedIdMsg = 0;

            //With
            topTenGame.ResetRegisterMsg();

            //Then
            topTenGame.RegisterMsgId.Should().Be(expectedIdMsg);
        }

        [Fact]
        public void ShouldResetTopTenGameParams()
        {
            //Given
            var topTenGame = new mod.TopTenGame();
            const ulong idMsg = 25;
            topTenGame.StoreRegisterMsg(idMsg);
            topTenGame.RegisterMsgId.Should().Be(idMsg);

            const string theme = "Thème 1";
            List<string> themesList = new List<string> { theme };
            topTenGame.RegisterTheme(theme);
            topTenGame.Themes.Should().BeEquivalentTo(themesList);

            const string player = "Player 1";
            List<string> playersList = new List<string> { player };
            topTenGame.RegisterUser(player);
            topTenGame.Users.Should().BeEquivalentTo(playersList);

            const ulong expectedIdMsg = 0;
            List<string> themesListExpected = new List<string> { };
            List<string> playersListExpected = new List<string> { };

            const bool expectedCaptenPresence = false;
            topTenGame.NextCapten();

            //With
            topTenGame.Clear();

            //Then
            topTenGame.RegisterMsgId.Should().Be(expectedIdMsg);
            topTenGame.Themes.Should().BeEquivalentTo(themesListExpected);
            topTenGame.Users.Should().BeEquivalentTo(playersListExpected);
            topTenGame.HasCapten.Should().Be(expectedCaptenPresence);
        }

        [Fact]
        public void ShouldCount0TopTenUsers()
        {
            var topTenGame = new mod.TopTenGame();
            const int expectedNbOfUsers = 0;

            topTenGame.NbsUsers.Should().Be(expectedNbOfUsers);
        }

        [Fact]
        public void ShouldCount1TopTenUsers()
        {
            var topTenGame = new mod.TopTenGame();
            const string player = "Player 1";
            topTenGame.RegisterUser(player);
            const int expectedNbOfUsers = 1;

            topTenGame.NbsUsers.Should().Be(expectedNbOfUsers);
        }

        [Fact]
        public void ShouldCount5TopTenUsers()
        {
            var topTenGame = new mod.TopTenGame();
            Add5PlayersToGame(topTenGame);
            const int expectedNbOfUsers = 5;

            topTenGame.NbsUsers.Should().Be(expectedNbOfUsers);
        }

        [Fact]
        public void ShouldCaptenBeFirstTopTenPlayer()
        {
            var topTenGame = new mod.TopTenGame();
            Add5PlayersToGame(topTenGame);
            const int expectedIndexCapten = 0;

            topTenGame.NextCapten();

            topTenGame.IndexCapten.Should().Be(expectedIndexCapten);
        }

        [Fact]
        public void ShouldCaptenBeSecondTopTenPlayerAfter2Distributions()
        {
            var topTenGame = new mod.TopTenGame();
            Add5PlayersToGame(topTenGame);
            const int expectedIndexCapten = 1;

            topTenGame.NextCapten();
            topTenGame.NextCapten();

            topTenGame.IndexCapten.Should().Be(expectedIndexCapten);
        }

        [Fact]
        public void ShouldCaptenDistributionBeACircularPermutation()
        {
            var topTenGame = new mod.TopTenGame();
            Add5PlayersToGame(topTenGame);
            const int expectedIndexCapten = 0;

            topTenGame.NextCapten();
            topTenGame.NextCapten();
            topTenGame.NextCapten();
            topTenGame.NextCapten();
            topTenGame.NextCapten();
            topTenGame.NextCapten();

            topTenGame.IndexCapten.Should().Be(expectedIndexCapten);
        }

        [Fact]
        public void ShouldNotHaveCaptenWithoutDistribution()
        {
            var topTenGame = new mod.TopTenGame();
            Add5PlayersToGame(topTenGame);
            const bool expectedCaptenPresence = false;

            topTenGame.HasCapten.Should().Be(expectedCaptenPresence);
        }

        [Fact]
        public void ShouldHaveCaptenAfter1Distribution()
        {
            var topTenGame = new mod.TopTenGame();
            Add5PlayersToGame(topTenGame);
            const bool expectedCaptenPresence = true;

            topTenGame.NextCapten();

            topTenGame.HasCapten.Should().Be(expectedCaptenPresence);
        }

        [Fact]
        public void ShouldHaveCorrectCaptenAfter1Distribution()
        {
            var topTenGame = new mod.TopTenGame();
            const string expectedCapten = "The Capten";
            topTenGame.RegisterUser(expectedCapten);

            topTenGame.NextCapten();

            topTenGame.Capten.Should().Be(expectedCapten);
        }

        [Fact]
        public void ShouldNotHaveCaptenAfter1DistributionAndAClear()
        {
            var topTenGame = new mod.TopTenGame();
            Add5PlayersToGame(topTenGame);
            const bool expectedCaptenPresence = false;

            topTenGame.NextCapten();
            topTenGame.ClearCapten();

            topTenGame.HasCapten.Should().Be(expectedCaptenPresence);
        }

        [Fact]
        public void ShouldAddArrayOfNewPlayers()
        {
            var topTenGame = new mod.TopTenGame();
            Add5PlayersToGame(topTenGame);
            const string playerToAdd = "New Player";
            var playersToAdd = GetArrayOfPlayers().ToList();

            playersToAdd.Add(playerToAdd);

            List<string> expectedListOfPlayers = playersToAdd.ToList();

            topTenGame.RegisterUser(playersToAdd.ToArray());

            topTenGame.Users.Should().BeEquivalentTo(expectedListOfPlayers);
        }

        [Fact]
        public void ShouldAddArrayOfNewThemes()
        {
            var topTenGame = new mod.TopTenGame();
            var themesToAdd = GetArrayOfThemes().ToList();

            List<string> expectedListOfThemes = themesToAdd.ToList();

            topTenGame.RegisterTheme(themesToAdd.ToArray());

            topTenGame.Themes.Should().BeEquivalentTo(expectedListOfThemes);
        }

        [Fact]
        public void ShouldNotAddExistingPlayersAmongUsers()
        {
            var topTenGame = new mod.TopTenGame();
            const string playerToAddTwice = "New Player";

            List<string> expectedListOfPlayers = new List<string>
            {
                playerToAddTwice
            };

            topTenGame.RegisterUser(playerToAddTwice);
            topTenGame.RegisterUser(playerToAddTwice);

            topTenGame.Users.Should().BeEquivalentTo(expectedListOfPlayers);
        }

        [Fact]
        public void ShouldPopFirstTheme()
        {
            var topTenGame = new mod.TopTenGame();
            const string expectedTheme = "New theme";
            List<string> expectedThemesList = new List<string>();

            topTenGame.RegisterTheme(expectedTheme);

            string theme = topTenGame.GetNextTheme();

            theme.Should().Be(expectedTheme);
            topTenGame.Themes.Should().BeEquivalentTo(expectedThemesList);
        }

        [Fact]
        public void ShouldCreateThemeFromDataBase()
        {
            using (var context = new DataInMemoryContext(MethodBase.GetCurrentMethod().Name))
            {
                var expectedTheme = new TopTenTheme
                {
                    Theme = "New Theme",
                };

                context.TopTenTheme.Add(expectedTheme);
                context.SaveChanges();

                var themeFromDataBase = context.TopTenTheme.FirstOrDefault();

                themeFromDataBase.Should().Be(expectedTheme);
            }
        }

        [Fact]
        public void ShouldGetThemeFromDataBase()
        {
            using (var context = new DataInMemoryContext(MethodBase.GetCurrentMethod().Name))
            {
                context.TopTenTheme.RemoveRange(context.TopTenTheme);

                var arrayOfThemes = GetArrayOfThemes();
                var arrayOfTopTenTheme = arrayOfThemes.Select(t => new TopTenTheme { Theme = t })
                                                      .ToArray();
                context.TopTenTheme.AddRange(arrayOfTopTenTheme);
                context.SaveChanges();

                var themes = BotANick.Discord.Services.TopTenService.GetRandomThemes(context);

                arrayOfThemes.Should().BeEquivalentTo(themes);

                foreach (string theme in arrayOfThemes)
                {
                    theme.Should().BeOneOf(themes);
                }
            }
        }

        private static void Add5PlayersToGame(mod.TopTenGame topTenGame)
        {
            var players = GetArrayOfPlayers();
            foreach (string player in players)
            {
                topTenGame.RegisterUser(player);
            }
        }

        private static string[] GetArrayOfPlayers()
        {
            return new string[]
            {
                "Player 1",
                "Player 2",
                "Player 3",
                "Player 4",
                "Player 5",
            };
        }

        private static string[] GetArrayOfThemes()
        {
            return new string[]
            {
                "Theme 1",
                "Theme 2",
                "Theme 3",
                "Theme 4",
                "Theme 5",
            };
        }
    }
}
