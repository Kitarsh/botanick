using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace TwitchBot.Modules
{
    static class Pub
    {
        public static void PubDiscord(TwitchClient client)
        {
            var channel = client.GetJoinedChannel("Kitarsh");
            var msg = $"Rejoignez le Discord de la communauté : https://discord.gg/PjNqJSY9E6. Des récompenses et droits supplémentaires pour les subs !";
            client.SendMessage(channel, msg);
        }
    }
}
