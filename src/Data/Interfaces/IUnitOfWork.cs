using Data.Interfaces;

namespace Data.Interfaces;

public interface IUnitOfWork
{
    IMessageRepository Messages { get; }
    Task Save();
    void Dispose();
}