using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Chat.Models;

public class Clan
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public ICollection<AppIdentityUser> ClanMembers { get; set; }
}
