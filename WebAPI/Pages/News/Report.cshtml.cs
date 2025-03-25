using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Services;
namespace WebAPI.Pages.News
{
    [Authorize(Roles = "0")]
    public class ReportModel : PageModel
    {
        private readonly NewsArticleService _context;
        public ReportModel()
        {
            _context = new NewsArticleService();
        }
        [BindProperty(SupportsGet = true)]
        public String StartDate { get; set; } = "";
        [BindProperty(SupportsGet = true)]

        public String EndDate { get; set; } = "";
        public ReportDTO ReportDTO { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(String startDate, String endDate)
        {
            if (startDate.IsNullOrEmpty()|| endDate.IsNullOrEmpty())
            {
                startDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd 00:00:00.000");
                endDate = DateTime.Now.ToString("yyyy-MM-dd 23:59:59.999");
                ReportDTO = _context.GetReportData(startDate, endDate);
                StartDate = startDate.Replace(" 00:00:00.000", "");
                EndDate = endDate.Replace(" 23:59:59.999", "");
                return Page();
            }
            startDate = DateTime.Parse(startDate).AddDays(-1).ToString("yyyy-MM-dd 23:59:59.999");
            endDate = DateTime.Parse(endDate).ToString("yyyy-MM-dd 23:59:59.999");
            StartDate = startDate.Replace(" 23:59:59.999", "");
            EndDate = endDate.Replace(" 23:59:59.999", "");
            ReportDTO = _context.GetReportData(startDate, endDate);
            return Page();
        }
    }
}
