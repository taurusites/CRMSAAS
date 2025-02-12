using Application.Common.Services.SaaSManager;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.SaaSManager;


public class TenantService : ITenantService
{
    public string TenantId { get; set; }

    public TenantService(
        IHttpContextAccessor httpContextAccessor
        )
    {
        TenantId = httpContextAccessor.HttpContext?.Items["TenantId"]?.ToString() ?? "";
    }

}

