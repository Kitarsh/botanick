using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BotANick.Data
{
    public class DataContext : DbContext
    {
        public DbSet<TopTenTheme> TopTenTheme{ get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=BotANick_data.db");
    }

}