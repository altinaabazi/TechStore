using TechStore.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechStore.Models.DTOs;
using TechStore.Repositories;
using Microsoft.EntityFrameworkCore;
using TechStore.Data;

namespace TechStore.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class AdminOperationsController : Controller
    {
        private readonly IUserOrderRepository _userOrderRepository;
        private readonly ApplicationDbContext _context;

        public AdminOperationsController(IUserOrderRepository userOrderRepository, ApplicationDbContext context)
        {
            _userOrderRepository = userOrderRepository;
            _context = context;
        }
        //public async Task<IActionResult> AllOrders(int? countryOrderId)
        //{
        //    // Get list of countries for dropdown
        //    var countries = await _context.CountryOrders
        //                                  .Select(c => new { c.Id, c.Name })
        //                                  .ToListAsync();
        //    ViewBag.Countries = countries;
        //    ViewBag.SelectedCountryId = countryOrderId;

        //    // Get orders and filter by CountryOrderId if provided
        //    var orders = _context.Orders
        //                          .Include(o => o.OrderDetail)
        //                                 .ThenInclude(od => od.Product)
        //                                 .ThenInclude(p => p.Brand)
        //                                 .Include(o => o.OrderDetail)
        //                                 .ThenInclude(od => od.Product)
        //                                 .ThenInclude(p => p.Category)
        //                                 .Include(o => o.OrderStatus)
        //                         .AsQueryable();

        //    if (countryOrderId.HasValue)
        //    {
        //        orders = orders.Where(o => o.CountryOrderId == countryOrderId.Value);
        //    }

        //    return View(await orders.ToListAsync());
        //}
        public async Task<IActionResult> AllOrders(int? countryOrderId, int page = 1)
        {
            int pageSize = 5; // Numri i porosive për faqe

            // Get list of countries for dropdown
            var countries = await _context.CountryOrders
                                            .Select(c => new { c.Id, c.Name })
                                            .ToListAsync();
            ViewBag.Countries = countries;
            ViewBag.SelectedCountryId = countryOrderId;

            // Get orders and filter by CountryOrderId if provided
            var ordersQuery = _context.Orders
                                      .Include(o => o.OrderDetail)
                                             .ThenInclude(od => od.Product)
                                             .ThenInclude(p => p.Brand)
                                      .Include(o => o.OrderDetail)
                                             .ThenInclude(od => od.Product)
                                             .ThenInclude(p => p.Category)
                                      .Include(o => o.OrderStatus)
                                      .AsQueryable();

            if (countryOrderId.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.CountryOrderId == countryOrderId.Value);
            }

            // Paginimi
            var totalOrders = await ordersQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalOrders / pageSize);

            var ordersToShow = await ordersQuery
                                        .Skip((page - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            // Shto të dhënat e paginimit në ViewBag
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            // Krijo modelin për View
            return View(ordersToShow);
        }




        // Toggle the payment status of an order
        public async Task<IActionResult> TogglePaymentStatus(int orderId)
        {
            try
            {
                await _userOrderRepository.TogglePaymentStatus(orderId);
            }
            catch (Exception ex)
            {
                // log exception here
            }
            return RedirectToAction(nameof(AllOrders));
        }

        // Show the update order status page
        public async Task<IActionResult> UpdateOrderStatus(int orderId)
        {
            var order = await _userOrderRepository.GetOrderById(orderId);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with id:{orderId} does not found.");
            }

            var orderStatusList = (await _userOrderRepository.GetOrderStatuses()).Select(orderStatus =>
            {
                return new SelectListItem { Value = orderStatus.Id.ToString(), Text = orderStatus.StatusName, Selected = order.OrderStatusId == orderStatus.Id };
            });

            var data = new UpdateOrderStatusModel
            {
                OrderId = orderId,
                OrderStatusId = order.OrderStatusId,
                OrderStatusList = orderStatusList
            };

            return View(data);
        }

        // Handle the update order status POST request
        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(UpdateOrderStatusModel data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    data.OrderStatusList = (await _userOrderRepository.GetOrderStatuses()).Select(orderStatus =>
                    {
                        return new SelectListItem { Value = orderStatus.Id.ToString(), Text = orderStatus.StatusName, Selected = orderStatus.Id == data.OrderStatusId };
                    });

                    return View(data);
                }

                await _userOrderRepository.ChangeOrderStatus(data);
                TempData["msg"] = "Updated successfully";
            }
            catch (Exception ex)
            {
                // log exception here
                TempData["msg"] = "Something went wrong";
            }
            return RedirectToAction(nameof(UpdateOrderStatus), new { orderId = data.OrderId });
        }

        // API endpoint to get order details for a specific order
        //[HttpGet]
        //[Route("api/order")]
        //public IActionResult GetOrderDetails(int orderId)
        //{
        //    var orderDetails = _context.OrderDetails
        //        .Include(od => od.Product)
        //        .ThenInclude(p => p.Brand)
        //        .Include(od => od.Product)
        //        .ThenInclude(p => p.Category)
        //        .Where(od => od.OrderId == orderId)
        //        .ToList();

        //    if (orderDetails == null || !orderDetails.Any())
        //    {
        //        return NotFound("No order details found for the given orderId");
        //    }

        //    var orderDetailDto = new OrderDetailModalDTO
        //    {
        //        OrderDetails = orderDetails.Select(od => new OrderDetailModal
        //        {
        //            Id = od.Id,
        //            OrderId = od.OrderId,
        //            ProductName = od.Product?.ProductName ?? "N/A",
        //            BrandName = od.Product?.Brand?.Name ?? "N/A",
        //            CategoryName = od.Product?.Category?.Name ?? "N/A",
        //            UnitPrice = od.UnitPrice,
        //            Quantity = od.Quantity,
        //            TotalPrice = od.Quantity * od.UnitPrice
        //        }).ToList()
        //    };

        //    return Ok(orderDetailDto); // Ensure that the data is returned using Ok()
        //}
        [HttpGet]
        [Route("api/order")]
        public IActionResult GetOrderDetails(int orderId)
        {
            var orderDetails = _context.OrderDetails
                .Include(od => od.Product)
                .ThenInclude(p => p.Brand)
                .Include(od => od.Product)
                .ThenInclude(p => p.Category)
                .Where(od => od.OrderId == orderId)
                .ToList();

            if (orderDetails == null || !orderDetails.Any())
            {
                return NotFound("No order details found for the given orderId");
            }

            var orderDetailDto = new OrderDetailModalDTO
            {
                OrderDetail = orderDetails.Select(od => new OrderDetailModal
                {
                    Id = od.Id,
                    OrderId = od.OrderId,
                    ProductName = od.Product?.ProductName ?? "N/A",
                    BrandName = od.Product?.Brand?.Name ?? "N/A",
                    CategoryName = od.Product?.Category?.Name ?? "N/A",
                    UnitPrice = od.UnitPrice,
                    Quantity = od.Quantity,
                    TotalPrice = od.Quantity * od.UnitPrice
                }).ToList()
            };

            return Ok(orderDetailDto);
        }

        

        // Delete an order
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            try
            {
                // Fetch the order from the database
                var order = await _context.Orders.FindAsync(orderId);

                if (order == null)
                {
                    // If order not found, send an error message
                    TempData["msg"] = "Order not found!";
                    return RedirectToAction(nameof(AllOrders));
                }

                // Delete the order from the database
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                TempData["msg"] = "Order deleted successfully";
            }
            catch (Exception ex)
            {
                // Log the exception if an error occurs
                TempData["msg"] = "An error occurred while deleting the order";
            }

            // After deletion, redirect back to the order list
            return RedirectToAction(nameof(AllOrders));
        }


        // Dashboard view
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
