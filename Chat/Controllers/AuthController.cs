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
        ObjectResult response;

        var signupResult = await _authService.Signup(signupModel);

        switch (signupResult.Response)
        {
            case ResponseStatus.Success:
                response = Ok(signupResult);
                break;
            case ResponseStatus.NotFound:
                response = NotFound(signupResult);
                break;
            case ResponseStatus.BadRequest:
                response = BadRequest(signupResult);
                break;
            default:
                response = BadRequest(signupResult);
                break;
        }

        return response;
    }

    [HttpPost, Route("login")]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        ObjectResult response;

        var loginResponse = await _authService.Login(loginModel);

        switch (loginResponse.Response)
        {
            case ResponseStatus.Success:
                response = Ok(loginResponse);
                break;
            case ResponseStatus.NotFound:
                response = NotFound(loginResponse);
                break;
            case ResponseStatus.BadRequest:
                response = BadRequest(loginResponse);
                break;
            default:
                response = BadRequest(loginResponse);
                break;
        }

        return response;
    }

    [HttpGet, Route("logout")]
    public async Task<IActionResult> Logout()
    {
        var loginResponse = await _authService.Logout();

        return Ok(loginResponse);
    }
}
