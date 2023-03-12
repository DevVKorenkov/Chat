using Chat.Models;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Chat.Repositories.Abstraction;

public interface IUserRepository
{
    Task<IEnumerable<AppIdentityUser>> GetAllAsync(
        Func<IQueryable<AppIdentityUser>, 
            IIncludableQueryable<AppIdentityUser, object>> includes = null);

    Task<AppIdentityUser> GetAsync(
        Expression<Func<AppIdentityUser, bool>> filter = null,
        Func<IQueryable<AppIdentityUser>, IIncludableQueryable<AppIdentityUser, object>> includes = null);

    Task SetClan(string userId, Clan clan);

    Task SaveAsync();
}
