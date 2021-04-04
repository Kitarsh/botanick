using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BotANick.Core.Data
{
    public interface IDataContext : IDisposable
    {
        public DbSet<TopTenTheme> TopTenTheme { get; set; }

        public DbSet<Idee> Idee { get; set; }
    }

    public class DataInMemoryContext : DbContext, IDataContext
    {
        private static DbContextOptions _options = new DbContextOptionsBuilder<DataInMemoryContext>()
                                               .UseInMemoryDatabase(databaseName: "Test")
                                               .Options;

        public DataInMemoryContext()
                    : base(_options)
        { }

        public DbSet<TopTenTheme> TopTenTheme { get; set; }

        public DbSet<Idee> Idee { get; set; }
    }

    public class SqlLiteContext : DbContext, IDataContext
    {
        public DbSet<TopTenTheme> TopTenTheme { get; set; }

        public DbSet<Idee> Idee { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=E:\\VisualStudioProjects\\DiscordBot\\BotANick.Core\\botanick_data.db");
    }
}
