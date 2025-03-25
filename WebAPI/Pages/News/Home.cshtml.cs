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
namespace WebAPI.Pages.News
{
    [Authorize(Roles = "1")]
    public class HomeModel : PageModel
    {
        private readonly INewsArticleService _context;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;

        public HomeModel()
        {
            _context = new NewsArticleService();
            _categoryService = new CategoryService();
            _tagService = new TagService();
        }
        [BindProperty(SupportsGet = true)]
        public String SearchString { get; set; }
        public IList<NewsArticle> NewsArticle { get;set; } = default!;
        public IList<Category> CategoryList { get; set; } = default!;
        public IList<Tag> TagList { get; set; } = default!;

        public async Task OnGetAsync()
        {
            try
            {
                TagList = _tagService.GetAllTagsWithNews().ToList();
                CategoryList = _categoryService.GetAllCategories("true").ToList();
                if (!string.IsNullOrEmpty(SearchString))
                {
                    NewsArticle = _context.GetAllNewsArticles("all").Where(s => s.NewsTitle.ToLower().Contains(SearchString.ToLower())).ToList();
                    
                }
                else
                NewsArticle = _context.GetAllNewsArticles("all").ToList();
            }
            catch (Exception ex)
            {
                NewsArticle = new List<NewsArticle>();
                TagList = new List<Tag>();
                CategoryList = new List<Category>();
            }
        }
    }
}
