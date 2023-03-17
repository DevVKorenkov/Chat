using Chat.Models;

namespace Chat.Repositories.Abstraction
{
    public interface IMessageCacheRepository
    {
        /// <summary>
        /// Provides all messages in the cache
        /// </summary>
        /// <param name="clanMessageStoreName"></param>
        /// <returns></returns>
        Task<IEnumerable<Message>> GetAllAsync(string clanMessageStoreName);
        /// <summary>
        /// Adds new message in the cache
        /// </summary>
        /// <param name="message"></param>
        /// <param name="clanMessageStoreName"></param>
        /// <returns></returns>
        Task AddAsync(Message message, string clanMessageStoreName);
    }
}
