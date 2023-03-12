using Chat.Models;
using Chat.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost, Route("signup")]
    public async Task<IActionResult> SignUp(SignupModel signupModel)
    {
        ObjectResult response = null;

        var signupResult = await _authService.Signup(signupModel);

        switch (signupResult.Response)
        {
            case Responses.Success:
                response = Ok(signupResult);
                break;
            case Responses.NotFound:
                response = NotFound(signupResult);
                break;
            case Responses.BadReques:
                response = BadRequest(signupResult);
                break;
        }

        return response;
    }

    [HttpPost, Route("login")]
    public async Task<IActionResult> Login(SignupModel signupModel)
    {
        return Ok();
    }

    [HttpPost, Route("logout")]
    public async Task<IActionResult> Logout(SignupModel signupModel)
    {
        return Ok();
    }
}
