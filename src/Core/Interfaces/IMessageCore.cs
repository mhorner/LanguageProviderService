using EntitiesUi;

namespace Core.Interfaces;

public interface IMessageCore: IEntityMapperCore
{
    Task<List<MessageUi>> GetMessages();
    Task<MessageUi> GetMessageById(Guid id);
    // Task<MessageUi> GetMessageByKey(string key);
    Task<bool> InsertMessage(MessageUi objMessageUI);
    Task<bool> UpdateMessage(MessageUi objMessageUI);
    Task<bool> DeleteMessage(Guid id);
}