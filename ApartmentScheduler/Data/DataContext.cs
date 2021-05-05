using ApartmentScheduler.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentScheduler.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>()
                .HasMany(p => p.Apartments);
            builder.Entity<Apartment>()
                .HasOne(p => p.Owner);
            builder.Entity<Apartment>()
                .HasMany(p => p.Users);

        }

    }
}
