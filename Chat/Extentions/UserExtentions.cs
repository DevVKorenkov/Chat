using System.Security.Claims;

namespace Chat.Extentions;

public static class UserExtentions
{
    public static string GetUserId(this ClaimsPrincipal user) => user.Claims.FirstOrDefault(
                u => u.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value;

    public static string GetUserName(this ClaimsPrincipal user) =>
            user.Claims.FirstOrDefault(
                x => x.Type.Equals(ClaimTypes.Name, StringComparison.OrdinalIgnoreCase))?.Value;
}
