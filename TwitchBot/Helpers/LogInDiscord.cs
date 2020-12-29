using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using BotANick.Services;

namespace TwitchBot.Helpers
{
    public static class LogInDiscord
    {
        public static void Log(Object sender, OnMessageReceivedArgs e)
        {
            var displayName = e.ChatMessage.DisplayName;
            var msg = e.ChatMessage.Message;
            _ = TwitchLogs.LogTwitchChat(displayName, msg);
        }
    }
}
