using Chat.Models;

namespace Chat.Repositories.Abstraction
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetSeveralAsync(int count);
        Task AddAsync(Message message, string clanMessageStoreName);
    }
}
