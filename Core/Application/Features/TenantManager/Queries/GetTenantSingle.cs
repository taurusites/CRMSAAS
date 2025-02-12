using Application.Common.CQS.Queries;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TenantManager.Queries;

public record GetTenantSingleDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Reference { get; init; }
    public string? Description { get; init; }
    public bool? IsActive { get; init; }
    public ICollection<TenantMember> TenantMemberList { get; set; } = new List<TenantMember>();
}

public class GetTenantSingleProfile : Profile
{
    public GetTenantSingleProfile()
    {
        CreateMap<Tenant, GetTenantSingleDto>();
    }
}

public class GetTenantSingleResult
{
    public GetTenantSingleDto? Data { get; init; }
}

public class GetTenantSingleRequest : IRequest<GetTenantSingleResult>
{
    public string? Id { get; init; }
}

public class GetTenantSingleValidator : AbstractValidator<GetTenantSingleRequest>
{
    public GetTenantSingleValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetTenantSingleHandler : IRequestHandler<GetTenantSingleRequest, GetTenantSingleResult>
{
    private readonly IQueryContext _context;
    private readonly IMapper _mapper;

    public GetTenantSingleHandler(
        IQueryContext context,
        IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetTenantSingleResult> Handle(GetTenantSingleRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .Tenant
            .AsNoTracking()
                .Include(x => x.TenantMemberList)
            .AsQueryable();

        query = query
            .Where(x => x.Id == request.Id);

        var entity = await query.SingleOrDefaultAsync(cancellationToken);

        var dto = _mapper.Map<GetTenantSingleDto>(entity);

        return new GetTenantSingleResult
        {
            Data = dto
        };
    }
}