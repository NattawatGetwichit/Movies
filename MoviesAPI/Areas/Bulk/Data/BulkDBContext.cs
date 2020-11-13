using Microsoft.EntityFrameworkCore;
using MoviesAPI.Areas.ApiV1.Models;
using System;
using System.Collections.Generic;

namespace MoviesAPI.Data
{
    public class BulkDBContext : DbContext
    {
        public BulkDBContext(DbContextOptions<BulkDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bulk>(entity =>
            {
                entity.Property(e => e.BulkId).ValueGeneratedNever();

                entity.Property(e => e.BulkCode).HasMaxLength(20);

                entity.Property(e => e.BulkName).HasMaxLength(20);
            });

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Bulk> Bulk { get; set; }
    }
}