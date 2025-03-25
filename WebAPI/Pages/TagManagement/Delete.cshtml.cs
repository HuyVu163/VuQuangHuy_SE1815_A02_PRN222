using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Services;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
namespace WebAPI.Pages.TagManagement
{
    [Authorize(Roles = "1")]
    public class DeleteModel : PageModel
    {
        private readonly ITagService _context;

        public DeleteModel()
        {
            _context = new TagService();
        }

        [BindProperty]
        public Tag Tag { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try { 
            if (id == null)
            {
                    TempData["ErrorMessageNotification"] = "Not Found Tag!";
                    return Page();
                }

            var tag = _context.GetTagById(id);

            if (tag == null)
            {
                    TempData["ErrorMessageNotification"] = "Not Found Tag!";
                    return Page();
                }
            else
            {
                Tag = tag;
            }
            }
            catch
            {
                TempData["ErrorMessageNotification"] = "Something Error!";
                return Page();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = _context.GetTagById(id);
            if (tag != null)
            {
                Tag = tag;
                // Check tag have any news
                if (Tag.NewsArticles.Count > 0)
                {
                    // Show aleart message if tag have any news
                    TempData["ErrorMessage"] = "Tag have news, please remove news before delete";
                    return Page();
                }

                _context.Delete(Tag);
                // Show aleart message after delete
                TempData["SuccessMessage"] = "Tag deleted successfully";
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
