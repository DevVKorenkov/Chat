using Chat.DTOs;
using Chat.Models;

namespace Chat.Helpers;

public class UserHelper
{
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
