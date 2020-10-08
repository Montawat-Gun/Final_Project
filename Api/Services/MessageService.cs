using System;
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
    public class MessageService : IMessageService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MessageService(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;

        }
        public async Task<bool> AddMessage(Message message)
        {
            message.TimeSend = DateTime.Now;
            _context.Messages.Add(message);
            return await _context.SaveChangesAsync() > 0;
        }

        public Task DeleteMessage(Message message)
        {
            throw new System.NotImplementedException();
        }

        public async Task<MessageDto> GetMessage(int id)
        {
            var message = await _context.Messages.Where(x => x.MessageId == id)
            .Include(s => s.Sender).ThenInclude(i => i.Image)
            .Include(r => r.Recipient).ThenInclude(i => i.Image)
            .FirstOrDefaultAsync();
            return _mapper.Map<MessageDto>(message);
        }

        public async Task<IEnumerable<MessageDto>> GetMessages(string currentUserId, string otherUserId)
        {
            var messages = await _context.Messages.Where(u =>
            u.SenderId == currentUserId && u.RecipientId == otherUserId ||
            u.SenderId == otherUserId && u.RecipientId == currentUserId)
            .Include(s => s.Sender).ThenInclude(i => i.Image)
            .ToListAsync();
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<IEnumerable<UserToList>> GetUserContacts(string userId)
        {
            var contactsRecipient = await _context.Messages.Where(u => u.SenderId == userId)
            .Include(r => r.Recipient).ThenInclude(s => s.Image)
            .Select(r => new { users = r.Recipient, timeSend = r.TimeSend }).ToListAsync();

            var contactsSender = await _context.Messages.Where(u => u.RecipientId == userId)
            .Include(s => s.Sender).ThenInclude(s => s.Image)
            .Select(s => new { users = s.Sender, timeSend = s.TimeSend }).ToListAsync();

            var contacts = contactsRecipient.Concat(contactsSender).OrderByDescending(x => x.timeSend)
            .Select(x => x.users).Distinct();
            return _mapper.Map<IEnumerable<UserToList>>(contacts);
        }
    }
}