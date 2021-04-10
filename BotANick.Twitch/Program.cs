using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace BotANick.Twitch
{
    public static class Program
    {
        private static readonly string _directoryPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase), "..\\..\\..\\..\\");

        public static IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args)
        {
            // Getting the URI of the project.
            var configProdYml = Path.Combine(_directoryPath, "BotANick.Twitch\\config-prod.yml");
            string configProdStr = new Uri(configProdYml).LocalPath;

            var builder = new ConfigurationBuilder()                    // Create a new instance of the config builder
                              .SetBasePath(AppContext.BaseDirectory)    // Specify the default location for the config file
                              .AddYamlFile(configProdStr);              // Add this (yaml encoded) file to the configuration
            Configuration = builder.Build();                            // Build the configuration
            _ = new Bot();
        }
    }
}
