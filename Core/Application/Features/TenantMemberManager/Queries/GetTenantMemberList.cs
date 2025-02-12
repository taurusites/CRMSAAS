using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using Application.Common.Services.SecurityManager;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TenantMemberManager.Queries;

public record GetTenantMemberListDto
{
    public string? Id { get; init; }
    public string? UserId { get; init; }
    public string? UserEmail { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public Tenant? Tenant { get; init; }
    public string? TenantId { get; init; }
    public string? TenantName { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetTenantMemberListProfile : Profile
{
    public class UserEmailResolver : IValueResolver<TenantMember, GetTenantMemberListDto, string?>
    {
        private readonly ISecurityService _securityService;

        public UserEmailResolver(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public string Resolve(TenantMember source, GetTenantMemberListDto destination, string destMember, ResolutionContext context)
        {
            var email = _securityService.GetEmailAsync(source.UserId ?? "").Result;
            return email ?? string.Empty;
        }
    }

    public GetTenantMemberListProfile()
    {
        CreateMap<TenantMember, GetTenantMemberListDto>()
            .ForMember(
                dest => dest.TenantName,
                opt => opt.MapFrom(src => src.Tenant != null ? src.Tenant.Name : string.Empty)
            )
            .ForMember(
                dest => dest.UserEmail,
                opt => opt.MapFrom<UserEmailResolver>()
            );
    }
}

public class GetTenantMemberListResult
{
    public List<GetTenantMemberListDto>? Data { get; init; }
}

public class GetTenantMemberListRequest : IRequest<GetTenantMemberListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetTenantMemberListHandler : IRequestHandler<GetTenantMemberListRequest, GetTenantMemberListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetTenantMemberListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetTenantMemberListResult> Handle(GetTenantMemberListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .TenantMember
            .AsNoTracking()
                .Include(x => x.Tenant)
            .IsDeletedEqualTo(request.IsDeleted)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetTenantMemberListDto>>(entities);

        return new GetTenantMemberListResult
        {
            Data = dtos
        };
    }


}



