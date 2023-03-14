using Chat.Models;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Chat.Repositories.Abstraction;

public interface IClanRepository
{
    Task<IEnumerable<Clan>> GetAllAsync(Func<IQueryable<Clan>, IIncludableQueryable<Clan, object>> includes = null);
    Task<Clan> GetAsync(
        Expression<Func<Clan, bool>> filter = null, 
        Func<IQueryable<Clan>, IIncludableQueryable<Clan, object>> includes = null);
    Task AddAsync(Clan item);
    bool Exists(string name);
    Task SaveAsync();
}
