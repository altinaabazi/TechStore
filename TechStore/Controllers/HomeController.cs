using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechStore.Models;
using TechStore.Models.DTOs;

namespace TechStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;


        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
            _logger = logger;
        }
      
        public async Task<IActionResult> Index(string sterm = "", int brandId = 0, int page = 1)
        {
            int pageSize = 10; // Numri i produkteve për faqe

            // Merrni produktet dhe brandet
            IEnumerable<Product> products = await _homeRepository.GetProducts(sterm, brandId);
            IEnumerable<Brand> brands = await _homeRepository.Brands();

            // Paginimi
            var totalProducts = products.Count();
            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            // Merrni vetëm produktet për faqe të caktuar
            var productsToShow = products.Skip((page - 1) * pageSize).Take(pageSize);

            // Modeli për të dërguar në view
            ProductDisplayModel productModel = new ProductDisplayModel
            {
                Products = productsToShow,
                Brands = brands,
                STerm = sterm,
                BrandId = brandId
            };

            // Shto të dhënat për paginim në ViewBag
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(productModel);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
