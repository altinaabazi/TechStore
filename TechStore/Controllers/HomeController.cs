using System.Diagnostics;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using TechStore.Models;
using TechStore.Data;
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
        public async Task<IActionResult> Index(string sterm = "", int brandId = 0)
        {

            IEnumerable<Product> products = await _homeRepository.GetProducts(sterm, brandId);
            IEnumerable<Brand> brands = await _homeRepository.Brands();
            ProductDisplayModel productModel = new ProductDisplayModel
            {
                Products = products,
                Brands = brands,
              STerm = sterm,
              BrandId = brandId
            };
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
