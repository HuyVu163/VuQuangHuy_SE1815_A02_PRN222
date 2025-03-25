using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.Models;
using Services;
using static WebAPI.Utils.CUtils;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.Pages.News
{
    [IgnoreAntiforgeryToken]
    [Authorize(Roles = "1")]
    public class CreateModel : PageModel
    {
        private readonly INewsArticleService _context;
        private readonly ISystemAccountService _systemAccountService;
        private readonly ITagService _tagService;
        private readonly IHubContext<NewsHub> _hubContext;

        public CreateModel(BusinessObjects.Models.NmsContext context, IHubContext<NewsHub> hubContext)
        {
            _context = new NewsArticleService();
            _systemAccountService = new SystemAccountService();
            _tagService = new TagService();
            _hubContext = hubContext;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;
        [BindProperty]
        public ICollection<Tag> Tags { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<JsonResult> OnPostAsync()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            var options = new JsonSerializerOptions
            {
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
                ,
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new EmptyStringToNullableIntConverter());
            options.Converters.Add(new EmptyStringToNullableBooleanConverter());
            options.Converters.Add(new TagCollectionConverter());
            NewsArticle model = JsonSerializer.Deserialize<NewsArticle>(body, options);
            var tags = model.Tags;
            if (model.NewsTitle.IsNullOrEmpty() || model.NewsContent.IsNullOrEmpty() || model.NewsSource.IsNullOrEmpty() || !model.CategoryId.HasValue || model.Headline.IsNullOrEmpty())
            {
                return new JsonResult(new { success = false, message = "Vui lòng điền đầy đủ thông tin!" }) { StatusCode = 400 };
            }
            try
            {
                model.NewsStatus = true;
                model.CreatedDate = DateTime.Now;
                // Get email from token
                ClaimsPrincipal currentUser = this.User;
                short? id = Convert.ToInt16(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);
                model.CreatedById = id;
                var createdArticle = _context.AddNewsArticle(model);

                // Fetch the full article with related data for SignalR
                var articleWithRelations = _context.GetNewsArticleById(createdArticle.NewsArticleId);

                // Create a simplified version for SignalR to avoid circular references
                var articleForSignalR = new
                {
                    articleWithRelations.NewsArticleId,
                    articleWithRelations.NewsTitle,
                    articleWithRelations.Headline,
                    articleWithRelations.NewsContent,
                    articleWithRelations.NewsSource,
                    articleWithRelations.NewsStatus,
                    articleWithRelations.CreatedDate,
                    articleWithRelations.ModifiedDate,
                    Category = new { articleWithRelations.Category.CategoryName },
                    CreatedBy = new { articleWithRelations.CreatedBy.AccountEmail },
                    UpdatedById = articleWithRelations.UpdatedById,
                    Tags = articleWithRelations.Tags.Select(t => new { t.TagId, t.TagName }).ToList()
                };

                await _hubContext.Clients.All.SendAsync("NewsCreated", articleForSignalR);
                return new JsonResult(new { success = true, message = "Thêm bài viết thành công!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message }) { StatusCode = 500 };
            }
        }
    }
}
