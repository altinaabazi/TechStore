using TechStore.Data;
using TechStore.Models;
using Microsoft.EntityFrameworkCore;


namespace TechStore.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;
        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Brand>> Brands()
        {
            return await _db.Brands.ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProducts(string sTerm = "", int brandId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Product> products = await (from product in _db.Products
                                                join brand in _db.Brands
                                             on product.BrandId equals brand.Id
                                             where string.IsNullOrWhiteSpace(sTerm) || (product != null && product.ProductName.ToLower().StartsWith(sTerm))
                                             select new Product
                                             {
                                                 Id = product.Id,
                                                 Image = product.Image,
                                                 ProductName = product.ProductName,
                                                 CategoryId = product.CategoryId,
                                                 BrandId = product.BrandId,
                                                 Price = product.Price,
                                                 BrandName = brand.Name
                                             }
                         ).ToListAsync();
            if (brandId > 0)
            {
                products = products.Where(a => a.BrandId == brandId).ToList();
            }
            return products;
        }
    }
}
