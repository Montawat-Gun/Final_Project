using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Models;

namespace Api.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetNotifications(string userId);
        Task<NotificationDto> GetNotification(int id);
        Task<bool> AddNotification(Notification notification);
        Task<object> GetNotificationCount(string userId);
        Task MarkAsRead(string userId);
        Task DeleteNotifications(string userId);
    }
}