﻿using Chat.DataContext;
using Chat.DTOs;
using Chat.Models;
using Chat.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace Chat.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDataContext _appDataContext;
    private readonly DbSet<AppIdentityUser> _dbSet;
    private readonly IMemoryCache _memoryCache;

    public UserRepository(AppDataContext appDataContext, IMemoryCache memoryCache)
    {
        _appDataContext = appDataContext;
        _dbSet = _appDataContext.Set<AppIdentityUser>();
        _memoryCache = memoryCache;
    }

    public async Task<IEnumerable<AppIdentityUser>> GetAllAsync(
        Expression<Func<AppIdentityUser, bool>> filter = null,
        Func<IQueryable<AppIdentityUser>, IIncludableQueryable<AppIdentityUser, object>> includes = null)
    {
        IQueryable<AppIdentityUser> query = _dbSet;

        if(includes != null)
        {
            query = includes(query);
        }

        if(filter != null)
        {
            query = query.Where(filter);
        }

        var users = await query.ToListAsync();

        return users;
    }

    public async Task<AppIdentityUser> GetAsync(
        Expression<Func<AppIdentityUser, bool>> filter = null, 
        Func<IQueryable<AppIdentityUser>, IIncludableQueryable<AppIdentityUser, object>> includes = null)
    {
        IQueryable<AppIdentityUser> query = _dbSet;

        if (includes != null)
        {
            query = includes(query);
        }

        var user = await query.FirstOrDefaultAsync(filter);

        return user;
    }

    public async Task SetClan(string userId, Clan clan)
    {
        var user = await GetAsync(
            u => u.Id == userId, 
            includes: x => x.Include(u => u.UserClan));

        user.UserClan = clan;

        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _appDataContext.SaveChangesAsync();
    }
}
