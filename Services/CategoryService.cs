using System.Collections.Generic;
using Z01.Repositories;

namespace Z01.services
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepository = new CategoryRepository();

        public IEnumerable<string> GetAllCategories()
        {
            return _categoryRepository.GetAllCategories();
        }
    }
}
