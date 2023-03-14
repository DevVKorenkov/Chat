using Chat.DTOs;

namespace Chat.Models.Responses;

public class AuthResponse : Response
{
    public ResponseStatus Response { get; set; }
    public string Token { get; set; }
    public UserDTO User { get; set; }
}
