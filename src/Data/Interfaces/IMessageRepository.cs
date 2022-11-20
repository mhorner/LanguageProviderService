using Entities;

namespace Data.Interfaces;

public interface IMessageRepository
{
    Task<bool> InsertMessage(Message message);
    
    Task<bool> DeleteMessage(Guid id);

    /// <summary>
    ///  Retrieve all messages saved.
    /// </summary>
    /// <returns>A list containing all messages.</returns>
    Task<IEnumerable<Message>> GetAllMessages(string culture);
    
    Task<bool> UpdateMessage(Guid id, string value);
    // Task<IEnumerable<Message>> GetAllMessagesByCulture(string culture = "en-US");
    Task<Message> GetMessageById(Guid id);

    /// <summary>
    /// Performs a lookup of messages with the key.
    /// </summary>
    /// <param name="key">The message with the matching key.</param>
    /// <returns>A message if found.</returns>
    IQueryable<Message> GetMessageByKey(string key);
}