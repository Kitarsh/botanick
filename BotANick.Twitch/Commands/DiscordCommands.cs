using System;
using System.Collections.Generic;
using System.Text;
using BotANick.Twitch.Interfaces;
using BotANick.Twitch.Modules;

namespace BotANick.Twitch.Commands
{
    public static class DiscordCommands
    {
        public enum EnumDiscordCommand
        {
            Pub = 1,
        };

        public static void Execute(string command, IWriteService writeSrv)
        {
            switch (GetDiscordCommands(command))
            {
                case EnumDiscordCommand.Pub: Pub(writeSrv); break;
                default: break;
            }
        }

        public static EnumDiscordCommand GetDiscordCommands(string command)
        {
            if (!command.StartsWith("discord") || command.Length < 8)
            {
                return 0;
            }
            command = command[8..];

            var enumArray = Enum.GetNames(typeof(EnumDiscordCommand));
            foreach (var enumElement in enumArray)
            {
                if (command.ToLower().StartsWith(enumElement.ToLower()))
                {
                    return (EnumDiscordCommand)Enum.Parse(typeof(EnumDiscordCommand), enumElement);
                }
            }

            return 0;
        }

        private static void Pub(IWriteService writeSrv)
        {
            string msg = Modules.Pub.MsgPubDiscord;
            writeSrv.WriteInChat(msg);
        }
    }
}
