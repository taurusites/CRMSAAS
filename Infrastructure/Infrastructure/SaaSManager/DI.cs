using Application.Common.Services.SaaSManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.SaaSManager;

public static class DI
{
    public static IServiceCollection RegisterSaaSManager(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<ISaaSService, SaaSService>();

        return services;
    }
}
