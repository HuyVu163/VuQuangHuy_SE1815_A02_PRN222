using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace DataAccessLayer
{
    public class CategoryDAO
    {
        public static IEnumerable<Category> GetAllCategories(String? status)
        {
            try
            {
                if(status == "all")
                {
                    using (var context = new NmsContext())
                    {
                        return context.Categories.ToList();
                    }
                }
                else
                {
                    using (var context = new NmsContext())
                    {
                        return context.Categories.Where(c => c.IsActive == true).ToList();
                    }
                }
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Category GetCategoryById(short? id)
        {
            try
            {
                using (var context = new NmsContext())
                {
                    return context.Categories.FirstOrDefault(c => c.CategoryId == id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void AddCategory(Category category)
        {
            try
            {
                using (var context = new NmsContext())
                {
                    context.Categories.Add(category);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void UpdateCategory(Category category)
        {
            try
            {
                using (var context = new NmsContext())
                {
                    context.Categories.Update(category);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void DeleteCategory(short? id)
        {
            try
            {
                using (var context = new NmsContext())
                {
                    var news = context.NewsArticles.Where(n => n.CategoryId == id).ToList();
                    var categoryParent = context.Categories.Where(c => c.ParentCategoryId == id).ToList();
                    string categoryParentName = "";
                    if (categoryParent.Count > 0)
                    {
                        foreach (var item in categoryParent)
                        {
                            categoryParentName += item.CategoryName + " ";
                        }
                        throw new Exception("This category is being used as a parent category in " + categoryParentName + "categories");
                    }
                    else
                    if (news.Count > 0)
                    {
                        throw new Exception("This category is being used in news articles");
                    }
                    else
                    {
                        var category = context.Categories.Find(id);
                        context.Categories.Remove(category);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
