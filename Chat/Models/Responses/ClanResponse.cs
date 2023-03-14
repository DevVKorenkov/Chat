using Chat.DTOs;

namespace Chat.Models.Responses;

public class ClanResponse
{
    public string Message { get; set; }
    public ClanDTO Clan { get; set; }
}
