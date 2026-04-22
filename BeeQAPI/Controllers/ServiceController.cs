using BAL.ContractIF.BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Claims;

[ApiController]
public class ServiceController : ControllerBase
{
    private readonly IBAL_Service _Service;
    private readonly ILogger<ServiceController> _logger;

    public ServiceController(ILogger<ServiceController> logger, IBAL_Service service)
    {
        _logger = logger;
        _Service = service;
    }

    // 🔥 LIST
    [HttpPost("ServiceList")]
    [Authorize(Policy = "Service.View")]
    [ProducesResponseType(typeof(APIGetResponseModel<List<ServiceModel>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIGetResponseModel<List<ServiceModel>>>> ServiceList([FromBody] ServiceSearchKeys obj)
    {
        var result = await _Service.ServiceList(obj, transaction: null);
        return Ok(result);
    }

    // 🔥 GET BY ID
    [HttpPost("ServiceById")]
    [Authorize(Policy = "Service.View")]
    [ProducesResponseType(typeof(APIGetResponseModel<ServiceModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIGetResponseModel<ServiceModel>>> ServiceById([FromBody] ServiceSearchKeys obj)
    {
        var result = await _Service.ServiceById(obj, transaction: null);
        return Ok(result);
    }

    // 🔥 CREATE
    [HttpPost("NewService")]
    [Authorize(Policy = "Service.Create")]
    [ProducesResponseType(typeof(APIGetResponseModel<int>), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIGetResponseModel<int>>> ServiceCreate([FromBody] ServiceModel data)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var result = await _Service.ServiceCreate(data, userId, transaction: null);
        return Ok(result);
    }

    // 🔥 UPDATE
    [HttpPost("ServiceUpdate")]
    [Authorize(Policy = "Service.Update")]
    [ProducesResponseType(typeof(APIGetResponseModel<int>), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIGetResponseModel<int>>> ServiceUpdate([FromBody] ServiceModel data)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var result = await _Service.ServiceUpdate(data, userId, transaction: null);
        return Ok(result);
    }

    // 🔥 STATUS
    [HttpPost("ServiceStatus")]
    [Authorize(Policy = "Service.Status")]
    [ProducesResponseType(typeof(APIGetResponseModel<int>), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIGetResponseModel<int>>> ServiceStatus([FromBody] ServiceSearchKeys obj)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var result = await _Service.ServiceStatus(obj, userId, transaction: null);
        return Ok(result);
    }

    // 🔥 SERVICE DROPDOWN (SaaS Ready)
    //[HttpPost("ServiceDropdown")]
    //[Authorize]
    //[ProducesResponseType(typeof(APIGetResponseModel<List<ModelDropdown>>), StatusCodes.Status200OK)]
    //public async Task<ActionResult<APIGetResponseModel<List<ModelDropdown>>>> ServiceDropdown()
    //{
    //    var org = User.FindFirst("OrganizationId")?.Value;
    //    int orgId = string.IsNullOrEmpty(org) ? 0 : int.Parse(org);

    //    var result = await _Service.ServiceDropdown(orgId, transaction: null);
    //    return Ok(result);
    //}
}