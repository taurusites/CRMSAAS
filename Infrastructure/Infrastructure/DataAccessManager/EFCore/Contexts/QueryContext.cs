using Application.Common.CQS.Queries;
using Application.Common.Services.SaaSManager;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccessManager.EFCore.Contexts;

public class QueryContext : DataContext, IQueryContext
{
    private readonly ITenantService _tenantService;
    public QueryContext(DbContextOptions<DataContext> options, ITenantService tenantService)
        : base(options)
    {
        _tenantService = tenantService;
    }

    public new IQueryable<T> Set<T>() where T : class
    {
        return base.Set<T>();
    }

    public IQueryable<T> SetWithTenantFilter<T>() where T : class, IHasTenantId
    {
        return base.Set<T>().Where(x => x.TenantId == _tenantService.TenantId);
    }
}
