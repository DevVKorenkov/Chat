using Chat.Config;
using Chat.DTOs;
using Chat.Helpers;
using Chat.Models;
using Chat.Models.Responses;
using Chat.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chat.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppIdentityUser> _userManager;
    private readonly SignInManager<AppIdentityUser> _signInManager;
    private readonly IUserService _userService;

    public AuthService(
        UserManager<AppIdentityUser> userManager,
        SignInManager<AppIdentityUser> signInManager,
        IUserService userService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userService = userService;
    }

    public async Task<AuthResponse> Login(LoginModel loginModel)
    {
        var user = await _userService.GetAsync(u => u.UserName == loginModel.Name);

        if (user == null)
        {
            return new AuthResponse
            {
                Response = ResponseStatus.NotFound,
                Message = "User not found."
            };
        }

        var loginResult = await _signInManager.PasswordSignInAsync(loginModel.Name, loginModel.Password, false, false);

        if (loginResult.Succeeded)
        {
            var jwtToken = await CreateJwtToken(user);
            return new AuthResponse
            {
                Response = ResponseStatus.Success,
                Message = "You have successfully logged in",
                Token = jwtToken,
                User = UserHelper.GetUserDto(user),
            };
        }
        else
        {
            return new AuthResponse
            {
                Message = "Login or password is incorrect"
            };
        }
    }

    public async Task<AuthResponse> Logout()
    {
        await _signInManager.SignOutAsync();

        return new AuthResponse
        {
            Response = ResponseStatus.Success,
            Message = "You have successfully logged out"
        };
    }

    public async Task<AuthResponse> Signup(SignupModel signupModel)
    {
        var checkName = await _userManager.FindByNameAsync(signupModel.Name);

        if(checkName != null)
        {
            return new AuthResponse
            {
                Response = ResponseStatus.BadRequest,
                Message = "There is another user with the same name, please make different name."
            };
        }

        var user = new AppIdentityUser
        {
            UserName = signupModel.Name,
        };

        var registerResult = await _userManager.CreateAsync(user, signupModel.Password);

        if (registerResult.Succeeded)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            await _userManager.AddClaimsAsync(user, claims);

            await _userService.SaveAsync();

            await _signInManager.SignInAsync(user, isPersistent: false);
            var jwtToken = await CreateJwtToken(user);

            return new AuthResponse
            {
                Response = ResponseStatus.Success,
                Message = "User has been created successfuly",
                Token = jwtToken,
                User = UserHelper.GetUserDto(user),
            };
        }
        else
        {
            return new AuthResponse
            {
                Response = ResponseStatus.BadRequest,
                Message = registerResult.Errors?.FirstOrDefault()?.Description
            };
        }
    }

    private async Task<string> CreateJwtToken(AppIdentityUser user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
        };
        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SettingsManager.AppSettings["JWT:Secret"]));
        var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        var tokenOptions = new JwtSecurityToken(
            issuer: SettingsManager.AppSettings["Jwt:ValidIssuer"],
            audience: SettingsManager.AppSettings["Jwt:ValidAudience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var tokenString = jwtSecurityTokenHandler.WriteToken(tokenOptions);

        return tokenString;
    }
}
