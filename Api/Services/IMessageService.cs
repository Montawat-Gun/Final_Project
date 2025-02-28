using System.Threading.Tasks;
using System.Collections.Generic;
using Api.Dtos;
using Api.Models;

namespace Api.Services
{
    public interface IMessageService
    {
        Task<bool> AddMessage(Message message);
        Task<MessageDto> GetMessage(int id);
        Task<IEnumerable<UserToContact>> GetUserContacts(string userId);
        Task<IEnumerable<MessageDto>> GetMessages(string senderId, string recipientId);
        Task<bool> MarkAsRead(string currentUserId, string otherUserId);
        Task DeleteMessage(int id, string currentUserId);
        Task<bool> DeleteMessages(string currentUserId, string otherUserId);
    }
}