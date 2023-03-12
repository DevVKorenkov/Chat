using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Models;

public class AppIdentityUser : IdentityUser
{
    public int? UserClanId { get; set; }
    [ForeignKey("UserClanId")]
    public Clan? UserClan { get; set; }
}
