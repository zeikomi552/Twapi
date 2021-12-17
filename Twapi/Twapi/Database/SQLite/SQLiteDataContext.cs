using Twapi.Database.SQLite.Base;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twapi.Database.SQLite
{
    public class SQLiteDataContext : DbContext
    {
        public DbSet<FollowListBase> DbSet_FollowList { get; internal set; }


        // 最初にココを変更する
        string db_file_path = @"Twapi.db";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = new SqliteConnectionStringBuilder { DataSource = db_file_path }.ToString();
            optionsBuilder.UseSqlite(new SqliteConnection(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FollowListBase>().HasKey(c => new { c.UserId });

        }
    }
}
