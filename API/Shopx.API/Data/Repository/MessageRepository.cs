using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Helper;
using Shopx.API.Interfaces;

namespace Shopx.API.Data.Repository
{
    public class MessageRepository : IMessagesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MessageRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task AddMessage(Message message)
        {
            /// as i am sending for this user there should be no unread messages he sent to me
            /// this validation if a user send message from api instead of using the client app
            /// if user use client there should be no such case

            var unread = _context.Messages
                .Where(m => m.RecipenetUsername == message.SenderUsername
                    && m.SenderUsername == message.RecipenetUsername
                    && m.ProductId == message.ProductId
                ).ToList();

            foreach (var item in  unread)
            {
                item.DateRead = DateTime.UtcNow;
            }

            ////since this is the last message , set the previous last message to false 
            var preLastMessage = await
                GetLastMessage(message.SenderUsername, message.RecipenetUsername, message.ProductId);

            if (preLastMessage != null) 
                preLastMessage.LastMessage = false;

            _context.Messages.Add(message);
        }

        public async Task<Message> GetMessageAsync(int Id)
        {
            return await _context.Messages.FindAsync(Id);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageView(string username)
        {
            ////we getting last chat that this user on it and this user not delete it as well

            var query = _context.Messages.OrderByDescending(x => x.MessageSent).AsQueryable();

            query = query.Where(m => 
                ((m.RecipenetUsername == username && !m.RecipenetDeleted)

                || (m.SenderUsername == username && !m.SenderDeleted)) 
                
                && m.LastMessage
            );

            ///any message view after 250 will be exist but will not sent because
            ///it's not important messages and that will enhance performance if there are massive result
            query = query.Take(250);

            var messages = await query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider).ToListAsync();

            ///for each of those message set the unread messages

            foreach(var message in messages)
            {
                var unread = await _context.Messages
                    .Where(m => 
                    m.RecipenetUsername == username 
                    && m.SenderUsername == message.SenderUsername 
                    && m.DateRead == null
                    && m.ProductId == message.ProductId).ToListAsync();

                message.UnreadCount = unread.Count;
            }

            return messages;
        }


        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentName, string recipeName, int id)
        {
            var messages = _context.Messages.
                Where
                (
                    m => ((m.SenderUsername == currentName && m.RecipenetUsername == recipeName) ||
                    (m.SenderUsername == recipeName && m.RecipenetUsername == currentName))

                    && m.ProductId == id
                )
                .OrderBy(d => d.MessageSent).AsQueryable();

            var unread = _context.Messages.
                Where
                (
                    m => m.DateRead == null 
                    && m.RecipenetUsername == currentName 
                    && m.SenderUsername == recipeName
                    && m.ProductId == id).ToList();

            if (unread.Any())
            {
                foreach (var message in unread)
                {
                    message.DateRead = DateTime.UtcNow;
                }
                await _context.SaveChangesAsync();
            }

            ///any message view after 250 will be exist but will not sent because
            ///it's not important messages and that will enhance performance if there are massive result
            messages = messages.Take(250);

            return await messages.ProjectTo<MessageDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Message> GetLastMessage(string currentName, string recipeName, int productId)
        {
            return await _context.Messages
                .Where(m => (
                    (m.SenderUsername == currentName && m.RecipenetUsername == recipeName) || (m.SenderUsername == recipeName && m.RecipenetUsername == currentName)) 
                    && m.LastMessage  && m.ProductId == productId
                 )
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetMessagesNotificationCount(string username)
        {
            return await _context.Messages
                .Where(m => m.RecipenetUsername == username && m.DateRead == null)
                .CountAsync();
        }

        public async Task<IEnumerable<MessageDto>> GetProductMessages(int productId, string sellerName)
        {
            var messages =  await _context.Messages
                .Where(m => m.ProductId == productId && m.LastMessage)
                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            ///for each of those message set the unread messages

            foreach (var message in messages)
            {
                var unread = await _context.Messages
                    .Where(m =>
                    m.RecipenetUsername == sellerName
                    && m.SenderUsername == message.SenderUsername
                    && m.DateRead == null
                    && m.ProductId == message.ProductId).ToListAsync();

                message.UnreadCount = unread.Count;
            }

            return messages;
        }
    }
}
