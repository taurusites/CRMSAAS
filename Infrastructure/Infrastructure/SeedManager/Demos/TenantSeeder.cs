using Application.Common.Repositories;
using Application.Common.Services.SaaSManager;
using Domain.Entities;
using Infrastructure.DataAccessManager.EFCore.Contexts;
using Infrastructure.SecurityManager.AspNetIdentity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedManager.Demos;

public class TenantSeeder
{
    private readonly DataContext _context;
    private readonly ICommandRepository<Tenant> _tenant;
    private readonly ICommandRepository<TenantMember> _tenantMember;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantService _tenantService;
    private readonly ISaaSService _saasService;

    public TenantSeeder(
        DataContext context,
        ICommandRepository<Tenant> tenant,
        ICommandRepository<TenantMember> tenantMember,
        IUnitOfWork unitOfWork,
        ITenantService tenantService,
        ISaaSService saasService
    )
    {
        _context = context;
        _tenant = tenant;
        _tenantMember = tenantMember;
        _unitOfWork = unitOfWork;
        _tenantService = tenantService;
        _saasService = saasService;
    }

    public async Task GenerateDataAsync()
    {
        var tenants = new List<Tenant>
        {
            new Tenant { Name = "Tenant-1", Reference = "", Description = "", IsActive = true },
            new Tenant { Name = "Tenant-2", Reference = "", Description = "", IsActive = true },
            new Tenant { Name = "Tenant-3", Reference = "", Description = "", IsActive = true },
            new Tenant { Name = "Tenant-4", Reference = "", Description = "", IsActive = true },
            new Tenant { Name = "Tenant-5", Reference = "", Description = "", IsActive = true },
        };

        var nonAdminUsers = await _context.Users
            .Where(u => u.Email != "admin@root.com" && u.Email.Contains("Tenant"))
            .Take(5)
            .OrderBy(x => x.Email)
            .ToListAsync();

        var userQueue = new Queue<ApplicationUser>(nonAdminUsers);

        foreach (var tenant in tenants)
        {
            await _tenant.CreateAsync(tenant);


            if (userQueue.Any())
            {
                var user = userQueue.Dequeue();

                var member = new TenantMember
                {
                    UserId = user.Id,
                    Name = $"Member for {tenant.Name}",
                    Description = $"Member for {tenant.Name}",
                    TenantId = tenant.Id
                };
                tenant.TenantMemberList.Add(member);
                await _tenantMember.CreateAsync(member);
            }

            if (tenant.Name != "Tenant-1")
            {
                _tenantService.TenantId = tenant.Id;
                await _saasService.InitTenantAsync();
            }
        }

        await _unitOfWork.SaveAsync();
    }
}