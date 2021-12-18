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
        public DbSet<FrinedsLogBase> DbSet_FrinedsLog { get; internal set; }
        public DbSet<FollowersLogBase> DbSet_FollowersLog { get; internal set; }


        // 最初にココを変更する
        public static string db_file_path { get; set; } = @"twapi.db";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = new SqliteConnectionStringBuilder { DataSource = db_file_path }.ToString();
            optionsBuilder.UseSqlite(new SqliteConnection(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FollowListBase>().HasKey(c => new { c.UserId });
            modelBuilder.Entity<FrinedsLogBase>().HasKey(c => new { c.UserId });
            modelBuilder.Entity<FollowersLogBase>().HasKey(c => new { c.UserId });

        }
    }
}
