using System.ComponentModel.DataAnnotations;

namespace Chat.Models;

public class Message
{
    [Key]
    public int Id { get; set; }
    public string UserName { get; set; }
    public string UserMessage { get; set; }
}
