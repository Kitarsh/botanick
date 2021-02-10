using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BotANick.Core.Data
{
    public class DataContext : DbContext
    {
        public DbSet<TopTenTheme> TopTenTheme{ get; set; }

        public DbSet<Idee> Idee { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=E:\\VisualStudioProjects\\DiscordBot\\BotANick.Core\\botanick_data.db");
    }
}