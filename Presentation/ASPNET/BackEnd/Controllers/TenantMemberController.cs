using Application.Features.TenantMemberManager.Commands;
using Application.Features.TenantMemberManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class TenantMemberController : BaseApiController
{
    public TenantMemberController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateTenantMember")]
    public async Task<ActionResult<ApiSuccessResult<CreateTenantMemberResult>>> CreateTenantMemberAsync(CreateTenantMemberRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateTenantMemberResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateTenantMemberAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateTenantMember")]
    public async Task<ActionResult<ApiSuccessResult<UpdateTenantMemberResult>>> UpdateTenantMemberAsync(UpdateTenantMemberRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateTenantMemberResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateTenantMemberAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteTenantMember")]
    public async Task<ActionResult<ApiSuccessResult<DeleteTenantMemberResult>>> DeleteTenantMemberAsync(DeleteTenantMemberRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteTenantMemberResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteTenantMemberAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetTenantMemberList")]
    public async Task<ActionResult<ApiSuccessResult<GetTenantMemberListResult>>> GetTenantMemberListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetTenantMemberListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetTenantMemberListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetTenantMemberListAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetTenantMemberByTenantIdList")]
    public async Task<ActionResult<ApiSuccessResult<GetTenantMemberByTenantIdListResult>>> GetTenantMemberByTenantIdListAsync(
    CancellationToken cancellationToken,
    [FromQuery] string tenantId
    )
    {
        var request = new GetTenantMemberByTenantIdListRequest { TenantId = tenantId };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetTenantMemberByTenantIdListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetTenantMemberByTenantIdListAsync)}",
            Content = response
        });
    }


}


