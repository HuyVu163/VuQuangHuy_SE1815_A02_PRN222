using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories;
using BusinessObjects.Models;

namespace Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        public NewsArticleService()
        {
            _newsArticleRepository = new NewsArticleRepository();
        }
        public NewsArticle AddNewsArticle(NewsArticle newsArticle)
        {
            _newsArticleRepository.AddNewsArticle(newsArticle);
            return newsArticle;
        }
        public void UpdateNewsArticle(NewsArticle newsArticle)
        {
            _newsArticleRepository.UpdateNewsArticle(newsArticle);
        }
        public void DeleteNewsArticle(String? id)
        {
            _newsArticleRepository.DeleteNewsArticle(id);
        }
        public IEnumerable<NewsArticle> GetAllNewsArticles(String? status)
        {
            return _newsArticleRepository.GetAllNewsArticles(status);
        }
        public IEnumerable<NewsArticle> GetAllNewsArticlesByStaff(short status)
        {
            return _newsArticleRepository.GetAllNewsArticlesByStaff(status);
        }
        public NewsArticle GetNewsArticleById(String? id)
        {
            return _newsArticleRepository.GetNewsArticleById(id);
        }
        public IEnumerable<NewsArticle> GetNewsByPaging(string? strSearch, int? pageNumber, int? pageSize, short? categoryId)
        {
            return _newsArticleRepository.GetNewsByPaging(strSearch, pageNumber, pageSize, categoryId);
        }
        public ReportDTO GetReportData(String startDate, String endDate)
        {
            return _newsArticleRepository.GetReportData(startDate, endDate);
        }

    }
}
