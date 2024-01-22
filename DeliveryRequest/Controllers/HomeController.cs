using DeliveryRequest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DeliveryRequest.DBService;

namespace DeliveryRequest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbService db;
        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            db = new DbService(context);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var orders = await db.getAllOrdersFromDb();
                var sortedOrders = orders.OrderByDescending(item => item.Id);
                List<OrderToView> listOfOrders = new List<OrderToView>();
                
                foreach (var item in sortedOrders)
                {
                    listOfOrders.Add(new OrderToView
                    {
                        Id = item.Id,
                        OutcomingCity = item.OutcomingCity,
                        OutcomingAddress = item.OutcomingAddress,
                        IncomingCity = item.IncomingCity,
                        IncomingAddress = item.IncomingAddress,
                        Weight = item.Weight,
                        PickupDate = item.PickupDate,
                        isNew = false
                    });
                }
                return View(listOfOrders);
            }
            catch
            { 
                return RedirectToAction("Error");
            }
        }

        public IActionResult AddOrderView()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderToView order)
        {
            if (ModelState.IsValid)
            {
                var dbOrderModel = new Order 
                {
                    IncomingAddress = order.IncomingAddress,
                    IncomingCity = order.IncomingCity,
                    OutcomingAddress = order.OutcomingAddress,
                    OutcomingCity = order.OutcomingCity,
                    Weight = order.Weight,
                    PickupDate = order.PickupDate
                };
                try
                {
                    if (await db.addOrderToDb(dbOrderModel) == true)
                    {
                        return RedirectToAction("OrderView", "Home", new { id = dbOrderModel.Id, isNew = true });
                    }
                    else
                    {
                        return RedirectToAction("Error");
                    }
                }
                catch
                {
                    return RedirectToAction("Error");
                }
            }
            else
            {
                return RedirectToAction("AddOrderView");
            }
        }

        public IActionResult OrderView(int id, bool isNew)
        {
            if (id > 0)
            {
                try
                {
                    var order = db.getFromDb(id);
                    if (order != null)
                    {
                        OrderToView orderView = new OrderToView
                        {
                            Id = order.Id,
                            OutcomingCity = order.OutcomingCity,
                            OutcomingAddress = order.OutcomingAddress,
                            IncomingCity = order.IncomingCity,
                            IncomingAddress = order.IncomingAddress,
                            Weight = order.Weight,
                            PickupDate = order.PickupDate,
                            isNew = isNew
                        };
                        return View(orderView);
                    }
                    else
                    {
                        return RedirectToAction("Error");
                    }
                }
                catch 
                {
                    return RedirectToAction("Error");
                }
            }
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
