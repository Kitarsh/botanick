using BotANick.Twitch.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using BotANick.Twitch.Commands;

namespace BotANick.Twitch.Services
{
    public static class CommandService
    {
        public static void DoAction(OnMessageReceivedArgs e, TwitchClient client)
        {
            var writeSrv = new WriteService(client, e.ChatMessage.Channel);

            string message = e.ChatMessage.Message;
            string command = GetCommand(message);

            if (command == null)
            {
                return;
            }

            DiscordCommands.Execute(command);
            TextCommands.Execute(command, writeSrv);
        }

        public static string GetCommand(string msg)
        {
            if (msg.StartsWith("!"))
            {
                return msg[1..];
            }

            return null;
        }
    }
}
