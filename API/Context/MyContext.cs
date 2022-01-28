using API.Models;
using Microsoft.EntityFrameworkCore;


namespace API.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Profiling> Profilings { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleAccount> RoleAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Account)
                .WithOne(a => a.Employee)
                .HasForeignKey<Account>(a => a.NIK);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Profiling)
                .WithOne(p => p.Account)
                .HasForeignKey<Profiling>(p => p.NIK);

            modelBuilder.Entity<Profiling>()
                .HasOne(p => p.Education)
                .WithMany(ed => ed.Profilings);

            modelBuilder.Entity<Education>()
                .HasOne(ed => ed.University)
                .WithMany(u => u.Educations);

            // many to many
            
            modelBuilder.Entity<RoleAccount>()
                .HasKey(ra => new { ra.Id, ra.NIK });

            modelBuilder.Entity<RoleAccount>()
                .HasOne(ra => ra.Accounts)
                .WithMany(a => a.RoleAccounts)
                .HasForeignKey(ra => ra.NIK);

            modelBuilder.Entity<RoleAccount>()
                .HasOne(ra => ra.Roles)
                .WithMany(r => r.RoleAccounts)
                .HasForeignKey(ra => ra.Id);

        }
    }
}
