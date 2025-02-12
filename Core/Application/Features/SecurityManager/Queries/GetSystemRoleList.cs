using Application.Common.Services.SecurityManager;
using FluentValidation;
using MediatR;

namespace Application.Features.SecurityManager.Queries;



public class GetSystemRoleListResult
{
    public List<GetSystemRoleListResultDto>? Data { get; init; }
}

public class GetSystemRoleListRequest : IRequest<GetSystemRoleListResult>
{
}

public class GetSystemRoleListValidator : AbstractValidator<GetSystemRoleListRequest>
{
    public GetSystemRoleListValidator()
    {
    }
}

public class GetSystemRoleListHandler : IRequestHandler<GetSystemRoleListRequest, GetSystemRoleListResult>
{
    private readonly ISecurityService _securityService;

    public GetSystemRoleListHandler(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<GetSystemRoleListResult> Handle(GetSystemRoleListRequest request, CancellationToken cancellationToken)
    {
        var result = await _securityService.GetSystemRoleListAsync(
            cancellationToken
            );

        return new GetSystemRoleListResult
        {
            Data = result
        };
    }
}

