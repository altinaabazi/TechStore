using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechStore.Data;
using TechStore.Models;

namespace TechStore.Controllers
{
    public class EditModel : PageModel
    {
        private readonly TechStore.Data.ApplicationDbContext _context;

        public EditModel(TechStore.Data.ApplicationDbContext context)
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

            var countryorder =  await _context.CountryOrders.FirstOrDefaultAsync(m => m.Id == id);
            if (countryorder == null)
            {
                return NotFound();
            }
            CountryOrder = countryorder;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(CountryOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryOrderExists(CountryOrder.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CountryOrderExists(int id)
        {
          return (_context.CountryOrders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
