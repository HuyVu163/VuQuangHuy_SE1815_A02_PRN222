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
    public class DeleteModel : PageModel
    {
        private readonly ICategoryService _context;

        public DeleteModel()
        {
            _context = new CategoryService();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["ErrorMessageNotification"] = "Not found Category";
                    return Page();
                }

                var category = _context.GetCategoryById(id);

                if (category == null)
                {
                    TempData["ErrorMessageNotification"] = "Not found Category!";
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

        public async Task<IActionResult> OnPostAsync(short? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var category = _context.GetCategoryById(id);
                if (category != null)
                {
                    Category = category;
                    _context.DeleteCategory(id);
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return Page();
            }


            return RedirectToPage("./Index");
        }
    }
}
