using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.Models;
using Services;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Pages.TagManagement
{
    [IgnoreAntiforgeryToken]
    [Authorize(Roles = "1")]
    public class CreateModel : PageModel
    {
        private readonly ITagService _context;
        [BindProperty]
        public string tagName { get; set; }

        [BindProperty]
        public string note { get; set; }

        public CreateModel()
        {
            _context = new TagService();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<JsonResult> OnPostAsync()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            Tag model = JsonSerializer.Deserialize<Tag>(body);

            if (model.TagName.IsNullOrEmpty() )
            {
                return new JsonResult(new { success = false, message = "Vui lòng điền đầy đủ thông tin!" }) { StatusCode = 400 };
            }
            try
            {
                _context.Add(model);
                return new JsonResult(new { success = true, message = "Thêm tag thành công!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message }) { StatusCode = 500 };
            }

        }
    }
}
