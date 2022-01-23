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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Employee)
                .WithOne(e => e.Account)
                .HasForeignKey<Account>(a => a.NIK);

            modelBuilder.Entity<Profiling>()
                .HasOne(p => p.Account)
                .WithOne(a => a.Profiling)
                .HasForeignKey<Profiling>(p => p.NIK);

            modelBuilder.Entity<Profiling>()
                .HasOne(p => p.Education)
                .WithMany(ed => ed.Profilings)
                .HasForeignKey(p => p.Education_Id) 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Education>()
                .HasOne(ed => ed.University)
                .WithMany(u => u.Educations)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
