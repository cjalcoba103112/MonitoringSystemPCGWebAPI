
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Models;

namespace ApplicationContexts
{
    public class ApplicationContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Server=LAPTOP-G6M745HP\\SQLEXPRESS;Database=RTCA_E_MonitoringSystem;Trusted_Connection=True;Encrypt=false";
            string connectionString1 = "Server=LAPTOP-NPIJTS0A\\SQLEXPRESS;Database=RTCA_E_MonitoringSystem;Trusted_Connection=True;Encrypt=false";

            //string connectionString = "Server=db47734.public.databaseasp.net; Database=db47734; User Id=db47734; Password=Mq5#+Ee6p7J-; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;";
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        //public DbSet<ActivityFile> ActivityFile { get; set; }
        public DbSet<EmailEteCommunication> EmailEteCommunication { get; set; }



        public DbSet<ActivityType> ActivityType { get; set; }

        public DbSet<Department> Department { get; set; }

        public DbSet<EnlistmentRecord> EnlistmentRecord { get; set; }

        public DbSet<LeaveTypes> LeaveTypes { get; set; }

        public DbSet<OtpVerifications> OtpVerifications { get; set; }

        public DbSet<Personnel> Personnel { get; set; }

        public DbSet<PersonnelActivity> PersonnelActivity { get; set; }

        public DbSet<PersonnelPromotion> PersonnelPromotion { get; set; }

        public DbSet<Rank> Rank { get; set; }

        public DbSet<RankCategory> RankCategory { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet<Sidebar> Sidebar { get; set; }

        public DbSet<SidebarRoleMapping> SidebarRoleMapping { get; set; }

        public DbSet<Usertbl> Usertbl { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rank>()
                .HasIndex(a => a.RankLevel)
                .IsUnique();
        }

    }
}
