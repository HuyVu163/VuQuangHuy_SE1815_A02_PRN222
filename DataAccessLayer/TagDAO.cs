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
    public class TagDAO
    {
        public static void Add(Tag tag)
        {
            try
            {
                using (var context = new NmsContext())
                {
                    int count = context.Tags.Count();
                    tag.TagId = count + 1;
                    context.Tags.Add(tag);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Update(Tag tag)
        {
            try
            {
                using (var context = new NmsContext())
                {
                    context.Tags.Update(tag);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public static void Delete(Tag tag)
        {
            try
            {
                using (var context = new NmsContext())
                {
                    context.Tags.Remove(tag);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Tag GetTagById(int? id)
        {
            try
            {
                using (var context = new NmsContext())
                {
                    return context.Tags.Include(x => x.NewsArticles).FirstOrDefault(x => x.TagId == id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<Tag> GetAllTags()
        {
            try
            {
                using (var context = new NmsContext())
                {
                    return context.Tags.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<Tag> GetAllTagsWithNews()
        {
            try
            {
                using (var context = new NmsContext())
                {
                    var tags = context.Tags.Include(x => x.NewsArticles)
                            .Where(x => x.NewsArticles.Count == 0).ToList();
                    return tags;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
