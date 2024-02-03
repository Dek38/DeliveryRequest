using Microsoft.EntityFrameworkCore;

namespace DeliveryRequest.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<OrderToDB> Orders { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
