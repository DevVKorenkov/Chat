using Chat.Extentions;
using Chat.Helpers;
using Chat.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers;

[Authorize]
[ApiController, Route("[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet, Route("getUser")]
    public async Task<IActionResult> GetUser(string name)
    {
        var appUser = await _userService.GetAsync(u => u.UserName == name);

        if (appUser == null)
        {
            return NotFound(new
            {
                Message = $"User {name} has not been found",
            });
        }

        var user = UserHelper.GetUserDto(appUser);

        return Ok(new
        {
            Message = $"User {name} has been found",
            User = user,
        });
    }

    [HttpGet, Route("getAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var appUsers = await _userService.GetAllAsync();

        if (!appUsers.Any())
        {
            return NotFound(new
            {
                Message = "Users have't been found",
            });
        }

        var users = UserHelper.GetUserDtos(appUsers);

        return Ok(new
        {
            Message = "Users have been gotten",
            Users = users,
        });
    }

    [HttpGet, Route("getAllUsersFromClan")]
    public async Task<IActionResult> GetAllUsersFromClan(string clanName)
    {
        var appUsers = await _userService.GetAllAsync(u => u.UserClan.Name == clanName);

        if (!appUsers.Any())
        {
            return NotFound(new
            {
                Message = "Users or clan haven't been found",
            });
        }

        var users = UserHelper.GetUserDtos(appUsers);

        return Ok(new
        {
            Message = "Users have been gotten",
            Users = users,
        });
    }

    [HttpGet, Route("getStartFromIndex")]
    public async Task<IActionResult> GetStartFromIndex(int userIndex)
    {
        var appUsers = await _userService.GetAllAsync(u => u.Index > userIndex);

        if (!appUsers.Any())
        {
            return NotFound(new
            {
                Message = "Users haven't been found",
            });
        }

        var users = UserHelper.GetUserDtos(appUsers);

        return Ok(new
        {
            Message = "Users have been gotten",
            Users = users,
        });
    }

    [HttpPost, Route("setClan")]
    public async Task<IActionResult> SetClan(string clanName)
    {
        var userId = User.GetUserId();
        await _userService.SetClan(userId, clanName);

        return Ok(new
        {
            Message = $"User {User.GetUserName()} has been succsessfully added to {clanName} clan",
        });
    }
}
