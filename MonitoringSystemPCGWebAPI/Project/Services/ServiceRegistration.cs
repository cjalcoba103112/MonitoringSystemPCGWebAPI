
//In Program.cs (Main project) add this 
//builder.Services.AddServices();

using ApplicationContexts;
using MonitoringSystemPCGWebAPI.Project.Services.Classes;
using MonitoringSystemPCGWebAPI.Project.Services.Interfaces;
using Services.Classes;
using Services.Interfaces;

namespace Services
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            
            services.AddScoped<IActivityTypeService, ActivityTypeService>();
services.AddScoped<IDepartmentService, DepartmentService>();
services.AddScoped<IPersonnelService, PersonnelService>();
services.AddScoped<IPersonnelActivityService, PersonnelActivityService>();
services.AddScoped<IRankService, RankService>();
services.AddScoped<IRoleService, RoleService>();
services.AddScoped<ISidebarService, SidebarService>();
services.AddScoped<ISidebarRoleMappingService, SidebarRoleMappingService>();
services.AddScoped<IUsertblService, UsertblService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IRankCategoryService, RankCategoryService>();
            services.AddScoped<IEnlistmentRecordService, EnlistmentRecordService>();
            services.AddScoped<IPersonnelPromotionService, PersonnelPromotionService>();
            services.AddScoped<ILeaveTypesService, LeaveTypesService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOtpVerificationsService, OtpVerificationsService>();
            services.AddScoped<IEmailEteCommunicationService, EmailEteCommunicationService>();

        }
    }
}
