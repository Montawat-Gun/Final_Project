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
            message.TimeSend = DateTime.UtcNow;
            _context.Messages.Add(message);
            return await _context.SaveChangesAsync() > 0;
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
            u.SenderId == currentUserId && u.RecipientId == otherUserId && !u.IsSenderDelete ||
            u.SenderId == otherUserId && u.RecipientId == currentUserId && !u.IsRecipientDelete)
            .Include(s => s.Sender).ThenInclude(i => i.Image)
            .OrderBy(x => x.TimeSend).ToListAsync();
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<IEnumerable<UserToContact>> GetUserContacts(string userId)
        {
            var contactsRecipient = await _context.Messages.Where(u => u.SenderId == userId && !u.IsSenderDelete)
            .Include(r => r.Recipient).ThenInclude(s => s.Image)
            .Select(r => new { user = r.Recipient, timeSend = r.TimeSend, r.IsRead }).ToListAsync();

            var contactsSender = await _context.Messages.Where(u => u.RecipientId == userId && !u.IsRecipientDelete)
            .Include(s => s.Sender).ThenInclude(s => s.Image)
            .Select(s => new { user = s.Sender, timeSend = s.TimeSend, s.IsRead }).ToListAsync();

            var messages = contactsRecipient.Concat(contactsSender).OrderByDescending(x => x.timeSend);
            var contacts = _mapper.Map<IEnumerable<UserToContact>>(messages.Select(u => u.user).Distinct().ToList());

            foreach (var contact in contacts)
            {
                contact.MessageUnReadCount = contactsSender.Where(x => x.user.Id == contact.Id && x.IsRead == false).Count();
            }

            return _mapper.Map<IEnumerable<UserToContact>>(contacts);
        }

        public async Task<bool> MarkAsRead(string currentUserId, string otherUserId)
        {
            var messages = await _context.Messages.Where(u =>
            u.SenderId == otherUserId && u.RecipientId == currentUserId).ToListAsync();
            foreach (var message in messages)
            {
                message.IsRead = true;
                _context.Entry(message).State = EntityState.Modified;
            }
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task DeleteMessage(int id, string currentUserId)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message.RecipientId == currentUserId)
            {
                message.IsRecipientDelete = true;
            }
            else if (message.SenderId == currentUserId)
            {
                message.IsSenderDelete = true;
            }
            if (message.IsSenderDelete && message.IsRecipientDelete)
            {
                _context.Messages.Remove(message);
            }
            else
            {
                _context.Entry(message).State = EntityState.Modified;
            }
        }

        public async Task<bool> DeleteMessages(string currentUserId, string otherUserId)
        {
            var messages = await _context.Messages.
            Where(x => x.SenderId == currentUserId && x.RecipientId == otherUserId ||
            x.SenderId == otherUserId && x.RecipientId == currentUserId).ToListAsync();
            foreach (var message in messages)
            {
                await DeleteMessage(message.MessageId, currentUserId);
            }
            return await SaveChangeAsync();
        }

        public async Task<bool> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}