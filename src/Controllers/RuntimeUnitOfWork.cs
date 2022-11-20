using Data;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Controllers;

public abstract class RuntimeUnitOfWork
{
    public static UnitOfWork CreateUnitOfWork()
    {
        var builder = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: "TestDB")
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        var dataContext = new DataContext(builder.Options);
        return new UnitOfWork(dataContext);
    }
}