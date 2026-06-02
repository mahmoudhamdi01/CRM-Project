
using CRM.Application.Helpers;
using CRM.Application.Repositories;
using CRM.Infrastructure.Data;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.BackgroundJobs;
using CRM.Web.ExtensionClasses;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CRM.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //builder.Services.AddControllers();
            //// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            // ── Extension Methods ────────────────────────────────────────────────
            builder.Services.AddPresentationAndSwagger();
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddIdentityAndJwt(builder.Configuration);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
            builder.Host.UseSerilog();
            builder.Services.AddDbContext<ApplicationDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddHangfire(config => config
                   .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                   .UseSimpleAssemblyNameTypeSerializer()
                   .UseRecommendedSerializerSettings()
                   .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddHangfireServer();

            var app = builder.Build();


            app.UseMiddleware<LanguageMiddleware>();
            app.UseMiddleware<JwtCookieToHeaderMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwaggerWithUI();   // Swagger + UI + root redirect "/"
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseAuthorization();
            app.UseHangfireDashboard("/hangfire");
            using (var scope = app.Services.CreateScope())
            {
                var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
                var crmJobs = scope.ServiceProvider.GetRequiredService<ICrmBackgroundJobs>();

                // قفل الأقساط المتأخرة يومياً الساعة 1:00 صباحاً بالتوقيت العالمي
                recurringJobManager.AddOrUpdate(
                    "Update-Overdue-Installments",
                    () => crmJobs.ProcessOverdueInstallmentsAsync(),
                    Cron.Daily(1));

                // إرسال تذكيرات المتابعات يومياً الساعة 7:00 صباحاً بالتوقيت العالمي (9 صباحاً بمصر)
                recurringJobManager.AddOrUpdate(
                    "Daily-FollowUp-Reminders",
                    () => crmJobs.ProcessDailyFollowUpRemindersAsync(),
                    Cron.Daily(7));
            }
            app.MapControllers();

            try
            {
                Log.Information("Starting the CRM Application...");
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The Application failed to start correctly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}

