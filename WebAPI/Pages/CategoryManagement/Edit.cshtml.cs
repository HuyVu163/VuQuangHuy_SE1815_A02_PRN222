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
using static WebAPI.Utils.CUtils;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Pages.CategoryManagement
{
    [IgnoreAntiforgeryToken]
    [Authorize(Roles = "1")]
    public class EditModel : PageModel
    {
        private readonly ICategoryService _context;

        public EditModel()
        {
            _context = new CategoryService();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<JsonResult> OnGetAsync(short? id)
        {
            if (id == null)
            {
                return new JsonResult(new { success = false, message = "Không tìm thấy tài khoản!" }) { StatusCode = 404 };
            }

            var category =  _context.GetCategoryById(id);
            if (category == null)
            {
                return new JsonResult(new { success = false, message = "Không tìm thấy tài khoản!" }) { StatusCode = 404 };
            }
            return new JsonResult(new { success = true, data = category }) { StatusCode = 200 };
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
                ,
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new EmptyStringToNullableIntConverter());
            options.Converters.Add(new EmptyStringToNullableBooleanConverter());

            Category model = JsonSerializer.Deserialize<Category>(body, options);

            if (model.CategoryId != Convert.ToInt32(model.CategoryId) && _context.GetCategoryById(model.CategoryId) == null)
            {
                return new JsonResult(new { success = false, message = "Không tìm thấy tài khoản!" }) { StatusCode = 404 };
            }
            try
            {

                

                _context.UpdateCategory(model);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message }) { StatusCode = 500 };
            }
            return new JsonResult(new { success = true, message = "Cập nhật thành công" }) { StatusCode = 200 };

        }
    }
}
