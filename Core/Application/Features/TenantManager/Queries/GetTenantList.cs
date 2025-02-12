using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TenantManager.Queries;

public record GetTenantListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Reference { get; init; }
    public string? Description { get; init; }
    public bool? IsActive { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetTenantListProfile : Profile
{
    public GetTenantListProfile()
    {
        CreateMap<Tenant, GetTenantListDto>();
    }
}

public class GetTenantListResult
{
    public List<GetTenantListDto>? Data { get; init; }
}

public class GetTenantListRequest : IRequest<GetTenantListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetTenantListHandler : IRequestHandler<GetTenantListRequest, GetTenantListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetTenantListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetTenantListResult> Handle(GetTenantListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .Tenant
            .AsNoTracking()
            .IsDeletedEqualTo(request.IsDeleted)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetTenantListDto>>(entities);

        return new GetTenantListResult
        {
            Data = dtos
        };
    }


}



