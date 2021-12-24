Console.WriteLine("Hello, World!");

ManualResetEvent _quitEvent = new ManualResetEvent(false);

if (args.Contains("discord"))
{
    Startup startup = new();
    Console.WriteLine("Launching Discord");
}

if (args.Contains("twitch"))
{
    // Call BotANick.Twitch
    Console.WriteLine("Launching Twitch");
}

_quitEvent.WaitOne();
