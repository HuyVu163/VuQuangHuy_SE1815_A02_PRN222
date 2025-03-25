using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories
{
    public interface INewsArticleRepository
    {
        IEnumerable<NewsArticle> GetAllNewsArticles(String? status);
        IEnumerable<NewsArticle> GetAllNewsArticlesByStaff(short createBy);
        NewsArticle GetNewsArticleById(String? id);
        NewsArticle AddNewsArticle(NewsArticle newsArticle);
        void UpdateNewsArticle(NewsArticle newsArticle);
        void DeleteNewsArticle(String? id);
        IEnumerable<NewsArticle> GetNewsByPaging(String? strSearch,int? pageNumber, int? pageSize, short? categoryId);
        ReportDTO GetReportData(String startDate, String endDate);
    }
}
