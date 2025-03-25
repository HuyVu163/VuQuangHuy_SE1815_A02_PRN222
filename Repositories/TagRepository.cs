using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Repositories
{
    public class TagRepository : ITagRepository
    {
        public void Add(Tag tag) => TagDAO.Add(tag);
        public void Delete(Tag tag) => TagDAO.Delete(tag);
        public IEnumerable<Tag> GetAllTags() => TagDAO.GetAllTags();
        public Tag GetTagById(int? id) => TagDAO.GetTagById(id);
        public void Update(Tag tag) => TagDAO.Update(tag);
        public IEnumerable<Tag> GetAllTagsWithNews() => TagDAO.GetAllTagsWithNews();
    }
}
