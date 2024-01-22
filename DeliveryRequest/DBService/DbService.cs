using DeliveryRequest.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryRequest.DBService
{
    public class DbService
    {
        private readonly ApplicationContext db;
        public DbService(ApplicationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            else
            {
                db = context;
            }
        }
        public async Task<bool> addOrderToDb(Order order)
        {
            try
            {
                db.Orders.Add(order);
                await db.SaveChangesAsync();
            }
            catch 
            {
                return false;
            }
            return true;
        }
        public Order? getFromDb(int id)
        {
            return db.Orders.FirstOrDefault(a => a.Id == id);
        }

        public async Task<List<Order>> getAllOrdersFromDb()
        {
            return await db.Orders.ToListAsync();
        }

    }
}
