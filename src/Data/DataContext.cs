using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class DataContext : DbContext
{
    public DbSet<Message> Messages { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
}