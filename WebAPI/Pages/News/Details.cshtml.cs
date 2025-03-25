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

namespace WebAPI.Pages.News
{
    [Authorize(Roles = "1")]
    public class DetailsModel : PageModel
    {
        private readonly INewsArticleService _context;

        public DetailsModel()
        {
            _context = new NewsArticleService();
        }

        public NewsArticle NewsArticle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(String? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["ErrorMessage"] = "News doesn't exits";
                    return RedirectToPage("./Index");
                }

                NewsArticle = _context.GetNewsArticleById(id);
                if (NewsArticle == null)
                {
                    TempData["ErrorMessage"] = "News doesn't exits";
                    return RedirectToPage("./Index");
                }
                else
                {
                    return Page();
                }

            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return Page();
            }
        }
    }
}
