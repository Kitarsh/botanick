﻿using System;

namespace MainConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            bool discordLaunched = false;
            bool twitchLauched = false;
            while (1 != 0)
            {
                var command = Console.ReadLine();
                switch (command)
                {
                    case "Discord":
                        if (!discordLaunched)
                        {
                            discordLaunched = true;
                            BotANick.Program.Main(args);
                        }
                        break;

                    case "Twitch":
                        if (!twitchLauched)
                        {
                            twitchLauched = true;
                            TwitchBot.Program.Main(args);
                        }
                        break;

                    default:
                        Console.WriteLine($"{command} n'est pas une commande");
                        break;
                }
            }
        }
    }
}
