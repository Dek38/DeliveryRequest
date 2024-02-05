using System.Collections.Immutable;

namespace DeliveryRequest.DBService
{
    public interface IDBInterface
    {
        public Order? GetOrder(int id);
        public ImmutableList<Order> GetOrders();
        public int AddOrder(Order order);
        public Order DeleteOrder(int id);
    }

    public record Order
    {
        public int Id { get; init; }
        public string? OutcomingCity { get; init; }
        public string? OutcomingAddress { get; init; }
        public string? IncomingCity { get; init; }
        public string? IncomingAddress { get; init; }
        public decimal Weight { get; init; }
        public DateTime? PickupDate { get; init; }
    }
}
