using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStore.Models;
using TechStore.Repositories;

namespace TechStore.Controllers
{
    [Authorize]
    public class UserOrderController : Controller
    {
        private readonly IUserOrderRepository _userOrderRepo;

        public UserOrderController(IUserOrderRepository userOrderRepo)
        {
            _userOrderRepo = userOrderRepo;
        }
        //public async Task<IActionResult> UserOrders()
        //{
        //    var orders = await _userOrderRepo.UserOrders();
        //    return View(orders);
        //}
        public async Task<IActionResult> UserOrders(int page = 1)
        {
            int pageSize = 5; // Numri i porosive për faqe
            var orders = await _userOrderRepo.UserOrders();

            if (orders == null || !orders.Any())
            {
                ViewBag.TotalPages = 0;
                ViewBag.CurrentPage = page;
                return View(new List<Order>()); // Kthe një listë bosh te View
            }

            // Paginimi
            var totalOrders = orders.Count();
            var totalPages = (int)Math.Ceiling((double)totalOrders / pageSize);

            var ordersToShow = orders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Shto të dhënat e paginimit në ViewBag
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            // Dërgo vetëm porositë e paginuara te View
            return View(ordersToShow);
        }


    }
}
