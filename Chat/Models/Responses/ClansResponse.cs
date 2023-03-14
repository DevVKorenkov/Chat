using Chat.DTOs;

namespace Chat.Models.Responses
{
    public class ClansResponse : Response
    {
        public IEnumerable<ClanDTO> Clans { get; set; }
    }
}
