using Shopx.API.Entities;

namespace Shopx.API.Interfaces
{
    public interface INotificationRepository
    {
        void SendNotification(Notification notification);
        Task <List<Notification>> GetAllNotifications(string userId);
        Task<Notification> GetNotification (int notificationId);
        Task<bool> SaveChanges();
    }
}
