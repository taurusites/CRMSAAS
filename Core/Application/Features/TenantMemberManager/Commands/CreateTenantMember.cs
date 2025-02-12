using Application.Common.Extensions;
using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TenantMemberManager.Commands;

public class CreateTenantMemberResult
{
    public TenantMember? Data { get; set; }
}

public class CreateTenantMemberRequest : IRequest<CreateTenantMemberResult>
{
    public string? UserId { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? TenantId { get; init; }
    public string? CreatedById { get; init; }

    public class CreateTenantMemberValidator : AbstractValidator<CreateTenantMemberRequest>
    {
        public CreateTenantMemberValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.TenantId).NotEmpty();
        }
    }

    public class CreateTenantMemberHandler : IRequestHandler<CreateTenantMemberRequest, CreateTenantMemberResult>
    {
        private readonly ICommandRepository<TenantMember> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTenantMemberHandler(
            ICommandRepository<TenantMember> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateTenantMemberResult> Handle(CreateTenantMemberRequest request, CancellationToken cancellationToken = default)
        {
            var existing = await _repository
                .GetQuery()
                .AsNoTracking()
                .IsDeletedEqualTo(false)
                .Where(x => x.UserId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (existing != null)
            {
                throw new Exception($"User already used as tenant member.");
            }

            var entity = new TenantMember();
            entity.CreatedById = request.CreatedById;

            entity.UserId = request.UserId;
            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.TenantId = request.TenantId;


            await _repository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new CreateTenantMemberResult
            {
                Data = entity
            };
        }
    }
}