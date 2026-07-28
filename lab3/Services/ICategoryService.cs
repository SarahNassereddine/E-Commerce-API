using lab3.Models;

namespace lab3.Services
{
    public interface ICategoryService
    {
        public Task<int> CreateCategory(CreateCategoryCmd cmd);

        public Task<DisplayableCategory> ReadCategory(int categoryId);
        public Task<List<DisplayableCategoryWithId>> ReadCategories();
        public Task UpdateCategory(UpdateCategoryCmd cmd, int categoryId);

        public Task DeleteCategory(int categoryId);
    }
}
