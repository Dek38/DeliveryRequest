using DeliveryRequest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DeliveryRequest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext db;
        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await db.Orders.ToListAsync());
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
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Orders.Add(order);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
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

        public IActionResult OrderView(int id)
        {
            if (id > 0)
            {
                try
                {
                    var order = db.Orders.FirstOrDefault(a => a.Id == id);
                    return View(order);
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
