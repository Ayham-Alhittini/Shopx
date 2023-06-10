using Microsoft.EntityFrameworkCore;
using Shopx.API.Entities;
using Shopx.API.Interfaces;

namespace Shopx.API.Data.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DataContext _context;
        public NotificationRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Notification>> GetAllNotifications(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderBy(n => n.Read)
                .ThenBy(n => n.SendDate)
                .ToListAsync();
        }

        public async Task<Notification> GetNotification(int notificationId)
        {
            return await _context.Notifications.FindAsync(notificationId);
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void SendNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
        }
    }
}
