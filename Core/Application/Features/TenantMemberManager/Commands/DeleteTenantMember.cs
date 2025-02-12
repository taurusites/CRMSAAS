using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.TenantMemberManager.Commands;

public class DeleteTenantMemberResult
{
    public TenantMember? Data { get; set; }
}

public class DeleteTenantMemberRequest : IRequest<DeleteTenantMemberResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteTenantMemberValidator : AbstractValidator<DeleteTenantMemberRequest>
{
    public DeleteTenantMemberValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteTenantMemberHandler : IRequestHandler<DeleteTenantMemberRequest, DeleteTenantMemberResult>
{
    private readonly ICommandRepository<TenantMember> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTenantMemberHandler(
        ICommandRepository<TenantMember> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<DeleteTenantMemberResult> Handle(DeleteTenantMemberRequest request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.DeletedById;

        _repository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeleteTenantMemberResult
        {
            Data = entity
        };
    }
}

