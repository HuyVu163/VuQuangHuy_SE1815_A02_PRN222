using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories(String? status);
        Category GetCategoryById(short? id);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(short? id);
    }
}
