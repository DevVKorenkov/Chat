using Chat.Models;
using Chat.Repositories.Abstraction;
using Chat.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Chat.Services;

public class ClanService : IClanService
{
    private readonly IClanRepository _clanRepository;

    public ClanService(IClanRepository clanRepository)
    {
        _clanRepository = clanRepository;
    }

    public async Task AddAsync(Clan item)
    {
        await _clanRepository.AddAsync(item);
    }

    public bool Exists(string name) => _clanRepository.Exists(name);

    public async Task<IEnumerable<Clan>> GetAllAsync()
    {
        var clans = await _clanRepository.GetAllAsync(
            x => x.Include(c => c.ClanMembers));

        return clans;
    }

    public async Task<Clan> GetAsync(
        Expression<Func<Clan, bool>> filter = null)
    {
        var clan = await _clanRepository.GetAsync(
            filter,
            includes: x => x.Include(c => c.ClanMembers));

        return clan;
    }
}
