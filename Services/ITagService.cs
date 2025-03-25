using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Services
{
    public interface ITagService
    {
        void Add(Tag tag);
        void Delete(Tag tag);
        IEnumerable<Tag> GetAllTags();
        Tag GetTagById(int? id);
        void Update(Tag tag);
        IEnumerable<Tag> GetAllTagsWithNews();
    }
}
