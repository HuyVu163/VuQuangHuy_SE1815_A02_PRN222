using BusinessObjects.Models;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public void AddCategory(Category category) => CategoryDAO.AddCategory(category);
        public void DeleteCategory(short? id) => CategoryDAO.DeleteCategory(id);
        public IEnumerable<Category> GetAllCategories(String? status) => CategoryDAO.GetAllCategories(status);
        public Category GetCategoryById(short? id) => CategoryDAO.GetCategoryById(id);
        public void UpdateCategory(Category category) => CategoryDAO.UpdateCategory(category);

    }
}
