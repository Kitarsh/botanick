using System;
using Microsoft.Extensions.Configuration;

namespace TwitchBot
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()                                                        // Create a new instance of the config builder
                              .SetBasePath(AppContext.BaseDirectory)                                        // Specify the default location for the config file
                              .AddYamlFile("E:\\VisualStudioProjects\\DiscordBot\\TwitchBot\\config.yml"); // Add this (yaml encoded) file to the configuration
            Configuration = builder.Build();                                                                // Build the configuration
            Bot bot = new Bot();
        }
    }
}
