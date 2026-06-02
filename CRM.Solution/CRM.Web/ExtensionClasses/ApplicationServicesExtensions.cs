using CRM.Application.BackgroundJobs;
using CRM.Application.MappingProfile;
using CRM.Application.Repositories;
using CRM.Application.Services;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.BackgroundJobs;
using CRM.Interface.IServices.Notification;

namespace CRM.Web.ExtensionClasses
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // AutoMapper - scans the entire assembly of ProductProfile
            services.AddAutoMapper(typeof(UserProfile).Assembly);

            // HttpContext
            services.AddHttpContextAccessor();


            // Core Repositories & Services
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<INotificationService, NotificationService>();

            // Localization & Audit
            services.AddScoped<ILocalizationService, LocalizationService>();
            services.AddScoped<IEntityAuditHelper, EntityAuditHelper>();
            services.AddScoped<ICrmBackgroundJobs, CrmBackgroundJobs>();

            return services;
        }
    }
}
