using Application.Common.Extensions;
using Application.Common.Repositories;
using Application.Common.Services.SaaSManager;
using Domain.Common;
using Infrastructure.DataAccessManager.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccessManager.EFCore.Repositories;


public class CommandRepository<T> : ICommandRepository<T> where T : BaseEntity
{
    protected readonly CommandContext _context;
    private readonly ITenantService _tenantService;

    public CommandRepository(CommandContext context, ITenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity is IHasTenantId hasTenantId)
        {
            hasTenantId.TenantId = _tenantService.TenantId;
        }
        entity.CreatedAtUtc = DateTime.UtcNow;
        await _context.AddAsync(entity, cancellationToken);
    }

    public void Create(T entity)
    {
        if (entity is IHasTenantId hasTenantId)
        {
            hasTenantId.TenantId = _tenantService.TenantId;
        }
        entity.CreatedAtUtc = DateTime.UtcNow;
        _context.Add(entity);
    }

    public void Update(T entity)
    {
        entity.UpdatedAtUtc = DateTime.UtcNow;
        _context.Update(entity);
    }

    public void Delete(T entity)
    {
        entity.IsDeleted = true;
        entity.UpdatedAtUtc = DateTime.UtcNow;
        _context.Update(entity);
    }

    public void Purge(T entity)
    {
        _context.Remove(entity);
    }

    public virtual async Task<T?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<T>().AsQueryable();
        if (typeof(IHasTenantId).IsAssignableFrom(typeof(T)))
        {
            query = query.Where(x => ((IHasTenantId)x).TenantId == _tenantService.TenantId);
        }
        var entity = await query
            .IsDeletedEqualTo()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity;
    }

    public virtual T? Get(string id)
    {
        var query = _context.Set<T>().AsQueryable();
        if (typeof(IHasTenantId).IsAssignableFrom(typeof(T)))
        {
            query = query.Where(x => ((IHasTenantId)x).TenantId == _tenantService.TenantId);
        }
        var entity = query
            .IsDeletedEqualTo()
            .SingleOrDefault(x => x.Id == id);

        return entity;
    }

    public virtual IQueryable<T> GetQuery()
    {
        var query = _context.Set<T>().AsQueryable();
        if (typeof(IHasTenantId).IsAssignableFrom(typeof(T)))
        {
            query = query.Where(x => ((IHasTenantId)x).TenantId == _tenantService.TenantId);
        }
        return query;
    }


}
