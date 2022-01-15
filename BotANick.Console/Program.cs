Console.WriteLine("Hello, World!");

ManualResetEvent _quitEvent = new ManualResetEvent(false);

if (args.Contains("discord"))
{
    Console.WriteLine("Launching Discord");
    BotANick.Discord.Startup twitch = new();
}

if (args.Contains("twitch"))
{
    Console.WriteLine("Launching Twitch");
    BotANick.Twitch.Startup discord = new();
}

_quitEvent.WaitOne();
