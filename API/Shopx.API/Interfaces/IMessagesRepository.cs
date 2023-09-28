using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Helper;

namespace Shopx.API.Interfaces
{
    public interface IMessagesRepository
    {
        Task AddMessage(Message message);
        Task<Message> GetMessageAsync(int Id);
        Task<IEnumerable<MessageDto>> GetMessageView(string username);
        Task<IEnumerable<MessageDto>> GetProductMessages(int productId, string sellerName);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentName, string recipeName, int id);
        Task<Message> GetLastMessage(string currentName, string recipeName, int productId);
        Task<int> GetMessagesNotificationCount(string username);

        Task<bool> SaveAllAsync();
    }
}
