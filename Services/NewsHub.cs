using BusinessObjects.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebAPI.Hubs
{
    public class NewsHub : Hub
    {
        public async Task NotifyNewsCreated(NewsArticle newsArticle)
        {
            await Clients.All.SendAsync("NewsCreated", newsArticle);
        }

        public async Task NotifyNewsUpdated(NewsArticle newsArticle)
        {
            await Clients.All.SendAsync("NewsUpdated", newsArticle);
        }

        public async Task NotifyNewsDeleted(int newsArticleId)
        {
            await Clients.All.SendAsync("NewsDeleted", newsArticleId);
        }
    }
}