using Data.Interfaces;
using Data.Repositories;

namespace Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext context;

    public UnitOfWork(DataContext context)
    {
        this.context = context;
        Messages = new MessageRepository(this.context);
    }

    public IMessageRepository Messages { get; private set; }
    public async Task Save()
    {
        await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}