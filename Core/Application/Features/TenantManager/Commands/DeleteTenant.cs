using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.TenantManager.Commands;

public class DeleteTenantResult
{
    public Tenant? Data { get; set; }
}

public class DeleteTenantRequest : IRequest<DeleteTenantResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteTenantValidator : AbstractValidator<DeleteTenantRequest>
{
    public DeleteTenantValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteTenantHandler : IRequestHandler<DeleteTenantRequest, DeleteTenantResult>
{
    private readonly ICommandRepository<Tenant> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTenantHandler(
        ICommandRepository<Tenant> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteTenantResult> Handle(DeleteTenantRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.DeletedById;

        _repository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeleteTenantResult
        {
            Data = entity
        };
    }
}

