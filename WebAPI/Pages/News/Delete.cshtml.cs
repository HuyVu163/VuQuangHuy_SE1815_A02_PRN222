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
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.Pages.News
{
    [Authorize(Roles = "1")]
    public class DeleteModel : PageModel
    {
        private readonly INewsArticleService _context;
        private readonly IHubContext<NewsHub> _hubContext;


        public DeleteModel(IHubContext<NewsHub> hubContext)
        {
            _context = new NewsArticleService();
            _hubContext = hubContext;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(String? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["ErrorMessage"] = "News doesn't exits";
                    return Page();
                }

                NewsArticle = _context.GetNewsArticleById(id);
                if (NewsArticle == null)
                {
                    TempData["ErrorMessage"] = "News doesn't exits";
                    return Page();
                }
                else
                {
                    _context.DeleteNewsArticle(id);
                    TempData["SuccessMessage"] = "Delete success";
                    return Page();
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return Page();
            }

        }


        public async Task<IActionResult> OnPostAsync(String? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                NewsArticle = _context.GetNewsArticleById(id);
                if (NewsArticle == null)
                {
                    TempData["ErrorMessage"] = "News doesn't exits";
                    return Page();
                }
                else
                {
                    _context.DeleteNewsArticle(id);
                    await _hubContext.Clients.All.SendAsync("NewsDeleted", id);
                    TempData["SuccessMessage"] = "Delete success";
                    return Page();
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Something error";
                return Page();
            }
        }

    }
}
