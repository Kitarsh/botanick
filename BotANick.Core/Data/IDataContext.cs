using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public DataInMemoryContext(string dbName)
                    : base(GetOptions(dbName))
        { }

        public DbSet<TopTenTheme> TopTenTheme { get; set; }

        public DbSet<Idee> Idee { get; set; }

        private static DbContextOptions GetOptions(string dbName)
                        => new DbContextOptionsBuilder<DataInMemoryContext>().UseInMemoryDatabase(databaseName: dbName).Options;
    }

    public class SqlLiteContext : DbContext, IDataContext
    {
        public DbSet<TopTenTheme> TopTenTheme { get; set; }

        public DbSet<Idee> Idee { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=BotANick.Core/botanick_data.db");
        }
    }
}
