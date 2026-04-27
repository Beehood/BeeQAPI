using BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize] // JWT required
[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IBAL_Menu _bal;

    public MenuController(IBAL_Menu bal)
    {
        _bal = bal;
    }

    [HttpPost("sidebar")]
    public async Task<IActionResult> GetSidebar()
    {
        try
        {
            var userIdClaim = User.FindFirst("user_id")
                  ?? User.FindFirst("sub")
                  ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
                return Unauthorized("Invalid token");

            if (!long.TryParse(userIdClaim.Value, out long userId))
                return BadRequest("Invalid UserId");
            Console.WriteLine("UserId: " + userId); // 🔥 DEBUG

            var result = await _bal.GetSidebar(userId);

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }
}