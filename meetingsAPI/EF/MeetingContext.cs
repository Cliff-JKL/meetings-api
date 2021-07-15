using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using meetingsAPI.Models;

namespace meetingsAPI.EF
{
    public class MeetingContext : DbContext, IMeetingContext
    {
        public MeetingContext(DbContextOptions<MeetingContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ConferenceRoom> ConferenceRoom { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Meeting> Meeting { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<UserMeeting> UserMeeting { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConferenceRoom>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            });

            modelBuilder.Entity<Meeting>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.HostId).HasColumnName("host_id");

                entity.HasOne(d => d.Host)
                .WithMany(p => p.Meetings)
                .IsRequired()
                .HasForeignKey(d => d.HostId);

                entity.Property(e => e.DateStart)
                .HasColumnName("date_start")
                .IsRequired();

                entity.Property(e => e.DateEnd)
                .HasColumnName("date_end")
                .IsRequired();

                entity.Property(e => e.Description)
                .HasColumnName("description");

                entity.Property(e => e.ConferenceRoomId).HasColumnName("conference_room_id");

                entity.HasOne(d => d.ConferenceRoom)
                .WithMany(p => p.Meetings)
                .HasForeignKey(d => d.ConferenceRoomId);

                entity.Property(e => e.ProjectId).HasColumnName("project_id");

                entity.HasOne(d => d.Project)
                .WithMany(p => p.Meetings)
                .HasForeignKey(d => d.ProjectId);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

                entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50);

                entity.Property(e => e.Login)
                .IsRequired()
                .HasMaxLength(50);

                entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(50);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.UserId);

                entity.Property(e => e.RoleId);

                entity.HasOne(d => d.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserMeeting>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.UserId);

                entity.Property(e => e.MeetingId);

                entity.Property(e => e.IsTakePart)
                .IsRequired();

                entity.Property(e => e.Rating);

                entity.Property(e => e.Comment)
                .HasMaxLength(50);

                entity.HasOne(d => d.User)
                .WithMany(p => p.UserMeetings)
                .HasForeignKey(d => d.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Meeting)
                .WithMany(p => p.UserMeetings)
                .HasForeignKey(d => d.MeetingId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Role>().HasData(
                new { Id = 1, Name = "admin" },
                new { Id = 2, Name = "user" }
            );

            modelBuilder.Entity<ConferenceRoom>().HasData(
                new { Id = 1, Name = "Seoul" },
                new { Id = 2, Name = "Montreal" },
                new { Id = 3, Name = "San Jose" },
                new { Id = 4, Name = "Florence" },
                new { Id = 5, Name = "Sydney" }
            );

            modelBuilder.Entity<User>().HasData(
                new {
                    Id = 1,
                    Name = "Admin",
                    Email = "admin@email.com",
                    Login = "admin",
                    Password = "admin"
                },
                new
                {
                    Id = 2,
                    Name = "Default User",
                    Email = "user@email.com",
                    Login = "user",
                    Password = "password"
                }
            );

            modelBuilder.Entity<UserRole>().HasData(
                new { Id = 1, UserId = 1, RoleId = 1 },
                new { Id = 2, UserId = 2, RoleId = 2 }
            );
        }
    }
}
