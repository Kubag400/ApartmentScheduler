using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApartmentScheduler.Extensions
{
    public interface IInstaller
    {
        void InstallInAssembly(IServiceCollection services, IConfiguration configuration);
    }
}
