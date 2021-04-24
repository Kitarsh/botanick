using BotANick.Twitch.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace BotANick.Twitch.Modules
{
    public static class Pub
    {
        public static string MsgPubDiscord => "Rejoignez le Discord de la communauté : https://discord.gg/PjNqJSY9E6. Des récompenses et droits supplémentaires pour les subs !";

        public static void PubDiscord()
        {
            var client = Bot.Client;
            var channel = client.GetJoinedChannel("Kitarsh");
            client.SendMessage(channel, MsgPubDiscord);
        }

        public static void PubStreamStart(string titleStream)
        {
            var msg = $"{titleStream} Rejoignez le stream sur http://www.twitch.tv/kitarsh !";
            _ = Discord.Services.TwitchLogs.LogStreamStart(msg);
        }
    }
}
