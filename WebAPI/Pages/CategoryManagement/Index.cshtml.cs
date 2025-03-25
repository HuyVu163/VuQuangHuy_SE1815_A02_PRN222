using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Services;
using Microsoft.AspNetCore.Authorization;
namespace WebAPI.Pages.CategoryManagement
{
    [Authorize(Roles = "1")]
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _context;

        public IndexModel()
        {
            _context = new CategoryService();
        }

        public IList<Category> Category { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public String SearchString { get; set; }

        public async Task OnGetAsync()
        {
            IEnumerable<Category> list = _context.GetAllCategories("all") ?? new List<Category>();

            if (!string.IsNullOrEmpty(SearchString))
            {
                try
                {
                    list = list.Where(s => s.CategoryName.ToLower().Contains(SearchString.ToLower()));
                }
                catch (Exception e)
                {
                    list = new List<Category>();
                }
            }
            Category = list.ToList();
        }
    }
}
