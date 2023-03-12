using System.ComponentModel.DataAnnotations;

namespace Chat.Models;

public class SignupModel
{
    [Required, MinLength(3)]
    public string Name { get; set; }
    [Required, MinLength(6)]
    public string Password { get; set; }
}
