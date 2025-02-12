using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.TenantManager.Commands;

public class UpdateTenantResult
{
    public Tenant? Data { get; set; }
}

public class UpdateTenantRequest : IRequest<UpdateTenantResult>
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Reference { get; init; }
    public string? Description { get; init; }
    public bool? IsActive { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateTenantValidator : AbstractValidator<UpdateTenantRequest>
{
    public UpdateTenantValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.IsActive).NotEmpty();
    }
}

public class UpdateTenantHandler : IRequestHandler<UpdateTenantRequest, UpdateTenantResult>
{
    private readonly ICommandRepository<Tenant> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTenantHandler(
        ICommandRepository<Tenant> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateTenantResult> Handle(UpdateTenantRequest request, CancellationToken cancellationToken)
    {


        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.UpdatedById;

        entity.Name = request.Name;
        entity.Reference = request.Reference;
        entity.Description = request.Description;
        entity.IsActive = request.IsActive;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new UpdateTenantResult
        {
            Data = entity
        };
    }
}

