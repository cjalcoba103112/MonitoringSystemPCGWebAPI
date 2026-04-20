
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Models;
using MonitoringSystemPCGWebAPI.Project.Models;

namespace ApplicationContexts
{
    public class ApplicationContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)   
        {
            string connectionString2 = "Server=LAPTOP-G6M745HP\\SQLEXPRESS;Database=RTCA_E_MonitoringSystem;Trusted_Connection=True;Encrypt=false";
            string connectionString = "Server=LAPTOP-NPIJTS0A\\SQLEXPRESS;Database=RTCA_E_MonitoringSystem;Trusted_Connection=True;Encrypt=false";
           

            string connectionString1 = "Server=db48160.public.databaseasp.net; Database=db48160; User Id=db48160; Password=m-9KF4d_q=6M; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;";
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        //public DbSet<ActivityFile> ActivityFile { get; set; }
        public DbSet<EmailEteCommunication> EmailEteCommunication { get; set; }

        public DbSet<PersonnelDutyLogs> PersonnelDutyLogs { get; set; }

        public DbSet<ApprovalProccess> ApprovalProccess { get; set; }
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
           
        }

    }
}
