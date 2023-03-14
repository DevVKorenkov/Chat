using Chat.DTOs;
using Chat.Helpers;
using Chat.Models;
using Chat.Models.Responses;
using Chat.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Chat.Controllers;

[Authorize]
[ApiController, Route("[controller]")]
public class ClanController : Controller
{
    private readonly IClanService _clanService;

    public ClanController(IClanService clanService)
    {
        _clanService = clanService;
    }

    [HttpPost, Route("addClan")]
    public async Task<IActionResult> AddClan(string clanName)
    {
        if (_clanService.Exists(clanName))
        {
            return BadRequest(new ClanResponse
            {
                Message = $@"""{clanName}"" Clan is already exsits",
            });
        }

        await _clanService.AddAsync(new Clan
        {
            Name = clanName,
        });

        return Ok(new ClanResponse
        {
            Message = $@"""{clanName}"" clan has been successfully created",
        });
    }

    [HttpGet, Route("getAllClans")]
    public async Task<IActionResult> GetAllClans()
    {
        var clans = await _clanService.GetAllAsync();

        if (!clans.Any())
        {
            return NotFound(new ClansResponse
            {
                Message = "There aren't any clans",
            });
        }

        var clansDto = clans.Select(c =>
        {
            return new ClanDTO
            {
                Id = c.Id,
                Name = c.Name,
                ClanMembers = UserHelper.GetUserDtos(c.ClanMembers),
            };
        });

        return Ok(new ClansResponse
        {
            Message = "Clans have been found",
            Clans = clansDto,
        });
    }

    [HttpPost, Route("getClan")]
    public async Task<IActionResult> GetClan(string name)
    {
        var clan = await _clanService.GetAsync(c => c.Name == name);

        if (clan == null)
        {
            return NotFound(new ClansResponse
            {
                Message = $"There isn't clans named {name}",
            });
        }

        var clansDto = new ClanDTO
        {
            Id = clan.Id,
            Name = clan.Name,
            ClanMembers = UserHelper.GetUserDtos(clan.ClanMembers),
        };

        return Ok(new ClanResponse
        {
            Message = $"{name} clan has been found",
            Clan = clansDto,
        });
    }
}
