using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;
using BotANick.Twitch.Services;
using BotANick.Twitch.Commands;
using BotANick.Twitch.Interfaces;

namespace BotANick.Tests.Twitch
{
    public class CommandTests
    {
        [Fact]
        public void ShouldGetTextCommands()
        {
            const string textCommandStr = "help";
            var textCommand = TextCommands.GetTextCommands(textCommandStr);

            textCommand.Should().Be(TextCommands.EnumTextCommand.Help);
        }

        [Fact]
        public void ShouldGetNothingFromTextCommandsIfWrongCommand()
        {
            const int expectedEnumValue = 0;
            const string textCommandStr = "notACommand";
            var textCommand = TextCommands.GetTextCommands(textCommandStr);
            textCommand.Should().Be(expectedEnumValue);
        }

        [Fact]
        public void ShouldGetDiscordPubCommands()
        {
            const string discordCommandStr = "discord pub";
            var discordCommand = DiscordCommands.GetDiscordCommands(discordCommandStr);

            discordCommand.Should().Be(DiscordCommands.EnumDiscordCommand.Pub);
        }

        [Fact]
        public void ShouldDiscordCommandReturnNothing()
        {
            const string discordCommandStr = "discord";
            var writeSrv = new TestWriteService();
            DiscordCommands.Execute(discordCommandStr, writeSrv);

            writeSrv.WrittenChat.Should().BeNull();
        }

        [Fact]
        public void ShouldDiscordPubCommandReturnPub()
        {
            const string discordCommandStr = "discord pub";
            const string expectedMsgToWrite = "Rejoignez le Discord de la communauté : https://discord.gg/PjNqJSY9E6. Des récompenses et droits supplémentaires pour les subs !";
            var writeSrv = new TestWriteService();

            DiscordCommands.Execute(discordCommandStr, writeSrv);

            writeSrv.WrittenChat.Should().Be(expectedMsgToWrite);
        }

        [Fact]
        public void ShouldTransformTextToCommand()
        {
            const string text = "!command";
            var command = CommandService.GetCommand(text);

            command.Should().Be("command");
        }

        [Fact]
        public void ShouldTransformOnlyCommand()
        {
            const string text = "notAcommand";
            var command = CommandService.GetCommand(text);

            command.Should().BeNull();
        }

        [Fact]
        public void ShouldUseInterface()
        {
            var writeSrv = new TestWriteService();
            const string expectedMessageToWrite = "Message";
            writeSrv.WriteInChat(expectedMessageToWrite);
            writeSrv.WrittenChat.Should().Be(expectedMessageToWrite);
        }

        [Fact]
        public void ShouldExecuteFalseCommandDoNothing()
        {
            var writeSrv = new TestWriteService();
            const string command = "notACommand";
            TextCommands.Execute(command, writeSrv);

            writeSrv.WrittenChat.Should().BeNull();
        }

        [Fact]
        public void ShouldHelpCommandWriteSpecificMsg()
        {
            var writeSrv = new TestWriteService();
            const string expectedMessageToWrite = "Liste des commandes : '!Help' '!Hydrate' '!Toto' '!Bonjour' '!Rig' '!Indelivrables' '!Discord Pub'";
            const string command = "help";
            TextCommands.Execute(command, writeSrv);

            writeSrv.WrittenChat.Should().Be(expectedMessageToWrite);
        }

        [Fact]
        public void ShouldTotoCommandWriteSpecificMsg()
        {
            var writeSrv = new TestWriteService();
            const string expectedMessageToWrite = "Votre langage est très évolué.";
            const string command = "toto";
            TextCommands.Execute(command, writeSrv);

            writeSrv.WrittenChat.Should().Be(expectedMessageToWrite);
        }

        [Fact]
        public void ShouldHydrateCommandWriteRandomMsg()
        {
            var writeSrv = new TestWriteService();
            const string command = "hydrate";

            TextCommands.Execute(command, writeSrv);

            writeSrv.WrittenChat.Should().BeOneOf(TextCommands.HydrateResults);
        }

        [Fact]
        public void ShouldBonjourCommandWriteSpecificMsg()
        {
            var writeSrv = new TestWriteService();
            const string expectedMessageToWrite = "HeyGuys";
            const string command = "bonjour";

            TextCommands.Execute(command, writeSrv);

            writeSrv.WrittenChat.Should().Be(expectedMessageToWrite);
        }

        [Fact]
        public void ShouldRigCommandWriteSpecificMsg()
        {
            var writeSrv = new TestWriteService();
            const string expectedMessageToWrite = "Il a 4 écrans et il ne parle que de ça...";
            const string command = "rig";

            TextCommands.Execute(command, writeSrv);

            writeSrv.WrittenChat.Should().Be(expectedMessageToWrite);
        }

        [Fact]
        public void ShouldIndelivrablesCommandWriteSpecificMsg()
        {
            var writeSrv = new TestWriteService();
            const string expectedMessageToWrite = "Allez tous regarder la chaîne YouTube des Indélivrables : https://www.youtube.com/channel/UCl7djHZZcnOt-t05QMYx90g";
            const string command = "indelivrables";

            TextCommands.Execute(command, writeSrv);

            writeSrv.WrittenChat.Should().Be(expectedMessageToWrite);
        }

        private class TestWriteService : IWriteService
        {
            public string WrittenChat { get; set; }

            public void WriteInChat(string msg)
            {
                WrittenChat = msg;
            }
        }
    }
}
