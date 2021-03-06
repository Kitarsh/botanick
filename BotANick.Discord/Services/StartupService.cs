﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BotANick.Discord.Services
{
    public class StartupService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;

        // DiscordSocketClient, CommandService, and IConfigurationRoot are injected automatically from the IServiceProvider
        public StartupService(
            IServiceProvider provider,
            DiscordSocketClient discord,
            CommandService commands,
            IConfigurationRoot config)
        {
            _provider = provider;
            _config = config;
            _discord = discord;
            _commands = commands;
        }

        public void Start()
        {
            string discordToken = _config["tokens:discord"];     // Get the discord token from the config file
            if (string.IsNullOrWhiteSpace(discordToken))
                throw new ArgumentException("Please enter your bot's token into the `_configuration.json` file found in the applications root directory.");

            TwitchLogs.SetDiscordClient(_discord);
            BoiteAIdeeService.SetDiscordClient(_discord);
            GitHubService.InitConfig(_config);
            _ = StartAsync(discordToken);
        }

        public async Task StartAsync(string discordToken)
        {
            var projectAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.Contains("BotANick"));
            await _discord.LoginAsync(TokenType.Bot, discordToken);     // Login to discord
            await _discord.StartAsync();                                // Connect to the websocket
            await _commands.AddModulesAsync(projectAssembly, _provider);     // Load commands and modules into the command service
            await BoiteAIdeeService.UpdateBoiteIdees();
        }
    }
}
