using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Data;
using Api.Dtos;
using Api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class NotificationService : INotificationService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public NotificationService(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> AddNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<NotificationDto> GetNotification(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            return _mapper.Map<NotificationDto>(notification);
        }

        public async Task<IEnumerable<NotificationDto>> GetNotifications(string userId)
        {
            var notifications = await _context.Notifications.Where(x => x.RecipientId == userId)
            .Include(x => x.Sender).ThenInclude(x => x.Image)
            .OrderByDescending(x => x.TimeNotification).ToListAsync();
            return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
        }

        public async Task<object> GetNotificationCount(string userId)
        {
            var count = await _context.Notifications.Where(x => x.RecipientId == userId && !x.IsRead).CountAsync();
            return new { NotificationCount = count };
        }

        public async Task MarkAsRead(string userId)
        {
            var notifications = await _context.Notifications.Where(x => x.RecipientId == userId && !x.IsRead).ToListAsync();
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                _context.Entry(notification).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNotifications(string userId)
        {
            var notifications = await _context.Notifications.Where(x => x.RecipientId == userId).ToListAsync();
            foreach (var notification in notifications)
            {
                _context.Notifications.Remove(notification);
            }
            await _context.SaveChangesAsync();
        }
    }
}