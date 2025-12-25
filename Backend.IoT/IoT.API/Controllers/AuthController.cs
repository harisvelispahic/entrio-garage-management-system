using IoT.Application.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoT.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly LoginService _loginService;

    public AuthController(LoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _loginService.LoginAsync(request);
        return Ok(new { token });
    }

    //[Authorize]
    //[HttpGet("auth-test")]
    //public IActionResult AuthTest()
    //{
    //    return Ok("AUTH OK");
    //}
}
