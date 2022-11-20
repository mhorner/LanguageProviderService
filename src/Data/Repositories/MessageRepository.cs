using Data.Interfaces;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class MessageRepository : GenericRepository<Message>, IMessageRepository
{
    private readonly DataContext context;
 
    public MessageRepository(DataContext dbContext) : base(dbContext)
    {
        this.context = dbContext;
    }
 
    public async Task<bool> DeleteMessage(Guid id)
    {
        return await Delete(id);
    }
 
    public async Task<IEnumerable<Message>> GetAllMessages(string culture)
    {
        return context.Set<Message>().AsNoTracking().Where(c => c.Culture == culture);
    }
 
    public async Task<Message> GetMessageById(Guid id)
    {
        return await GetById(id);
    }

    public IQueryable<Message> GetMessageByKey(string key)
    {
        return context.Set<Message>().AsNoTracking().Where(c => c.Key == key);

    }
 
    public async Task<bool> InsertMessage(Message message)
    {
        var existingMessage = GetMessageByKey(message.Key).FirstOrDefault();
        if (existingMessage == null)
        {
            return await Create(message);
        }

        return false;
    }
 
    public async Task<bool> UpdateMessage(Guid id, String value)
    {
        var inputMessage = await GetMessageById(id);
        if (inputMessage == null) return false;
        inputMessage.Value = value;
        
        return Update(inputMessage).Result;
    }
}