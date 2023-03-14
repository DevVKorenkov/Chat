using Chat.Models;

namespace Chat.DTOs;

public class UserDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Clan UserClan { get; set; }
}
