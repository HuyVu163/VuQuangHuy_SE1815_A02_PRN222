using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Services;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Pages.TagManagement
{
    [IgnoreAntiforgeryToken]
    [Authorize(Roles = "1")]
    public class EditModel : PageModel
    {
        private readonly ITagService _context;

        public EditModel()
        {
            _context = new TagService();
        }

        [BindProperty]
        public string tagName { get; set; }
        [BindProperty]
        public string note { get; set; }

        [BindProperty]
        public string tagId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return new JsonResult(new { success = false, message = "Không tìm thấy tài khoản!" }) { StatusCode = 404 };
            }

            var tag =  _context.GetTagById(id);
            if (tag == null)
            {
                return new JsonResult(new { success = false, message = "Không tìm thấy tài khoản!" }) { StatusCode = 404 };
            }
            return new JsonResult(new { success = true, data = tag }) { StatusCode = 200 };
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            var options = new JsonSerializerOptions
            {
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
            };

            Tag model = JsonSerializer.Deserialize<Tag>(body, options);

            if (model.TagId != Convert.ToInt32(tagId) && _context.GetTagById(model.TagId) == null)
            {
                return new JsonResult(new { success = false, message = "Không tìm thấy tài khoản!" }) { StatusCode = 404 };
            }
            try
            {
                _context.Update(model);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message }) { StatusCode = 500 };
            }
            return new JsonResult(new { success = true, message = "Cập nhật thành công" }) { StatusCode = 200 };


        }
    }
}
