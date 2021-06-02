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
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Contributor> Contributors { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Apartment>()
                .HasMany(x => x.Contributors)
                .WithOne(x => x.Apartment)
                .OnDelete(DeleteBehavior.Cascade);
                    

            builder.Entity<Apartment>()
                .HasOne(p => p.Owner);


            builder.Entity<Apartment>()
                .HasMany(x => x.Jobs)
                .WithOne(y => y.Apartment)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
