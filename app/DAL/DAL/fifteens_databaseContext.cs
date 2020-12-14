// <copyright file="fifteens_databaseContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#nullable disable

namespace DAL
{
    using System;
    using System.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;

    /// <summary>
    /// Class for a database object.
    /// </summary>
    public partial class Fifteens_databaseContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Fifteens_databaseContext"/> class.
        /// </summary>
        public Fifteens_databaseContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fifteens_databaseContext"/> class.
        /// </summary>
        /// <param name="options"> Options. </param>
        public Fifteens_databaseContext(DbContextOptions<Fifteens_databaseContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets property of Matches table.
        /// </summary>
        public virtual DbSet<Match> Matches { get; set; }

        /// <summary>
        /// Gets or sets property of Users table.
        /// </summary>
        public virtual DbSet<User> Users { get; set; }

        /// <summary>
        /// Configures the database.
        /// </summary>
        /// <param name="optionsBuilder">The type of options being requested.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString);
            }
        }

        /// <summary>
        /// Defines the model for the context being created.
        /// </summary>
        /// <param name="modelBuilder"> Instance of the ModelBuilder. </param>
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

                entity.Property(e => e.Size).HasColumnName("size");

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

            this.OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}