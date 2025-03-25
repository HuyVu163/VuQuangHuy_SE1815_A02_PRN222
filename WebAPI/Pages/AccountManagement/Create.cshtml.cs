using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.Models;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Services;
using Microsoft.AspNetCore.Authorization;
namespace WebAPI.Pages.AccountManagement
{
    [IgnoreAntiforgeryToken]
    [Authorize(Roles = "0")]
    public class CreateModel : PageModel
    {
        private readonly ISystemAccountService _context;
        public CreateModel()
        {
            _context = new SystemAccountService();
        }
        [BindProperty]
        public string email { get; set; }

        [BindProperty]
        public string password { get; set; }

        [BindProperty]
        public string role { get; set; }
        [BindProperty]
        public string name { get; set; }
        public async Task<JsonResult> OnPostAsync()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            // Deserialize vào DTO thay vì vào CreateModel
            var model = JsonSerializer.Deserialize<AdminAccountConfig>(body);

            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.Role) || string.IsNullOrEmpty(model.Name))
            {
                return new JsonResult(new { success = false, message = "Vui lòng điền đầy đủ thông tin!" }) { StatusCode = 400 };
            }

            if (_context.CheckEmailExists(model.Email))
            {
                return new JsonResult(new { success = false, message = "Email đã tồn tại!" }) { StatusCode = 400 };
            }

            if (!int.TryParse(model.Role, out int role))
            {
                return new JsonResult(new { success = false, message = "Role không hợp lệ!" }) { StatusCode = 400 };
            }
            try
            {
                _context.Add(new SystemAccount
                {
                    AccountEmail = model.Email,
                    AccountPassword = model.Password,
                    AccountRole = role,
                    AccountName = model.Name
                });
            }
            catch (Exception e)
{
    return new JsonResult(new { success = false, message = e.Message }) { StatusCode = 500 };
}

            return new JsonResult(new { success = true, message = "Tạo tài khoản thành công!" }) { StatusCode = 200 };
        }
    }
}
