using Chat.Config;
using Chat.DTOs;
using Chat.Models;
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
        var user = await _userManager.FindByNameAsync(loginModel.Name);

        if (user == null)
        {
            return new AuthResponse
            {
                Response = Responses.NotFound,
                Message = "User not found."
            };
        }

        var result = await _signInManager.PasswordSignInAsync(loginModel.Name, loginModel.Password, false, false);

        if (result.Succeeded)
        {
            var jwtToken = await CreateJwtToken(user);
            return new AuthResponse
            {
                Response = Responses.Success,
                Message = "You have successfully logged in",
                Token = jwtToken,
                User = new UserDTO 
                { 
                    Id = user.Id,
                    Name = user.UserName,
                },
            };
        }
        else
        {
            return new AuthResponse
            {
                Response = Responses.Unauthorized,
                Message = "Login or password is incorrect"
            };
        }
    }

    public async Task<AuthResponse> Logout()
    {
        await _signInManager.SignOutAsync();

        return new AuthResponse
        {
            Response = Responses.Success,
            Message = "You have successfully logged out"
        };
    }

    public async Task<AuthResponse> Signup(SignupModel signupModel)
    {
        AuthResponse response = null;
        var checkName = await _userManager.FindByNameAsync(signupModel.Name);

        if(checkName != null)
        {
            return new AuthResponse
            {
                Response = Responses.BadReques,
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
            await _userService.SaveAsync();
            await _signInManager.SignInAsync(user, isPersistent: false);
            var jwtToken = await CreateJwtToken(user);

            return new AuthResponse
            {
                Response = Responses.Success,
                Message = "User has been created successfuly",
                Token = jwtToken,
                User = new UserDTO
                {
                    Id = user.Id,
                    Name = user.UserName
                }
            };
        }
        else
        {
            return new AuthResponse
            {
                Response = Responses.BadReques,
                Message = registerResult.Errors?.FirstOrDefault()?.Description
            };
        }
    }

    private async Task<string> CreateJwtToken(AppIdentityUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var rolesString = string.Join(',', roles);
        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SettingsManager.AppSettings["JWT:Secret"]));
        var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        var tokenOptions = new JwtSecurityToken(
            issuer: SettingsManager.AppSettings["Jwt:ValidIssuer"],
            audience: SettingsManager.AppSettings["Jwt:ValidAudience"],
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var tokenString = jwtSecurityTokenHandler.WriteToken(tokenOptions);

        return tokenString;
    }
}
