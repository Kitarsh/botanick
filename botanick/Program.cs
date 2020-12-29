using System;
using System.Threading.Tasks;
using BotANick.Modules;
using Discord;
using Discord.WebSocket;


namespace BotANick
{
	public class Program
	{
		public static void Main(string[] args)
			=> Startup.RunAsync(args).GetAwaiter().GetResult();
	}
}
