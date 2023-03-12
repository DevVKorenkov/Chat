using Chat.Models;
using Chat.Repositories.Abstraction;
using Chat.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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

    public async Task<IEnumerable<Clan>> GetAllAsync()
    {
        var clans = await _clanRepository.GetAllAsync(
            x => x.Include(c => c.ClanMembers)
            .ThenInclude(u => u.UserClan));

        return clans;
    }

    public async Task<Clan> GetAsync(
        Expression<Func<Clan, bool>> filter = null, 
        Func<IQueryable<Clan>, IIncludableQueryable<Clan, object>> includes = null)
    {
        var clan = await _clanRepository.GetAsync(filter, includes);

        return clan;
    }
}
