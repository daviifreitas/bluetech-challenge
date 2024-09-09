using Microsoft.Extensions.DependencyInjection;
using Schedule.Domain.Interfaces.Service;
using Schedule.Domain.Services;

namespace Schedule.Infra.IoC
{
    public static class InjectServices
    {
        public static void SetServicesDependencyInjection(this IServiceCollection service)
        {
            

            
            //service.AddScoped<IAuthenticationService, IdentityAuthenticationService>();
        }
    }
}
