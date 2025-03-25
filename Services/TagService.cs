using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Repositories;

namespace Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        public TagService()
        {
            _tagRepository = new TagRepository();
        }
        public void Add(Tag tag)
        {
            _tagRepository.Add(tag);
        }
        public void Delete(Tag tag)
        {
            _tagRepository.Delete(tag);
        }
        public IEnumerable<Tag> GetAllTags()
        {
            return _tagRepository.GetAllTags();
        }
        public Tag GetTagById(int? id)
        {
            return _tagRepository.GetTagById(id);
        }
        public void Update(Tag tag)
        {
            _tagRepository.Update(tag);
        }
        public IEnumerable<Tag> GetAllTagsWithNews()
        {
            return _tagRepository.GetAllTagsWithNews();
        }
    }
}
