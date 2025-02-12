using Application.Common.Services.SecurityManager;
using FluentValidation;
using MediatR;

namespace Application.Features.SecurityManager.Queries;



public class GetSystemUserListResult
{
    public List<GetSystemUserListResultDto>? Data { get; init; }
}

public class GetSystemUserListRequest : IRequest<GetSystemUserListResult>
{
}

public class GetSystemUserListValidator : AbstractValidator<GetSystemUserListRequest>
{
    public GetSystemUserListValidator()
    {
    }
}

public class GetSystemUserListHandler : IRequestHandler<GetSystemUserListRequest, GetSystemUserListResult>
{
    private readonly ISecurityService _securityService;

    public GetSystemUserListHandler(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<GetSystemUserListResult> Handle(GetSystemUserListRequest request, CancellationToken cancellationToken)
    {
        var result = await _securityService.GetSystemUserListAsync(
            cancellationToken
            );

        return new GetSystemUserListResult
        {
            Data = result
        };
    }
}


