using Application.Common.Repositories;
using Application.Common.Services.SaaSManager;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.TenantManager.Commands;

public class CreateTenantResult
{
    public Tenant? Data { get; set; }
}

public class CreateTenantRequest : IRequest<CreateTenantResult>
{
    public string? Name { get; init; }
    public string? Reference { get; init; }
    public string? Description { get; init; }
    public bool? IsActive { get; init; }
    public string? CreatedById { get; init; }

    public class CreateTenantValidator : AbstractValidator<CreateTenantRequest>
    {
        public CreateTenantValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.IsActive).NotEmpty();
        }
    }

    public class CreateTenantHandler : IRequestHandler<CreateTenantRequest, CreateTenantResult>
    {
        private readonly ICommandRepository<Tenant> _tenant;
        private readonly ITenantService _tenantService;
        private readonly ISaaSService _saasService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTenantHandler(
            ICommandRepository<Tenant> tenant,
            ITenantService tenantService,
            ISaaSService saasService,
            IUnitOfWork unitOfWork
            )
        {
            _tenant = tenant;
            _tenantService = tenantService;
            _saasService = saasService;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateTenantResult> Handle(CreateTenantRequest request, CancellationToken cancellationToken = default)
        {
            var tenant = new Tenant();
            tenant.CreatedById = request.CreatedById;

            tenant.Name = request.Name;
            tenant.Reference = request.Reference;
            tenant.Description = request.Description;
            tenant.IsActive = request.IsActive;

            await _tenant.CreateAsync(tenant, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            _tenantService.TenantId = tenant.Id;
            await _saasService.InitTenantAsync(cancellationToken);

            return new CreateTenantResult
            {
                Data = tenant
            };
        }
    }
}