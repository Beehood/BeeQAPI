using BAL.ContractIF.BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Claims;

[ApiController]
public class BranchServiceController : ControllerBase
{
    private readonly IBAL_BranchService _BranchService;
    private readonly ILogger<BranchServiceController> _logger;

    public BranchServiceController(ILogger<BranchServiceController> logger, IBAL_BranchService branchService)
    {
        _logger = logger;
        _BranchService = branchService;
    }

    // 🔥 LIST
    [HttpPost("BranchServiceList")]
    [Authorize(Policy = "BranchService.View")]
    [ProducesResponseType(typeof(APIGetResponseModel<List<BranchServiceModel>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIGetResponseModel<List<BranchServiceModel>>>> BranchServiceList([FromBody] BranchServiceSearchKeys obj)
    {
        var result = await _BranchService.BranchServiceList(obj, transaction: null);
        return Ok(result);
    }

    // 🔥 GET BY ID (FIXED)
    [HttpPost("BranchServiceById")]
    [Authorize(Policy = "BranchService.View")]
    [ProducesResponseType(typeof(APIGetResponseModel<BranchServiceModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIGetResponseModel<BranchServiceModel>>> BranchServiceById([FromBody] BranchServiceSearchKeys obj)
    {
        var result = await _BranchService.BranchServiceById(obj, transaction: null); // ✅ PASS OBJECT
        return Ok(result);
    }

    // 🔥 CREATE
    [HttpPost("NewBranchService")]
    [Authorize(Policy = "BranchService.Create")]
    [ProducesResponseType(typeof(APIGetResponseModel<int>), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIGetResponseModel<int>>> BranchServiceCreate([FromBody] BranchServiceModel data)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var result = await _BranchService.BranchServiceCreate(data, userId, transaction: null);
        return Ok(result);
    }

    // 🔥 UPDATE
    [HttpPost("BranchServiceUpdate")]
    [Authorize(Policy = "BranchService.Update")]
    [ProducesResponseType(typeof(APIGetResponseModel<int>), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIGetResponseModel<int>>> BranchServiceUpdate([FromBody] BranchServiceModel data)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var result = await _BranchService.BranchServiceUpdate(data, userId, transaction: null);
        return Ok(result);
    }

    // 🔥 STATUS
    [HttpPost("BranchServiceStatus")]
    [Authorize(Policy = "BranchService.Status")]
    [ProducesResponseType(typeof(APIGetResponseModel<int>), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIGetResponseModel<int>>> BranchServiceStatus([FromBody] BranchServiceSearchKeys obj)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var result = await _BranchService.BranchServiceStatus(obj, userId, transaction: null);
        return Ok(result);
    }
}