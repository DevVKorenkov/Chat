using Chat.DataContext;
using Chat.Models;
using Chat.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Chat.Repositories;

public class ClanRepository : IClanRepository
{
	private readonly AppDataContext _appDataContext;
    private readonly DbSet<Clan> _dbSet;

	public ClanRepository(AppDataContext appDataContext)
	{
        _appDataContext = appDataContext;
        _dbSet = _appDataContext.Set<Clan>();
	}

    public async Task<IEnumerable<Clan>> GetAllAsync(Func<IQueryable<Clan>, IIncludableQueryable<Clan, object>> includes = null)
    {
        IQueryable<Clan> query = _dbSet;

        if(includes != null)
        {
            query = includes(query);
        }

        var clans = await query.ToListAsync(); 

        return clans;
    }

    public async Task<Clan> GetAsync(
        Expression<Func<Clan, bool>> filter = null, 
        Func<IQueryable<Clan>, IIncludableQueryable<Clan, object>> includes = null)
    {
        IQueryable<Clan> query = _dbSet;
        Clan clan = null;

        if (includes != null)
        {
            query = includes(query);
        }

        if(filter != null)
        {
            clan = await query.FirstOrDefaultAsync(filter);
        }

        clan = clan ?? await query.FirstOrDefaultAsync();

        return clan;
    }

    public async Task AddAsync(Clan item)
    {
        await _dbSet.AddAsync(item);
        await SaveAsync();
    }

    public bool Exists(string name)
    {
        var result = _dbSet.Any(x => x.Name == name);

        return result;
    }

    public async Task SaveAsync()
    {
        await _appDataContext.SaveChangesAsync();
    }
}
