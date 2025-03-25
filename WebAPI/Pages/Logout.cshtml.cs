using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAPI.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            // Đăng xuất
            await HttpContext.SignOutAsync("CookieAuth");

            // Chuyển hướng về trang chủ hoặc trang đăng nhập
            return RedirectToPage("/Index");
        }
    }
}
