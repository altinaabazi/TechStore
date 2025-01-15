
 using TechStore.Data;
 using TechStore.Models;
 using TechStore.Repositories;
using TechStore.Controllers;

namespace TechStore
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Product>> GetProducts(string sTerm = "", int brandId = 0);
        Task<IEnumerable<Brand>> Brands();
    }
}