using System;
using srv = MainConsole.Services;

namespace MainConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            while (1 != 0)
            {
                var command = Console.ReadLine();
                switch (command)
                {
                    case "Discord":
                        if (!srv.Discord.IsDiscordLaunched)
                        {
                            srv.Discord.IsDiscordLaunched = true;
                            BotANick.Discord.Program.Main(args);
                        }
                        break;

                    case "Twitch":
                        srv.Twitch.LaunchTwitch();
                        break;

                    case "Stream":
                        _ = BotANick.Twitch.Api.StreamInfo.IsStreaming();
                        break;

                    default:
                        Console.WriteLine($"{command} n'est pas une commande");
                        break;
                }
            }
        }
    }
}
