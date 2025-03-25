using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace WebAPI.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SystemAccountService _systemAccountService;
        private readonly IConfiguration _configuration;
        public LoginModel()
        {
            _systemAccountService = new SystemAccountService();
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }
        [BindProperty]
        public SystemAccount Account { get; set; } = new SystemAccount();
        public string Message { get; set; }


        public async Task<IActionResult> OnPost()
        {
   
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];
            if (Account.AccountEmail == adminEmail && Account.AccountPassword == adminPassword)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Admin"),
                    new Claim(ClaimTypes.Email, adminEmail),
                    new Claim(ClaimTypes.Role, "0"),
                    new Claim(ClaimTypes.NameIdentifier, "SYS")
                };

                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
                };
                await HttpContext.SignInAsync("CookieAuth", claimsPrincipal, authProperties);
                return RedirectToPage("/Index");
            }
            else 
            { 
            var account = _systemAccountService.Login(Account.AccountEmail, Account.AccountPassword);
            if (account != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.AccountName),
                    new Claim(ClaimTypes.Email, account.AccountEmail),
                    new Claim(ClaimTypes.Role, account.AccountRole.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString())

                };

                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
                };
                await HttpContext.SignInAsync("CookieAuth", claimsPrincipal, authProperties);
                if(account.AccountRole == 2)
                    {
                    return RedirectToPage("/News/Index");
                }
                return RedirectToPage("/Index");
            }else
            {
                    // Send message to the user
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }
            return Page();
        }
      
    }
}
