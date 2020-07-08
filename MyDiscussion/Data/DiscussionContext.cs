using Microsoft.EntityFrameworkCore;
using MyDiscussion.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MyDiscussion.Data
{
    public class DiscussionContext : DbContext
    {
        #region DbSets

        public DbSet<User> Users { get; private set; }
        public DbSet<Article> Articles { get; private set; }

        #endregion


        public DiscussionContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
