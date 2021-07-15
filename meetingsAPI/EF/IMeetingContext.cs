using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using meetingsAPI.Models;
using System.Threading;
using System.Threading.Tasks;

namespace meetingsAPI.EF
{
    public interface IMeetingContext
    {
        ChangeTracker ChangeTracker { get; }
        DatabaseFacade Database { get; }

        public DbSet<ConferenceRoom> ConferenceRoom { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Meeting> Meeting { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<UserMeeting> UserMeeting { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
