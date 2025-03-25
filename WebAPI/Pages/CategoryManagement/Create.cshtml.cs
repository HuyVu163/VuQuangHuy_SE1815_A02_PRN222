using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.Models;
using Services;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using static WebAPI.Utils.CUtils;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Pages.CategoryManagement
{
    [IgnoreAntiforgeryToken]
    [Authorize(Roles = "1")]
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _context;

        public CreateModel()
        {
            _context = new CategoryService();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<JsonResult> OnPostAsync()
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
            Category model = JsonSerializer.Deserialize<Category>(body, options);

            if (model.CategoryName.IsNullOrEmpty() || model.CategoryDesciption.IsNullOrEmpty())
            {
                return new JsonResult(new { success = false, message = "Vui lòng điền đầy đủ thông tin!" }) { StatusCode = 400 };
            }
            try
            {
                model.IsActive = true;
                _context.AddCategory(model);
                return new JsonResult(new { success = true, message = "Thêm tag thành công!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message }) { StatusCode = 500 };
            }

        }
    }
}
