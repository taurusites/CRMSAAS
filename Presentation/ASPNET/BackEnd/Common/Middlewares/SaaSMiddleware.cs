namespace ASPNET.BackEnd.Common.Middlewares;

public class SaaSMiddleware
{
    private readonly RequestDelegate _next;

    public SaaSMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var tenantId = context.Request.Headers["TenantId"].FirstOrDefault()
                       ?? context.Request.Query["TenantId"];

        if (!string.IsNullOrEmpty(tenantId))
        {
            context.Items["TenantId"] = tenantId;
        }
        await _next(context);
    }
}
