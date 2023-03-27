using Microsoft.EntityFrameworkCore;
using WalksAPI.Models.Domain;

namespace WalksAPI.Data
{
    public class WalksDbContext:DbContext
    {
        public WalksDbContext(DbContextOptions<WalksDbContext> options):base(options)
        {
            //use dbcontext options and pass to base class
        }
        //create Onmodel create override
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User_Role>()
                .HasOne(x => x.Role)
                .WithMany(y => y.UserRoles)
                .HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<User_Role>()
                .HasOne(x => x.User)
                .WithMany(y => y.UserRoles)
                .HasForeignKey(x => x.UserId);
        }
        //create three tables in databse
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulties { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User_Role> UserRoles { get; set; }
        //start migration for these newly added tables user,roles,userroles

        //nuget-Pacakge manager console
        //add-migration "adding users"
        //update-database
    }
}
