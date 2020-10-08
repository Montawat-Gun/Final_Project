using System.Threading.Tasks;
using System.Collections.Generic;
using Api.Dtos;
using Api.Models;

namespace Api.Services
{
    public interface IMessageService
    {
        Task<bool> AddMessage(Message message);
        Task DeleteMessage(Message message);
        Task<MessageDto> GetMessage(int id);
        Task<IEnumerable<UserToList>> GetUserContacts(string userId);
        Task<IEnumerable<MessageDto>> GetMessages(string senderId, string recipientId);
    }
}