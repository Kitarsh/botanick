using System;
using System.Linq;
using System.Threading;
using srv = MainConsole.Services;

namespace MainConsole
{
    static public class Program
    {
        static ManualResetEvent _quitEvent = new ManualResetEvent(false);

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            if (args.Contains("discord"))
            {
                BotANick.Discord.Program.Main(args);
            }

            if (args.Contains("twitch"))
            {
                srv.Twitch.LaunchTwitch();
            }

            _quitEvent.WaitOne();
        }
    }
}
