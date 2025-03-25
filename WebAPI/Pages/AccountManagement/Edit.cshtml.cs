using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebAPI.Pages.AccountManagement
{
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ISystemAccountService _context ;

        public EditModel()
        {
            _context = new SystemAccountService();
        }

        [BindProperty]
        public string email { get; set; }
        [BindProperty]
        public string id { get; set; }

        [BindProperty]
        public string role { get; set; }
        [BindProperty]
        public string name { get; set; }
        [Authorize]
        public async Task<JsonResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return new JsonResult(new { success = false, message = "Không tìm thấy tài khoản!" }) { StatusCode = 404 };
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy ID của người dùng hiện tại
            var userRoles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList(); // Lấy danh sách quyền
            var systemaccount = _context.GetAccountById(id);
            if (userRoles.Contains("1") && userId == id.ToString())
            {
                systemaccount = _context.GetAccountById(id);
            }
            else if (userRoles.Contains("0"))
            {
                systemaccount = _context.GetAccountById(id);
            }
            else
            {
                return new JsonResult(new { success = false, message = "Bạn không có quyền truy cập!" }) { StatusCode = 403 };
            }

            if (systemaccount == null)
            {
                return new JsonResult(new { success = false, message = "Không tìm thấy tài khoản!" }) { StatusCode = 404 };
            }
            return new JsonResult(new { success = true, data = systemaccount }) { StatusCode = 200 };
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        [Authorize]
        public async Task<IActionResult> OnPostAsync()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            // Deserialize vào DTO thay vì vào CreateModel
            var model = JsonSerializer.Deserialize<AdminAccountConfig>(body);

            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Role) || string.IsNullOrEmpty(model.Name))
            {
                return new JsonResult(new { success = false, message = "Vui lòng điền đầy đủ thông tin!" }) { StatusCode = 400 };
            }

            if (!int.TryParse(model.Role, out int role))
            {
                return new JsonResult(new { success = false, message = "Role không hợp lệ!" }) { StatusCode = 400 };
            }
            if (!int.TryParse(model.Id, out int id))
            {
                return new JsonResult(new { success = false, message = "Role không hợp lệ!" }) { StatusCode = 400 };
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy ID của người dùng hiện tại
            var userRoles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList(); // Lấy danh sách quyền
            var systemaccount = _context.GetAccountById(id);
            if (userRoles.Contains("1") || userRoles.Contains("2"))
            { 
                return new JsonResult(new { success = false, message = "Bạn không có quyền truy cập!" }) { StatusCode = 403 };
            }
            try
            {
                SystemAccount systemAccount = _context.GetAccountById(id);
                systemAccount.AccountEmail = model.Email;
                systemAccount.AccountRole = role;
                systemAccount.AccountName = model.Name;
                _context.Update(systemAccount);
                var claimsIdentity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, systemAccount.AccountName),
                    new Claim(ClaimTypes.Email, systemAccount.AccountEmail),
                    new Claim(ClaimTypes.Role, systemAccount.AccountRole.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, systemAccount.AccountId.ToString())
                }, "CookieAuth");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
                };
                //await HttpContext.SignOutAsync("CookieAuth");
                //await HttpContext.SignInAsync("CookieAuth", claimsPrincipal, authProperties);


            }
            catch (Exception e)
            {
                return new JsonResult(new { success = false, message = "Có lỗi xảy ra!" }) { StatusCode = 500 };
            }

            return new JsonResult(new { success = true, message = "Cập nhật tài khoản thành công!" }) { StatusCode = 200 };
        }

    }
}
