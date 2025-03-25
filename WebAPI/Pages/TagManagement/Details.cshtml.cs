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

namespace WebAPI.Pages.TagManagement
{
    [Authorize(Roles = "1")]
    public class DetailsModel : PageModel
    {
        private readonly ITagService _context;

        public DetailsModel()
        {
            _context = new TagService();
        }

        public Tag Tag { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
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
    }
}
