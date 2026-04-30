using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/profile")]
[ApiController]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IBAL_User _bal;

    public ProfileController(IBAL_User bal)
    {
        _bal = bal;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var userIdClaim = User.FindFirst("user_id")?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        long userId = Convert.ToInt64(userIdClaim);

        var result = await _bal.GetProfileById(userId);

        return Ok(result);
    }
}