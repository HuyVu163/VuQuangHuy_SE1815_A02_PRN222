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

namespace WebAPI.Pages.TagManagement
{
    [Authorize(Roles = "1")]
    public class IndexModel : PageModel
    {
        private readonly ITagService _context;

        public IndexModel()
        {
            _context = new TagService();
        }

        public IList<Tag> Tag { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public String SearchString { get; set; }

        public async Task OnGetAsync()
        {
            IEnumerable<Tag> listA = _context.GetAllTags() ?? new List<Tag>();
            
            if (!string.IsNullOrEmpty(SearchString))
            {
                try 
                {
                    listA = listA.Where(s => s.TagName.ToLower().Contains(SearchString.ToLower()));
                }
                catch (Exception e)
                {
                    listA = new List<Tag>();
                }
            }
            
            Tag = listA.ToList();
        }
    }
}
