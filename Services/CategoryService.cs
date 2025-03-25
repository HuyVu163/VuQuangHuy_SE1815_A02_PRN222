using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Repositories;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService()
        {
            _categoryRepository = new CategoryRepository();
        }
        public void AddCategory(Category category)
        {
            _categoryRepository.AddCategory(category);
        }
        public void UpdateCategory(Category category)
        {
            _categoryRepository.UpdateCategory(category);
        }
        public void DeleteCategory(short? id)
        {
            _categoryRepository.DeleteCategory(id);
        }
        public IEnumerable<Category> GetAllCategories(String? status)
        {
            return _categoryRepository.GetAllCategories(status);
        }
        public Category GetCategoryById(short? id)
        {
            return _categoryRepository.GetCategoryById(id);
        }

    }
}
