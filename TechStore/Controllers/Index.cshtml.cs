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
    public class IndexModel : PageModel
    {
        private readonly TechStore.Data.ApplicationDbContext _context;

        public IndexModel(TechStore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CountryOrder> CountryOrder { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.CountryOrders != null)
            {
                CountryOrder = await _context.CountryOrders.ToListAsync();
            }
        }
    }
}
