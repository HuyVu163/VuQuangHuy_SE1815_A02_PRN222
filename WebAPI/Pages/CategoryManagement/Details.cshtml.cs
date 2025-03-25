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
    public class DetailsModel : PageModel
    {
        private readonly ICategoryService _context;

        public DetailsModel()
        {
            _context = new CategoryService();
        }

        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            try { 
            if (id == null)
            {
                    TempData["ErrorMessage"] = "Not found Category";
                // Sendirect to the Index page
                return Page();

            }

            var category = _context.GetCategoryById(id);
            if (category == null)
            {
                    TempData["ErrorMessage"] = "Not found Category!";
                return Page();
            }
            else
            {
                Category = category;
            }
            }
            catch
            {
                TempData["ErrorMessage"] = "Something Error!";
                return Page();
            }
            return Page();
        }
    }
}
