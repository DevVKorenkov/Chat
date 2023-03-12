using Chat.Models;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Chat.Services.Abstractions;

public interface IClanService
{
    Task<IEnumerable<Clan>> GetAllAsync();
    Task<Clan> GetAsync(
        Expression<Func<Clan, bool>> filter = null,
        Func<IQueryable<Clan>, IIncludableQueryable<Clan, object>> includes = null);
    Task AddAsync(Clan item);
}
