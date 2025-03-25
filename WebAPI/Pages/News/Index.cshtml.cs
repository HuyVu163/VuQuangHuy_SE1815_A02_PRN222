using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
namespace WebAPI.Pages.News
{
    
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _context;
        private readonly ICategoryService _category;
        public IndexModel()
        {
            _context = new NewsArticleService();
            _category = new CategoryService();
        }
        public IList<NewsArticle> NewsArticle { get; set; } = default!;
        public IList<Category> NewCategories { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public String? SearchString { get; set; } = "";
        [BindProperty(SupportsGet = true)]
        public short? CategoryId { get; set; } = null;

        public async Task OnGetAsync()
        {
            int pageIndex =1;
            int pageSize = 1999999999;
            try
            {
                var newsArticles = _context.GetNewsByPaging(SearchString,pageIndex, pageSize,CategoryId) ?? new List<NewsArticle>();
                var category = _category.GetAllCategories("true") ?? new List<Category>();
                NewsArticle = newsArticles.ToList();
                NewCategories = category.ToList();
            }
            catch (Exception e)
            {
                NewsArticle = new List<NewsArticle>();
                NewCategories = new List<Category>();
            }
        }
    }
}
