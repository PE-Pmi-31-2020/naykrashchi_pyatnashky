// <copyright file="fifteens_databaseContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#pragma warning disable
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DAL
{
    public partial class fifteens_databaseContext : DbContext
    {
        public fifteens_databaseContext()
        {
        }

        public fifteens_databaseContext(DbContextOptions<fifteens_databaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=fifteens_database;Username=postgres;Password=555");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>(entity =>
            {
                entity.ToTable("match");

                entity.Property(e => e.MatchId).HasColumnName("match_id");

                entity.Property(e => e.CustomPicture)
                    .HasMaxLength(100)
                    .HasColumnName("custom_picture");

                entity.Property(e => e.DateTime).HasColumnName("date_time");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.Layout).HasColumnName("layout");

                entity.Property(e => e.Result).HasColumnName("result");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.Turns).HasColumnName("turns");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Matches)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("match_user_id_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.Login, "user_login_key")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("login");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("password");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
