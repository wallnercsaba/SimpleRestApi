using Microsoft.EntityFrameworkCore;
using SimpleRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleRestApi
{
    public class SimpleRestApiDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public SimpleRestApiDbContext(DbContextOptions<SimpleRestApiDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>().Property(user => user.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().HasKey(user => user.Id);
            modelBuilder.Entity<User>().Property(user => user.Email).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<User>().Property(user => user.FirstName).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<User>().Property(user => user.LastName).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<User>().Property(user => user.PhoneNumber).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<User>().Property(user => user.PhoneNumberVisibility).HasDefaultValue(false);
            modelBuilder.Entity<User>().Property(user => user.EmailVisibility).HasDefaultValue(false);
            
        }
    }
}
