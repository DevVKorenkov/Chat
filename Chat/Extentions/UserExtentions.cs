using System.Security.Claims;

namespace Chat.Extentions;

public static class UserExtentions
{
    /// <summary>
    /// Provides logged in user id
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static string GetUserId(this ClaimsPrincipal user) => user.Claims.FirstOrDefault(
                u => u.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value;
    /// <summary>
    /// Provides logged in user name
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static string GetUserName(this ClaimsPrincipal user) =>
            user.Claims.FirstOrDefault(
                x => x.Type.Equals(ClaimTypes.Name, StringComparison.OrdinalIgnoreCase))?.Value;
}
