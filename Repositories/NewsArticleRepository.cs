using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccessLayer;

namespace Repositories
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        public NewsArticle AddNewsArticle(NewsArticle newsArticle)
        {
            NewsArticleDAO.AddNewsArticle(newsArticle);
            return newsArticle;
        }

        public void DeleteNewsArticle(String? id) => NewsArticleDAO.DeleteNewsArticle(id);

        public IEnumerable<NewsArticle> GetAllNewsArticles(String? status) => NewsArticleDAO.GetAllNewsArticles(status);
        public IEnumerable<NewsArticle> GetAllNewsArticlesByStaff(short status) => NewsArticleDAO.GetAllNewsArticlesByStaff(status);

        public NewsArticle GetNewsArticleById(String? id) => NewsArticleDAO.GetNewsArticleById(id);
        public IEnumerable<NewsArticle> GetNewsByPaging(string? strSearch, int? pageNumber, int? pageSize, short? categoryId) => NewsArticleDAO.GetNewsByPaging(strSearch, pageNumber, pageSize, categoryId);
        public void UpdateNewsArticle(NewsArticle newsArticle) => NewsArticleDAO.UpdateNewsArticle(newsArticle);
        public ReportDTO GetReportData(String startDate, String endDate) => NewsArticleDAO.GetReport(startDate, endDate);
    }
}
