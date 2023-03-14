using Chat.Models;
using Chat.Repositories.Abstraction;
using Chat.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Chat.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IClanRepository _clanRepository;

    public UserService(
        IUserRepository userRepository, 
        IClanRepository clanRepository)
    {
        _userRepository = userRepository;
        _clanRepository = clanRepository;
    }

    public async Task<IEnumerable<AppIdentityUser>> GetAllAsync(Expression<Func<AppIdentityUser, bool>> filter = null)
    {
        var users = await _userRepository.GetAllAsync(filter,
            includes: x => x.Include(u => u.UserClan));

        return users;
    }

    public Task<AppIdentityUser> GetAsync(Expression<Func<AppIdentityUser, bool>> filter = null)
    {
        var user = _userRepository.GetAsync(
            filter, 
            includes: x => x.Include(u => u.UserClan));

        return user;
    }

    public async Task SetClan(string userId, string clanName)
    {
        var clan = await _clanRepository.GetAsync(
            c => c.Name == clanName,
            includes: x => x.Include(c => c.ClanMembers));

        await _userRepository.SetClan(userId, clan);
    }

    public async Task SaveAsync()
    {
        await _userRepository.SaveAsync();
    }
}
