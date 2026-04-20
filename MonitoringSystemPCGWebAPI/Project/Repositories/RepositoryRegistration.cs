
//In Program.cs (Main project) add this 
//builder.Services.AddRepositories();

using ApplicationContexts;
using Repositories.Classes;
using Repositories.Interfaces;

namespace Repositories
{
    public static class RepositoryRegistration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IActivityTypeRepository, ActivityTypeRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IPersonnelRepository, PersonnelRepository>();
            services.AddScoped<IPersonnelActivityRepository, PersonnelActivityRepository>();
            services.AddScoped<IRankRepository, RankRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISidebarRepository, SidebarRepository>();
            services.AddScoped<ISidebarRoleMappingRepository, SidebarRoleMappingRepository>();
            services.AddScoped<IUsertblRepository, UsertblRepository>();
            services.AddScoped<IRankCategoryRepository, RankCategoryRepository>();
            services.AddScoped<IEnlistmentRecordRepository, EnlistmentRecordRepository>();
            services.AddScoped<IPersonnelPromotionRepository, PersonnelPromotionRepository>();
            services.AddScoped<ILeaveTypesRepository, LeaveTypesRepository>();
            services.AddScoped<IOtpVerificationsRepository, OtpVerificationsRepository>();
            services.AddScoped<IEmailEteCommunicationRepository, EmailEteCommunicationRepository>();
            services.AddScoped<IPersonnelDutyLogsRepository, PersonnelDutyLogsRepository>();
            services.AddScoped<IApprovalProccessRepository, ApprovalProccessRepository>();
        }
    }
}
