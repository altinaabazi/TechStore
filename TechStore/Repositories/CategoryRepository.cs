using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using TechStore.Data;
using TechStore.Models;

namespace TechStore.Repositories;

public interface ICategoryRepository
{
    Task AddCategory(Category Category);
    Task UpdateCategory(Category Category);
    Task<Category?> GetCategoryById(int id);
    Task DeleteCategory(Category Category);
    Task<IEnumerable<Category>> GetCategories();
}
public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddCategory(Category Category)
    {
        _context.Categories.Add(Category);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateCategory(Category Category)
    {
        _context.Categories.Update(Category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategory(Category Category)
    {
        _context.Categories.Remove(Category);
        await _context.SaveChangesAsync();
    }

    public async Task<Category?> GetCategoryById(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<IEnumerable<Category>> GetCategories()
    {
        return await _context.Categories.ToListAsync();
    }


}