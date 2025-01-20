using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechStore.Data;
using TechStore.Models;

namespace TechStore.Controllers
{
    public class CreateModel : PageModel
    {
        private readonly TechStore.Data.ApplicationDbContext _context;

        public CreateModel(TechStore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CountryOrder CountryOrder { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.CountryOrders == null || CountryOrder == null)
            {
                return Page();
            }

            _context.CountryOrders.Add(CountryOrder);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
