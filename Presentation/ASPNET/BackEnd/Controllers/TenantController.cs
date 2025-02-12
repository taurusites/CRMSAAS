using Application.Features.TenantManager.Commands;
using Application.Features.TenantManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class TenantController : BaseApiController
{
    public TenantController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateTenant")]
    public async Task<ActionResult<ApiSuccessResult<CreateTenantResult>>> CreateTenantAsync(CreateTenantRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateTenantResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateTenantAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateTenant")]
    public async Task<ActionResult<ApiSuccessResult<UpdateTenantResult>>> UpdateTenantAsync(UpdateTenantRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateTenantResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateTenantAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteTenant")]
    public async Task<ActionResult<ApiSuccessResult<DeleteTenantResult>>> DeleteTenantAsync(DeleteTenantRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteTenantResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteTenantAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetTenantSingle")]
    public async Task<ActionResult<ApiSuccessResult<GetTenantSingleResult>>> GetTenantSingleAsync(
        CancellationToken cancellationToken,
        [FromQuery] string id
        )
    {
        var request = new GetTenantSingleRequest { Id = id };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetTenantSingleResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetTenantSingleAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetTenantList")]
    public async Task<ActionResult<ApiSuccessResult<GetTenantListResult>>> GetTenantListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetTenantListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetTenantListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetTenantListAsync)}",
            Content = response
        });
    }


}


