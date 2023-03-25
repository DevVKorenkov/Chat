using Chat.Models;

namespace Chat.Services.Abstractions
{
    public interface IJwTokenService : ITokenService<AppIdentityUser, string>
    {
    }
}
