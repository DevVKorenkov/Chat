using Chat.DTOs;
using Chat.Models;

namespace Chat.Helpers;

public class UserHelper
{
    /// <summary>
    /// Transforms application user identity entity to user data transfer object
    /// </summary>
    /// <param name="appUser"></param>
    /// <returns></returns>
    public static UserDTO GetUserDto(AppIdentityUser appUser)
    {
        var user = new UserDTO 
        { 
            Id = appUser.Id,
            Name = appUser.UserName,
            UserClan = appUser.UserClan,
        };

        return user;
    }

    /// <summary>
    /// Transforms a bunch of user identity entitys to a bunch of user data transfer objects
    /// </summary>
    /// <param name="appUsers"></param>
    /// <returns></returns>
    public static IEnumerable<UserDTO> GetUserDtos(IEnumerable<AppIdentityUser> appUsers)
    {
        var users = appUsers.Select(u =>
        {
            return new UserDTO
            {
                Id = u.Id,
                Name = u.UserName,
                UserClan = u.UserClan,
            };
        });

        return users;
    }
}
