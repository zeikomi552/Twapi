using FollowBackCore.Database.SQLite.Base;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowBackCore.Database.SQLite
{
    public class SQLiteDataContext : DbContext
    {
        public DbSet<TwitterSearchResultsBase> DbSet_TwitterSearchResults { get; internal set; }
        public DbSet<TwitterFollowLogBase> DbSet_TwitterFollowLog { get; internal set; }
        public DbSet<MyFriendsLogBase> DbSet_MyFriendsLog { get; internal set; }
        public DbSet<MyFollowerLogBase> DbSet_MyFollowerLog { get; internal set; }

        public SQLiteDataContext()
        {
        }


        // 最初にココを変更する
        string db_file_path = TwCommand.SQLitePath;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = new SqliteConnectionStringBuilder { DataSource = db_file_path }.ToString();
            optionsBuilder.UseSqlite(new SqliteConnection(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TwitterSearchResultsBase>().HasKey(c => new { c.CreateDt, c.Id });
            modelBuilder.Entity<TwitterFollowLogBase>().HasKey(c => new { c.CreateDt, c.UserId });
            modelBuilder.Entity<MyFriendsLogBase>().HasKey(c => new { c.CreateDt, c.UserId });
            modelBuilder.Entity<MyFollowerLogBase>().HasKey(c => new { c.CreateDt, c.UserId });

        }
    }
}
