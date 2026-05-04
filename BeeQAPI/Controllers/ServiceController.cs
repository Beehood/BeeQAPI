using BAL.ContractIF.BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Net;
using System.Security.Claims;

[Route("BeeQAPI")]
[ApiController]
public class ServiceController : ControllerBase
{
    private readonly IBAL_Service _bal;

    public ServiceController(IBAL_Service bal)
    {
        _bal = bal;
    }

    // ========================
    // GET ALL
    // ========================
    [Authorize(Policy = "SERVICE_VIEW")]
    [HttpPost("ServiceList")]
    [ProducesResponseType(typeof(APIGetResponseModel<List<ServiceModel>>), (int)HttpStatusCode.OK)]
    public async Task<APIGetResponseModel<List<ServiceModel>>> GetAll([FromBody] PaginationRequestDto request)
    {
        var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

        var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return await _bal.GetAll(request, roles, email, transaction: null);
    }

    // ========================
    // GET BY ID
    // ========================
    [Authorize(Policy = "SERVICE_VIEW")]
    [HttpPost("ServiceById")]
    [ProducesResponseType(typeof(APIGetResponseModel<ServiceModel>), (int)HttpStatusCode.OK)]
    public async Task<APIGetResponseModel<ServiceModel>> GetById([FromBody] long id)
    {
        var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

        var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return await _bal.GetById(id, roles, email, transaction: null);
    }

    // ========================
    // CREATE
    // ========================
    [Authorize(Policy = "SERVICE_CREATE")]
    [HttpPost("NewService")]
    [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
    public async Task<APIGetResponseModel<int>> Create([FromBody] ServiceRequestDto request)
    {
        var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

        var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return await _bal.Create(request, roles, email, transaction: null);
    }

    // ========================
    // UPDATE
    // ========================
    [Authorize(Policy = "SERVICE_UPDATE")]
    [HttpPost("EditService")]
    [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
    public async Task<APIGetResponseModel<int>> Update([FromBody] ServiceRequestDto request)
    {
        var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

        var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return await _bal.Update(request, roles, email, transaction: null);
    }

    // ========================
    // STATUS
    // ========================
    [Authorize(Policy = "SERVICE_STATUS")]
    [HttpPost("ServiceStatus")]
    [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
    public async Task<APIGetResponseModel<int>> ChangeStatus([FromBody] long id)
    {
        var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

        var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return await _bal.ChangeStatus(id, roles, email, transaction: null);
    }

    // ========================
    // DROPDOWN
    // ========================
    [Authorize(Policy = "SERVICE_VIEW")]
    [HttpGet("ServiceDropdown")]
    [ProducesResponseType(typeof(APIGetResponseModel<List<DropdownModel>>), (int)HttpStatusCode.OK)]
    public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown()
    {
        var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return await _bal.GetDropdown(email, transaction: null);
    }
}

