using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStore.Data;
using TechStore.Models.DTOs;
using TechStore.Repositories;

namespace TechStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepo;
        private readonly IProductRepository _productService;
        private readonly ApplicationDbContext _db;


        public CartController(ICartRepository cartRepo,IProductRepository productService, ApplicationDbContext db )
        {
            _cartRepo = cartRepo;
            _productService=productService;
            _db = db;
        }
        public async Task<IActionResult> AddItem(int productId, int qty = 1, int redirect = 0)
        {
            var cartCount = await _cartRepo.AddItem(productId, qty);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }

        
        public async Task<IActionResult> RemoveItem(int productId)
        {
            var cartCount = await _cartRepo.RemoveItem(productId);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepo.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartRepo.GetCartItemCount();
            return Ok(cartItem);
        }

        public IActionResult Checkout()
        {
            ViewBag.Countries = _db.CountryOrders.ToList();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                bool isCheckedOut = await _cartRepo.DoCheckout(model);

                if (!isCheckedOut)
                {
                    ViewBag.ErrorMessage = "Procesi i checkout dështoi. Ju lutemi, provoni përsëri.";
                    return View("OrderFailure", model);
                }

                return RedirectToAction(nameof(OrderSuccess));
            }
            catch (Exception ex)
            {
                // Logimi i gabimit për diagnostikim
                Console.WriteLine($"Gabim gjatë checkout: {ex.Message}");

                // Dërgo gabimin në View
                ViewBag.ErrorMessage = $"Ka ndodhur një gabim: {ex.Message}";
                return View("OrderFailure", model);
            }
        }


        public IActionResult Details(int id)
        {
            var product = _productService.GetProductById(id); // merrni produktin nga shërbimi
            if (product == null)
            {
                return NotFound();
            }
            return View(product); // transmetoni produktin në pamje
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }

        public IActionResult OrderFailure()
        {
            return View();
        }

    }
}
