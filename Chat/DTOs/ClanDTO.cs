namespace Chat.DTOs;

public class ClanDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<UserDTO> ClanMembers { get; set; }
}
