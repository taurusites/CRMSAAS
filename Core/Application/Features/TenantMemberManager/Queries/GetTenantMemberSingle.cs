using Application.Common.CQS.Queries;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TenantMemberManager.Queries;

public record GetTenantMemberSingleDto
{
    public string? Id { get; init; }
    public string? UserId { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public Tenant? Tenant { get; init; }

    public string? TenantName { get; init; }
}

public class GetTenantMemberSingleProfile : Profile
{
    public GetTenantMemberSingleProfile()
    {
        CreateMap<TenantMember, GetTenantMemberSingleDto>()
            .ForMember(
                dest => dest.TenantName,
                opt => opt.MapFrom(src => src.Tenant != null ? src.Tenant.Name : string.Empty)
            );
    }
}

public class GetTenantMemberSingleResult
{
    public GetTenantMemberSingleDto? Data { get; init; }
}

public class GetTenantMemberSingleRequest : IRequest<GetTenantMemberSingleResult>
{
    public string? Id { get; init; }
}

public class GetTenantMemberSingleValidator : AbstractValidator<GetTenantMemberSingleRequest>
{
    public GetTenantMemberSingleValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetTenantMemberSingleHandler : IRequestHandler<GetTenantMemberSingleRequest, GetTenantMemberSingleResult>
{
    private readonly IQueryContext _context;
    private readonly IMapper _mapper;

    public GetTenantMemberSingleHandler(
        IQueryContext context,
        IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetTenantMemberSingleResult> Handle(GetTenantMemberSingleRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .TenantMember
            .AsNoTracking()
                .Include(x => x.Tenant)
            .AsQueryable();

        query = query
            .Where(x => x.Id == request.Id);

        var entity = await query.SingleOrDefaultAsync(cancellationToken);

        var dto = _mapper.Map<GetTenantMemberSingleDto>(entity);

        return new GetTenantMemberSingleResult
        {
            Data = dto
        };
    }
}