using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.TenantMemberManager.Commands;

public class UpdateTenantMemberResult
{
    public TenantMember? Data { get; set; }
}

public class UpdateTenantMemberRequest : IRequest<UpdateTenantMemberResult>
{
    public string? Id { get; init; }
    public string? UserId { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? TenantId { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateTenantMemberValidator : AbstractValidator<UpdateTenantMemberRequest>
{
    public UpdateTenantMemberValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.TenantId).NotEmpty();
    }
}

public class UpdateTenantMemberHandler : IRequestHandler<UpdateTenantMemberRequest, UpdateTenantMemberResult>
{
    private readonly ICommandRepository<TenantMember> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTenantMemberHandler(
        ICommandRepository<TenantMember> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateTenantMemberResult> Handle(UpdateTenantMemberRequest request, CancellationToken cancellationToken)
    {


        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }


        entity.UpdatedById = request.UpdatedById;

        entity.UserId = request.UserId;
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.TenantId = request.TenantId;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new UpdateTenantMemberResult
        {
            Data = entity
        };
    }
}

