using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TechStore.Data;
using TechStore.Models;

namespace TechStore.Controllers
{
    public class DeleteModel : PageModel
    {
        private readonly TechStore.Data.ApplicationDbContext _context;

        public DeleteModel(TechStore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public CountryOrder CountryOrder { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.CountryOrders == null)
            {
                return NotFound();
            }

            var countryorder = await _context.CountryOrders.FirstOrDefaultAsync(m => m.Id == id);

            if (countryorder == null)
            {
                return NotFound();
            }
            else 
            {
                CountryOrder = countryorder;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.CountryOrders == null)
            {
                return NotFound();
            }
            var countryorder = await _context.CountryOrders.FindAsync(id);

            if (countryorder != null)
            {
                CountryOrder = countryorder;
                _context.CountryOrders.Remove(CountryOrder);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
