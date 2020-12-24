using System;
using System.Threading.Tasks;
using botanick.Modules;
using Discord;
using Discord.WebSocket;


namespace botanick
{
	public class Program
	{
		public static void Main(string[] args)
			=> Startup.RunAsync(args).GetAwaiter().GetResult();
	}
}
