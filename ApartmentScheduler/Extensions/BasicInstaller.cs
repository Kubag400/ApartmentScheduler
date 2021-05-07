using ApartmentScheduler.Interfaces;
using ApartmentScheduler.Services;
using AspNetCoreHero.ToastNotification;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApartmentScheduler.Extensions
{
    public class BasicInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddRazorPages();
            services.AddScoped<IDataService, DataService>();
            services.AddNotyf(config =>
            { config.DurationInSeconds = 5; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });
            services.AddControllersWithViews();
        }
    }
}
