using Chat.DTOs;

namespace Chat.Models;

public class AuthResponse
{
    public Responses Response { get; set; }
    public string Message { get; set; }
    public string Token { get; set; }
    public UserDTO User { get; set; }
}
