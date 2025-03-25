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
    public class DeleteModel : PageModel
    {
        private readonly ISystemAccountService _context;

        public DeleteModel()
        {
            _context = new SystemAccountService();
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["ErrorMessage"] = "Not found Account";
                }

                var systemaccount = _context.GetAccountById(id);
                if (systemaccount != null)
                {
                    SystemAccount = systemaccount;
                    _context.Delete(SystemAccount);

                }
            }
            catch(Exception e)
            {
                TempData["ErrorMessage"] = "Error :" + e.Message;
                return Page();
            }


            return RedirectToPage("./Index");
        }
    }
}
