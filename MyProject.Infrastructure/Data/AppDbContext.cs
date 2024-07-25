using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyProject.Core.Entities;

namespace MyProject.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePage> RolePages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations to derived entities
            ApplyBaseEntityConfiguration<User>(modelBuilder);
            ApplyBaseEntityConfiguration<Role>(modelBuilder);
            ApplyBaseEntityConfiguration<Page>(modelBuilder);
            ApplyBaseEntityConfiguration<UserRole>(modelBuilder);
            ApplyBaseEntityConfiguration<RolePage>(modelBuilder);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
            
            modelBuilder.Entity<RolePage>()
                .HasOne(ur => ur.Page)
                .WithMany(r => r.RolePages)
                .HasForeignKey(ur => ur.PageId);
            
        }
        private void ApplyBaseEntityConfiguration<T>(ModelBuilder modelBuilder) where T : BaseEntity
        {
            modelBuilder.Entity<T>()
                .Property(b => b.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<T>()
                .Property(b => b.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<T>()
                .Property(b => b.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);
        }

    }
}