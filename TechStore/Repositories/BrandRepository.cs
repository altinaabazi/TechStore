using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using TechStore.Data;
using TechStore.Models;

namespace TechStore.Repositories;

public interface IBrandRepository
{
    Task AddBrand(Brand Brand);
    Task UpdateBrand(Brand Brand);
    Task<Brand?> GetBrandById(int id);
    Task DeleteBrand(Brand Brand);
    Task<IEnumerable<Brand>> GetBrands();
}
public class BrandRepository : IBrandRepository
{
    private readonly ApplicationDbContext _context;
    public BrandRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddBrand(Brand Brand)
    {
        _context.Brands.Add(Brand);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateBrand(Brand Brand)
    {
        _context.Brands.Update(Brand);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBrand(Brand Brand)
    {
        _context.Brands.Remove(Brand);
        await _context.SaveChangesAsync();
    }

    public async Task<Brand?> GetBrandById(int id)
    {
        return await _context.Brands.FindAsync(id);
    }

    public async Task<IEnumerable<Brand>> GetBrands()
    {
        return await _context.Brands.ToListAsync();
    }


}