using DeliveryRequest.Models;
using System.Collections.Immutable;

namespace DeliveryRequest.DBService
{
    public class DbService: IDBInterface
    {
        private readonly IServiceProvider _services;
        public DbService(IServiceProvider services)
        {
            _services = services;
        }

        private ApplicationContext GetDbContext()
        {
            var scope = _services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        }

        public Order DeleteOrder(int id)
        {
            try
            {
                using var db = GetDbContext();
                var foundOrder = db.Orders.FirstOrDefault(a => a.Id == id);
                if (foundOrder != null)
                {
                    db.Orders.Remove(foundOrder);
                    db.SaveChanges();
                }
                else
                {
                    throw new Exception();
                }

                return new Order()
                {
                    Id = foundOrder.Id,
                    OutcomingCity = foundOrder.OutcomingCity,
                    OutcomingAddress = foundOrder.OutcomingAddress,
                    IncomingCity = foundOrder.IncomingCity,
                    IncomingAddress = foundOrder.IncomingAddress,
                    Weight = foundOrder.Weight,
                    PickupDate = foundOrder.PickupDate
                };

            }
            catch
            {
                throw new Exception("Ошибка удаления из БД");
            }
        }


        public int AddOrder(Order order)
        {
            try
            {
                using var db = GetDbContext();

                OrderToDB orderToDB = new OrderToDB()
                {
                    IncomingAddress = order.IncomingAddress,
                    IncomingCity = order.IncomingCity,
                    OutcomingAddress = order.OutcomingAddress,
                    OutcomingCity = order.OutcomingCity,
                    Weight = order.Weight,
                    PickupDate = order.PickupDate
                };
                db.Orders.Add(orderToDB);
                db.SaveChanges();
                return orderToDB.Id;
            }
            catch 
            {
                throw new Exception("Ошибка добавления в БД");
            }
        }
        public Order? GetOrder(int id)
        {
            using var db = GetDbContext();
            var foundOrder = db.Orders.FirstOrDefault(a => a.Id == id);
            if (foundOrder == null)
            {
                return null;
            }
            else
            {
                return new Order()
                {
                    Id = foundOrder.Id,
                    OutcomingCity = foundOrder.OutcomingCity,
                    OutcomingAddress = foundOrder.OutcomingAddress,
                    IncomingCity = foundOrder.IncomingCity,
                    IncomingAddress = foundOrder.IncomingAddress,
                    Weight = foundOrder.Weight,
                    PickupDate = foundOrder.PickupDate
                };
            }
        }

        public ImmutableList<Order> GetOrders()
        {
            using var db = GetDbContext();
            return db.Orders.Select(item => new Order()
            {
                Id = item.Id,
                OutcomingCity = item.OutcomingCity,
                OutcomingAddress = item.OutcomingAddress,
                IncomingCity = item.IncomingCity,
                IncomingAddress = item.IncomingAddress,
                Weight = item.Weight,
                PickupDate = item.PickupDate
            }).ToImmutableList();
        }
    }
}
