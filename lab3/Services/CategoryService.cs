using lab3.Data;
using lab3.Models;
using Microsoft.EntityFrameworkCore;

namespace lab3.Services
{
    public class CategoryService : ICategoryService
    {
        readonly AppDbContext _context;
        readonly ILogger _logger;
        public CategoryService(AppDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<CategoryService>();
        }

        public async Task<int> CreateCategory(CreateCategoryCmd cmd)
        {
          
            var category = cmd.toCategory();
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.CategoryId;
        }

        public async Task DeleteCategory(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category is not null) { 
            category.IsDeleted = true;
            await _context.SaveChangesAsync();
            }
        }

        public async Task<List<DisplayableCategoryWithId>> ReadCategories()
        {
            return await _context.Categories.
                Where(c => !c.IsDeleted)
                .Select(c => new DisplayableCategoryWithId
                {
               
                    CategoryId = c.CategoryId,
                    Name = c.Name


                }).ToListAsync();
          
        }

        public async Task<DisplayableCategory?> ReadCategory(int categoryId)
        {
            return await _context.Categories.Where(c => c.CategoryId == categoryId).Where(c => c.IsDeleted == false).Select(c => new DisplayableCategory
            {
                Name = c.Name,
              

            }).SingleOrDefaultAsync();

        }

        public async Task UpdateCategory(UpdateCategoryCmd cmd, int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null || category.IsDeleted) { throw new KeyNotFoundException("Unable to find the category"); }
            cmd.UpdateCategory(category);
            await _context.SaveChangesAsync();
        }
    }
}
