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

namespace WebAPI.Pages.AccountManagement
{
    [Authorize(Roles = "0")]
    public class DetailsModel : PageModel
    {
        private readonly ISystemAccountService _context;

        public DetailsModel()
        {
            _context = new SystemAccountService();
        }

        public SystemAccount SystemAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["ErrorMessage"] = "Not found Account";
                }

                var systemaccount = _context.GetAccountById(id);
                if (systemaccount == null)
                {
                    TempData["ErrorMessage"] = "Not found Account!";
                }
                else
                {
                    SystemAccount = systemaccount;
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "Something Error!";
            }

            return Page();
        }
    }
}
