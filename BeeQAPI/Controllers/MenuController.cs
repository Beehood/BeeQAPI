using BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize] 
[ApiController]
[Route("BeeQAPI")]
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
            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim == null || string.IsNullOrEmpty(userClaim.Value))
                return Unauthorized("Invalid token");

            string email = userClaim.Value;

           var result = await _bal.GetSidebar(email); //  pass email

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.ToString());
        }
    }
}