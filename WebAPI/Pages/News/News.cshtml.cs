using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using BusinessObjects.Models;

namespace WebAPI.Pages.News
{
    public class NewsModel : PageModel
    {
        private readonly INewsArticleService _context;
        [BindProperty(SupportsGet =true)]
        public String id { get; set; } = default!;
        public NewsArticle NewsArticle { get; set; } = default!;
        public NewsModel()
        {
            _context = new NewsArticleService();
        }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                NewsArticle = _context.GetNewsArticleById(id);
                if (NewsArticle == null)
                {
                    TempData["ErrorMessage"] = "News doesn't exits";
                    return Page();
                }
                else
                if (NewsArticle.NewsStatus == false)
                {
                    TempData["ErrorMessage"] = "Status news must be active";
                    return Page();
                }
                return Page();
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "News doesn't exits";
                return Page();
            }
        }


    }
}
