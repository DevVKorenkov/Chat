using Chat.Models;
using System.Linq.Expressions;

namespace Chat.Services.Abstractions;

public interface IUserService
{
    Task<IEnumerable<AppIdentityUser>> GetAllAsync(Expression<Func<AppIdentityUser, bool>> filter = null);

    Task<AppIdentityUser> GetAsync(
        Expression<Func<AppIdentityUser, bool>> filter = null);

    Task SetClan(string userId, string clanName);

    Task SaveAsync();
}
