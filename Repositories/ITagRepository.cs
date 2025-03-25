using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories
{
    public interface ITagRepository
    {
        void Add(Tag tag);
        void Update(Tag tag);
        void Delete(Tag tag);
        Tag GetTagById(int? id);
        IEnumerable<Tag> GetAllTags();
        IEnumerable<Tag> GetAllTagsWithNews();
    }
}
