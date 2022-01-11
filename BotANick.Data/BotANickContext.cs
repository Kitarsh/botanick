using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BotANick.Data.Models;

namespace BotANick.Data
{
    public class BotANickContext : DbContext
    {
        public DbSet<TopTenTheme> TopTenThemes { get; set; }

        public DbSet<Idee> Idees { get; set; }

        public DbSet<TwitchCommand> TwitchCommands { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(AppContext.BaseDirectory)
                        .AddYamlFile("data-config-prod.yml");
            var config = builder.Build();
            optionsBuilder.UseMySQL($"server={config["botanickcontext:server"]};database=botanick;user={config["botanickcontext:user"]};password={config["botanickcontext:password"]}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
