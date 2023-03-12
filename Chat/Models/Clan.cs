using System.ComponentModel.DataAnnotations;

namespace Chat.Models;

public class Clan
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<AppIdentityUser> ClanMembers { get; set; }
}
