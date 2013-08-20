using StoneFinch.SmpMaintenance.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace StoneFinch.SmpMaintenance.Data
{
    public class SmpContext : DbContext
    {
        public SmpContext(string connectionString)
            : base(connectionString)
        {
            // Do not attempt to create database
            Database.SetInitializer<SmpContext>(null);
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Membership> Memberships { get; set; }

        public DbSet<OAuthMembership> OAuthMemberships { get; set; }

        public DbSet<Role> Roles { get; set; }

        /////// <summary>
        /////// aka: Users in Roles
        /////// this DbSet<> can not be defined here because the Properties exists in both UserProfile and Role 
        /////// and the Map exists in the OnModelCreating() method
        /////// </summary>
        ////public DbSet<UserProfileRole> UserProfileRoles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // table names are not plural in DB, remove the convention
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // set up UserProfile Role Many-to-Many relationship
            modelBuilder.Entity<UserProfile>()
                .HasMany(x => x.Roles)
                .WithMany(x => x.UserProfiles)
                .Map(m =>
                    {
                        m.MapLeftKey("UserId");
                        m.MapRightKey("RoleId");
                        m.ToTable("webpages_UsersInRoles");
                    });

            base.OnModelCreating(modelBuilder);
        }
    }
}