using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Services;
using System.Text.Json;
using static WebAPI.Utils.CUtils;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.Pages.News
{
    [IgnoreAntiforgeryToken]
    [Authorize(Roles = "1")]
    public class EditModel : PageModel
    {
        private readonly INewsArticleService _context;
        private readonly IHubContext<NewsHub> _hubContext;

        public EditModel(IHubContext<NewsHub> hubContext)
        {
            _context = new NewsArticleService();
            _hubContext = hubContext;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;

        public async Task<JsonResult> OnGetAsync(String? id)
        {
            try
            {
                if (id == null)
                {
                    return new JsonResult(new { success = false, message = "Not found news aricle!" }) { StatusCode = 404 };
                }

                var newsarticle = _context.GetNewsArticleById(id);

                if (newsarticle == null)
                {
                    return new JsonResult(new { success = false, message = "Not found news aricle!" }) { StatusCode = 404 };
                }
                else
                {
                    NewsArticle = newsarticle;
                }
                return new JsonResult(new { success = true, data = NewsArticle }) { StatusCode = 200};

            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message }) { StatusCode = 500 };

            }
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<JsonResult> OnPostAsync()
        {
                try
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

                    if(model.CategoryId == null || model.NewsTitle.IsNullOrEmpty() || model.NewsContent.IsNullOrEmpty() || model.NewsSource.IsNullOrEmpty() || model.Headline.IsNullOrEmpty())
                    {
                        return new JsonResult(new { success = false, message = "Please fill in all required fields!" }) { StatusCode = 400 };
                    }
                    else
                    {
                        var news = _context.GetNewsArticleById(model.NewsArticleId);
                    if (news == null)
                    {
                        return new JsonResult(new { success = false, message = "Not found news article!" }) { StatusCode = 404 };
                    }
                    }
                    try
                    {
                    // Get id from token
                    ClaimsPrincipal currentUser = this.User;
                    short? id = Convert.ToInt16(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);
                    model.UpdatedById = id;
                    model.ModifiedDate = DateTime.Now;
                     _context.UpdateNewsArticle(model);
                    var updatedArticle = _context.GetNewsArticleById(model.NewsArticleId);

                    // Create a simplified version for SignalR
                    var articleForSignalR = new
                    {
                        updatedArticle.NewsArticleId,
                        updatedArticle.NewsTitle,
                        updatedArticle.Headline,
                        updatedArticle.NewsContent,
                        updatedArticle.NewsSource,
                        updatedArticle.NewsStatus,
                        updatedArticle.CreatedDate,
                        updatedArticle.ModifiedDate,
                        Category = new { updatedArticle.Category.CategoryName },
                        CreatedBy = new { updatedArticle.CreatedBy.AccountEmail },
                        UpdatedById = updatedArticle.UpdatedById,
                        Tags = updatedArticle.Tags.Select(t => new { t.TagId, t.TagName }).ToList()
                    };

                    await _hubContext.Clients.All.SendAsync("NewsUpdated", articleForSignalR);
                    return new JsonResult(new { success = true, message = "Update news article successfully!" }) { StatusCode = 200 };
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult(new { success = false, message = ex.Message }) { StatusCode = 500 };
                    }
                }
                catch (Exception ex)
                {
                    return new JsonResult(new { success = false, message = ex.Message }) { StatusCode = 500 };
                }

        
        }

        
    }
}
