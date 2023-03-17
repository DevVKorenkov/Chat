using Chat.Config;
using Chat.Models;
using Chat.Repositories.Abstraction;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace Chat.Repositories
{
    public class MessageCacheRepository : IMessageCacheRepository
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _db;
        private readonly int _messagesCount;

        public MessageCacheRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _db = _connectionMultiplexer.GetDatabase();
            int.TryParse(SettingsManager.AppSettings["messagecount"], out _messagesCount);
        }

        public async Task AddAsync(Message message, string clanMessageStoreName)
        {
            List<Message> messages = null;
            var messagesString = await _db.StringGetAsync(clanMessageStoreName);

            //If there are no messages in the list, creates a new list with the first one message inside.
            if (string.IsNullOrWhiteSpace(messagesString))
            {
                messages = new List<Message> { message };
                var messagesJsonString = JsonSerializer.Serialize(messages);
                await _db.StringSetAsync(clanMessageStoreName, messagesJsonString);
                return;
            }

            messages = JsonSerializer.Deserialize<List<Message>>(messagesString);
            messages?.Add(message);

            if (messages?.Count > _messagesCount)
            {
                messages.RemoveAt(0);
            }

            messagesString = JsonSerializer.Serialize(messages);
            await _db.StringSetAsync(clanMessageStoreName, messagesString);
        }

        public async Task<IEnumerable<Message>> GetAllAsync(string clanMessageStoreName)
        {
            var sringMessages = await _db.StringGetAsync(clanMessageStoreName);
            List<Message> messages = null;
            if (!string.IsNullOrWhiteSpace(sringMessages))
            {
                messages = JsonSerializer.Deserialize<List<Message>>(sringMessages);
            }
            return messages;
        }
    }
}
