using System.ComponentModel.DataAnnotations;

namespace Chat.Models;

public class Message
{
    public string UserName { get; set; }
    public string UserMessage { get; set; }
}
