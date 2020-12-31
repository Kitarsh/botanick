using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BotANick.Data
{
    public class DataContext : DbContext
    {
        public DbSet<TopTenTheme> TopTenTheme{ get; set; }

        public DbSet<Idee> Idee { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=botanick_data.db");
    }
}