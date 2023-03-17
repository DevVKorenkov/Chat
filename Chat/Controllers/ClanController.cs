using Chat.DTOs;
using Chat.Helpers;
using Chat.Models;
using Chat.Models.Responses;
using Chat.Repositories.Abstraction;
using Chat.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers;

[Authorize]
[ApiController, Route("[controller]")]
public class ClanController : Controller
{
    private readonly IClanService _clanService;
    private readonly IMessageCacheRepository _messageCacheRepository;

    public ClanController(IClanService clanService, IMessageCacheRepository messageCacheRepository)
    {
        _clanService = clanService;
        _messageCacheRepository = messageCacheRepository;
    }

    [HttpGet, Route("addClan")]
    public async Task<IActionResult> AddClan(string name)
    {
        if (_clanService.Exists(name))
        {
            return BadRequest(new ClanResponse
            {
                Message = $@"""{name}"" Clan is already exsits",
            });
        }

        await _clanService.AddAsync(new Clan
        {
            Name = name,
        });

        var clan = await _clanService.GetAsync(c => c.Name == name);

        return Ok(new ClanResponse
        {
            Message = $@"""{name}"" clan has been successfully created",
            Clan = new ClanDTO
            {
                Id = clan.Id,
                Name = clan.Name,
                ClanMembers = UserHelper.GetUserDtos(clan.ClanMembers),
            }
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

    [HttpPost, Route("setMessage")]
    public async Task<IActionResult> SetMessage(string clanName, Message message)
    {
        await _messageCacheRepository.AddAsync(message, clanName);
        return Ok();
    }

    [HttpGet, Route("getMessages")]
    public async Task<IActionResult> GetMessages(string clanName)
    {
        var messages = await _messageCacheRepository.GetAllAsync(clanName);
        return Ok(new
        {
            Messages = messages
        });
    }
}
