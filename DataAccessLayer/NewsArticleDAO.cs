using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccessLayer
{
    public class NewsArticleDAO
    {
        public static IEnumerable<NewsArticle> GetAllNewsArticles(String? status)
        {
            try
            {
                using (var db = new NmsContext())
                {
                    if (status == "all")
                    {
                        var news = from n in db.NewsArticles
                                   join c in db.Categories on n.CategoryId equals c.CategoryId
                                   join a in db.SystemAccounts on n.CreatedBy.AccountId equals a.AccountId
                                   join u in db.SystemAccounts on n.UpdatedById equals u.AccountId into updatebyJoin
                                   from u in updatebyJoin.DefaultIfEmpty()
                                   select new NewsArticle
                                   {
                                       NewsArticleId = n.NewsArticleId,
                                       NewsTitle = n.NewsTitle,
                                       Headline = n.Headline,
                                       CreatedDate = n.CreatedDate,
                                       NewsContent = n.NewsContent,
                                       NewsSource = n.NewsSource,
                                       Category = n.Category,
                                       CreatedBy = n.CreatedBy,
                                       NewsStatus = n.NewsStatus,
                                       CreatedById = n.CreatedById,
                                       ModifiedDate = n.ModifiedDate,
                                       UpdatedById = n.UpdatedById,
                                       Tags = n.Tags.Where(x => x.NewsArticles.Any(y => y.NewsArticleId == n.NewsArticleId)).ToList()
                                   };
                        return news.OrderByDescending(c => c.NewsArticleId).ToList();
                    }
                    else
                    {
                        var news = from n in db.NewsArticles
                                   join c in db.Categories on n.CategoryId equals c.CategoryId
                                   join a in db.SystemAccounts on n.CreatedBy.AccountId equals a.AccountId
                                   join u in db.SystemAccounts on n.UpdatedById equals u.AccountId into updatebyJoin
                                   from u in updatebyJoin.DefaultIfEmpty()
                                   where n.NewsStatus == true
                                   select new NewsArticle
                                   {
                                       NewsArticleId = n.NewsArticleId,
                                       NewsTitle = n.NewsTitle,
                                       Headline = n.Headline,
                                       CreatedDate = n.CreatedDate,
                                       NewsContent = n.NewsContent,
                                       NewsSource = n.NewsSource,
                                       Category = n.Category,
                                       CreatedBy = n.CreatedBy,
                                       NewsStatus = n.NewsStatus,
                                       CreatedById = n.CreatedById,
                                       ModifiedDate = n.ModifiedDate,
                                       UpdatedById = n.UpdatedById,
                                       Tags = n.Tags.Where(x => x.NewsArticles.Any(y => y.NewsArticleId == n.NewsArticleId)).ToList()
                                   };
                        return news.OrderByDescending(c => c.NewsArticleId).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static IEnumerable<NewsArticle> GetAllNewsArticlesByStaff(short CreateBy)
        {
            try
            {
                using (var db = new NmsContext())
                {
                        var news = from n in db.NewsArticles
                                   join c in db.Categories on n.CategoryId equals c.CategoryId
                                   join a in db.SystemAccounts on n.CreatedBy.AccountId equals a.AccountId
                                   join u in db.SystemAccounts on n.UpdatedById equals u.AccountId into updatebyJoin
                                   from u in updatebyJoin.DefaultIfEmpty()
                                   select new NewsArticle
                                   {
                                       NewsArticleId = n.NewsArticleId,
                                       NewsTitle = n.NewsTitle,
                                       Headline = n.Headline,
                                       CreatedDate = n.CreatedDate,
                                       NewsContent = n.NewsContent,
                                       NewsSource = n.NewsSource,
                                       Category = n.Category,
                                       CreatedBy = n.CreatedBy,
                                       NewsStatus = n.NewsStatus,
                                       CreatedById = n.CreatedById,
                                       ModifiedDate = n.ModifiedDate,
                                       UpdatedById = n.UpdatedById,
                                       Tags = n.Tags.Where(x => x.NewsArticles.Any(y => y.NewsArticleId == n.NewsArticleId)).ToList()
                                   };
                        return news.OrderByDescending(c => c.NewsArticleId).Where(x => x.CreatedBy.AccountId == CreateBy).ToList();
                    }
                   
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static NewsArticle GetNewsArticleById(String? id)
        {
            try
            {
                using (var db = new NmsContext())
                {
                    var news = from n in db.NewsArticles
                               join c in db.Categories on n.CategoryId equals c.CategoryId
                               join a in db.SystemAccounts on n.CreatedBy.AccountId equals a.AccountId
                               join u in db.SystemAccounts on n.UpdatedById equals u.AccountId into updatebyJoin
                               from u in updatebyJoin.DefaultIfEmpty()
                               where n.NewsArticleId == id 
                               select new NewsArticle
                               {
                                   NewsArticleId = n.NewsArticleId,
                                   NewsTitle = n.NewsTitle,
                                   Headline = n.Headline,
                                   CreatedDate = n.CreatedDate,
                                   NewsContent = n.NewsContent,
                                   NewsSource = n.NewsSource,
                                   CategoryId = n.CategoryId,
                                   Category = n.Category,
                                   CreatedBy = n.CreatedBy,
                                   NewsStatus = n.NewsStatus,
                                   CreatedById = n.CreatedById,
                                   ModifiedDate = n.ModifiedDate,
                                   UpdatedById = n.UpdatedById,
                                   Tags = n.Tags.Where(x => x.NewsArticles.Any(y => y.NewsArticleId == n.NewsArticleId)).ToList()
                               };
                    return news.FirstOrDefault();
                }

            }catch
            (Exception ex)
            {
                throw ex;
            }
        }
        public static void AddNewsArticle(NewsArticle newsArticle)
        {
            try
            {
                using (var db = new NmsContext())
                {
                    var category = db.Categories.FirstOrDefault(x => x.CategoryId == newsArticle.CategoryId);
                    if(category == null)
                    {
                        throw new Exception("Category not found");
                    }
                    var createdBy = db.SystemAccounts.FirstOrDefault(x => x.AccountId == newsArticle.CreatedById);
                    if (createdBy == null)
                    {
                        throw new Exception("User not found");
                    }
                    newsArticle.CreatedBy = createdBy;
                    //Check a news article will have many tags and one tag will belong to zero or one news article 
                    foreach (var tag in newsArticle.Tags)
                    {
                        var tagInDb = db.Tags.Include(x => x.NewsArticles).FirstOrDefault(x => x.TagId == tag.TagId);
                        if (tagInDb == null)
                        {
                            throw new Exception("Tag not found");
                        }
                        if (tagInDb.NewsArticles.Count()>0)
                        {
                            throw new Exception("Tag already assigned to another news article");
                        }
                    }
                    if (newsArticle != null)
                    {
                        // Lấy danh sách TagId từ JSON
                        var tagIds = newsArticle.Tags.Select(t => t.TagId).ToList();

                        // Lấy các Tag đã có trong database
                        var existingTags = db.Tags.Where(t => tagIds.Contains(t.TagId)).ToList();

                        // Tạo danh sách Tags mới để liên kết với NewsArticle
                        var finalTags = new List<Tag>();

                        foreach (var tag in newsArticle.Tags)
                        {
                            var existingTag = existingTags.FirstOrDefault(t => t.TagId == tag.TagId);

                            if (existingTag != null)
                            {
                                // Nếu Tag đã có, sử dụng instance có sẵn từ DbContext
                                finalTags.Add(existingTag);
                            }
                            else
                            {
                                // Nếu Tag chưa tồn tại, thêm mới
                                finalTags.Add(new Tag { TagId = tag.TagId, TagName = tag.TagName });
                            }
                        }

                        // Gán danh sách Tags đã xử lý vào model.NewsArticle
                        newsArticle.Tags = finalTags;

                       
                    }
                    String newsArticleId = "News" + db.NewsArticles.Count();
                    newsArticle.NewsArticleId = newsArticleId;
                       
                    db.NewsArticles.Add(newsArticle);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void UpdateNewsArticle(NewsArticle newsArticle)
        {
            try
            {
                using (var db = new NmsContext())
                {
                    // Tìm bài viết hiện tại trong database
                    var existingArticle = db.NewsArticles
                        .Include(a => a.Tags)
                        .FirstOrDefault(a => a.NewsArticleId == newsArticle.NewsArticleId);

                    if (existingArticle == null)
                    {
                        throw new Exception("News article not found");
                    }

                    var category = db.Categories.FirstOrDefault(x => x.CategoryId == newsArticle.CategoryId);
                    if (category == null)
                    {
                        throw new Exception("Category not found");
                    }

                    // Lấy danh sách TagId từ `newsArticle`
                    var newTagIds = newsArticle.Tags.Select(t => t.TagId).ToList();

                    // Lấy danh sách tag từ database theo danh sách mới
                    var newTags = db.Tags.Where(t => newTagIds.Contains(t.TagId)).ToList();

                    // **Xóa các Tags không còn tồn tại trong danh sách mới**
                    var tagsToRemove = existingArticle.Tags
                        .Where(t => !newTagIds.Contains(t.TagId))
                        .ToList(); // ToList() để tránh lỗi sửa đổi danh sách trong vòng lặp

                    foreach (var tag in tagsToRemove)
                    {
                        existingArticle.Tags.Remove(tag);
                    }

                    // **Thêm các Tags mới nếu chưa tồn tại**
                    foreach (var tag in newTags)
                    {
                        if (!existingArticle.Tags.Any(t => t.TagId == tag.TagId))
                        {
                            existingArticle.Tags.Add(tag);
                        }
                    }

                    // Cập nhật thông tin bài báo
                    existingArticle.NewsTitle = newsArticle.NewsTitle;
                    existingArticle.NewsContent = newsArticle.NewsContent;
                    existingArticle.CategoryId = newsArticle.CategoryId;
                    existingArticle.Headline = newsArticle.Headline;
                    existingArticle.NewsSource = newsArticle.NewsSource;
                    existingArticle.NewsStatus = newsArticle.NewsStatus;
                    existingArticle.ModifiedDate = DateTime.Now;
                    existingArticle.UpdatedById = newsArticle.UpdatedById;
                    db.Update(existingArticle);

                    // Lưu thay đổi vào database
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating news article: {ex.Message}");
            }
        }


        public static void DeleteNewsArticle(String? id)
        {
            try
            {
                using (var db = new NmsContext())
                {
                    var newsArticle = db.NewsArticles
                                        .Include(x => x.Tags)
                                        .FirstOrDefault(x => x.NewsArticleId == id);

                    if (newsArticle != null)
                    {
                        // Xóa tất cả các bản ghi trong bảng trung gian
                        newsArticle.Tags.Clear();
                        db.SaveChanges();

                        // Sau đó xóa newsArticle
                        db.NewsArticles.Remove(newsArticle);
                        db.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<NewsArticle> GetNewsByPaging(string? strSearch, int? pageNumber, int? pageSize, short? categoryId)
        {
            try
            {
                using (var db = new NmsContext())
                {
                    if (strSearch == null)
                    {
                        // LinQ

                        var news = from n in db.NewsArticles
                                   join c in db.Categories on n.CategoryId equals c.CategoryId
                                   join a in db.SystemAccounts on n.CreatedBy.AccountId equals a.AccountId
                                   join u in db.SystemAccounts on n.UpdatedById equals u.AccountId into updatebyJoin
                                   from u in updatebyJoin.DefaultIfEmpty()
                                   where (n.CategoryId == categoryId || categoryId == null) && n.NewsStatus == true
                                   select new NewsArticle
                                   {
                                       NewsArticleId = n.NewsArticleId,
                                       NewsTitle = n.NewsTitle,
                                       Headline = n.Headline,
                                       CreatedDate = n.CreatedDate,
                                       NewsContent = n.NewsContent,
                                       NewsSource = n.NewsSource,
                                       Category = n.Category,
                                       CreatedBy = n.CreatedBy,
                                       NewsStatus = n.NewsStatus,
                                       CreatedById = n.CreatedById,
                                       ModifiedDate = n.ModifiedDate,
                                       UpdatedById = n.UpdatedById,
                                       Tags = n.Tags.Where(x => x.NewsArticles.Any(y => y.NewsArticleId == n.NewsArticleId)).ToList()
                                   }
                                   ;
                        var newsList = news
                                .OrderByDescending(c => c.NewsArticleId)
                                .Skip((pageNumber.Value - 1) * pageSize.Value)
                                .Take(pageSize.Value)
                                .ToList();  // Materialize the query before returning

                        return newsList;
                    }
                    else
                    {
                        var news = from n in db.NewsArticles
                                   join c in db.Categories on n.CategoryId equals c.CategoryId
                                   join a in db.SystemAccounts on n.CreatedBy.AccountId equals a.AccountId
                                   join u in db.SystemAccounts on n.UpdatedById equals u.AccountId into updatebyJoin
                                   from u in updatebyJoin.DefaultIfEmpty()
                                   where n.NewsTitle.Contains(strSearch) && (n.CategoryId == categoryId || categoryId == null) && n.NewsStatus == true
                                   select new NewsArticle
                                   {
                                       NewsArticleId = n.NewsArticleId,
                                       NewsTitle = n.NewsTitle,
                                       Headline = n.Headline,
                                       CreatedDate = n.CreatedDate,
                                       NewsContent = n.NewsContent,
                                       NewsSource = n.NewsSource,
                                       Category = n.Category,
                                       CreatedBy = n.CreatedBy,
                                       NewsStatus = n.NewsStatus,
                                       CreatedById = n.CreatedById,
                                       ModifiedDate = n.ModifiedDate,
                                       UpdatedById = n.UpdatedById,
                                       Tags = n.Tags.Where(x => x.NewsArticles.Any(y => y.NewsArticleId == n.NewsArticleId)).ToList()
                                   };
                        var newsList = news
                                .OrderByDescending(c => c.NewsArticleId)
                                .Skip((pageNumber.Value - 1) * pageSize.Value)
                                .Take(pageSize.Value)
                                .ToList();  // Materialize the query before returning

                        return newsList;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static ReportDTO GetReport(String dateStart, String dateEnd)
        {
            try
            {
                using (var db = new NmsContext())
                {

                    var report = new ReportDTO();
                    var news = from n in db.NewsArticles
                               join c in db.Categories on n.CategoryId equals c.CategoryId
                               join a in db.SystemAccounts on n.CreatedBy.AccountId equals a.AccountId
                               join u in db.SystemAccounts on n.UpdatedById equals u.AccountId into updatebyJoin
                               from u in updatebyJoin.DefaultIfEmpty()
                               where n.CreatedDate >= DateTime.Parse(dateStart) && n.CreatedDate <= DateTime.Parse(dateEnd)
                               orderby n.CreatedDate descending
                               select new NewsArticle
                               {
                                   NewsArticleId = n.NewsArticleId,
                                   NewsTitle = n.NewsTitle,
                                   Headline = n.Headline,
                                   CreatedDate = n.CreatedDate,
                                   NewsContent = n.NewsContent,
                                   NewsSource = n.NewsSource,
                                   Category = n.Category,
                                   CreatedBy = n.CreatedBy,
                                   NewsStatus = n.NewsStatus,
                                   CreatedById = n.CreatedById,
                                   ModifiedDate = n.ModifiedDate,
                                   UpdatedById = n.UpdatedById,
                                   Tags = n.Tags.Where(x => x.NewsArticles.Any(y => y.NewsArticleId == n.NewsArticleId)).ToList()
                               };
                    report.totalNews = news.Count();
                    int totalTag = 0;
                    foreach (var item in news)
                    {
                        totalTag += item.Tags.Count();
                    }
                    report.totalTag = totalTag;
                    report.totalCategory = news.Count();
                    report.totalAuthor = news.Select(x => x.CreatedBy).Distinct().Count();
                    report.mostCategory = news.GroupBy(x => x.Category.CategoryName).OrderByDescending(x => x.Count()).Select(x => x.Key).FirstOrDefault();
                    return report;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
