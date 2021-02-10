using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using BotANick.Discord.Services;

namespace BotANick.Twitch.Services
{
    public static class Discord
    {
        public static void Log(Object sender, OnMessageReceivedArgs e)
        {
            var displayName = e.ChatMessage.DisplayName;
            var msg = e.ChatMessage.Message;
            _ = TwitchLogs.LogTwitchChat(displayName, msg);
        }

        public static void StreamStartAlert(TwitchClient client)
        {
            // TODO : Check with Twitch API the name of the stream and send it to Discord to display it.
            //var channel = client.GetJoinedChannel("kitarsh");
            _ = TwitchLogs.LogStreamStart();
        }
    }
}
