using Microsoft.EntityFrameworkCore;

namespace OrderApi.Infrastructure.Persistence
{
    public class EntityEventContext : DbContext
    {
        public EntityEventContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<EntityEvent> EntityEvent { get; set; }
    }
}