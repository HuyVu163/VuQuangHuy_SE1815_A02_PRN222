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
    public class IndexModel : PageModel
    {
        private readonly ISystemAccountService _context;
        [BindProperty(SupportsGet = true)]
        public String SearchString { get; set; }

        public IndexModel()
        {
            _context = new SystemAccountService();
        }

        public IEnumerable<SystemAccount> SystemAccount { get; set; } = default!;

        public async Task OnGetAsync()
        {
            IEnumerable<SystemAccount> studentQuery = _context.GetAllAccounts() ?? new List<SystemAccount>();

            if (!string.IsNullOrEmpty(SearchString))
            {
                try
                {
                    studentQuery = studentQuery.Where(s => s.AccountName.ToLower().Contains(SearchString.ToLower()));
                }
                catch (Exception e)
                {
                    studentQuery = new List<SystemAccount>();
                }
            }

            SystemAccount = studentQuery;
        }
    }
}
