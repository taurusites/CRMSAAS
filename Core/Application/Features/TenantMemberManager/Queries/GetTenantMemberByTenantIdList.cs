using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TenantMemberManager.Queries;

public record GetTenantMemberByTenantIdListDto
{
    public string? Id { get; init; }
    public string? UserId { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public Tenant? Tenant { get; init; }
    public string? TenantName { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetTenantMemberByTenantIdListProfile : Profile
{
    public GetTenantMemberByTenantIdListProfile()
    {
        CreateMap<TenantMember, GetTenantMemberByTenantIdListDto>()
            .ForMember(
                dest => dest.TenantName,
                opt => opt.MapFrom(src => src.Tenant != null ? src.Tenant.Name : string.Empty)
            );

    }
}

public class GetTenantMemberByTenantIdListResult
{
    public List<GetTenantMemberByTenantIdListDto>? Data { get; init; }
}

public class GetTenantMemberByTenantIdListRequest : IRequest<GetTenantMemberByTenantIdListResult>
{
    public string? TenantId { get; init; }
}


public class GetTenantMemberByTenantIdListHandler : IRequestHandler<GetTenantMemberByTenantIdListRequest, GetTenantMemberByTenantIdListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetTenantMemberByTenantIdListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetTenantMemberByTenantIdListResult> Handle(GetTenantMemberByTenantIdListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .TenantMember
            .AsNoTracking()
            .IsDeletedEqualTo(false)
            .Include(x => x.Tenant)
            .Where(x => x.TenantId == request.TenantId)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetTenantMemberByTenantIdListDto>>(entities);

        return new GetTenantMemberByTenantIdListResult
        {
            Data = dtos
        };
    }


}



