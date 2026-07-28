using lab3.Data;
using lab3.Models;
using Microsoft.EntityFrameworkCore;

namespace lab3.Services
{
    public class ProductService : IProductService
    {
        AppDbContext _context;
        ILogger _logger;

        public ProductService(AppDbContext context, ILoggerFactory factory)
        {
            _context= context;
            _logger= factory.CreateLogger<ProductService>();
        }
        public async Task<int> CreateProduct(CreateProductCmd cmd)
        {
            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == cmd.CategoryId);
            if (!categoryExists)
            {
                throw new KeyNotFoundException(" category dosn't exist"); 
            }
            var product= cmd.toProduct();

           _context.Products.Add(product);
         await  _context.SaveChangesAsync();
            return product.ProductId;
        }

        public async Task DeleteProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product is not null)
            {
                product.IsDeleted = true;
                await _context.SaveChangesAsync();
            }

        }

        public async  Task<DisplayableProduct?> ReadProduct(int productId)
        {
            return await _context.Products.Where(p => p.ProductId == productId).Where(p => !p.IsDeleted).Select(p => new DisplayableProduct
            {
                Name = p.Name,
                Price = p.Price,
                Description = p.Description ?? "",
                CategoryId = p.CategoryId,



            }).SingleOrDefaultAsync();
        }

        public async Task<List<DisplayableProductWithId>> ReadProducts()
        {
            return await _context.Products
                   .Where(p => !p.IsDeleted)
                   .Select(p => new DisplayableProductWithId
                   {

                      Name= p.Name,
                      Price= p.Price,
                      Description= p.Description ?? "",
                      CategoryId= p.CategoryId,
                      ProductId= p.ProductId


                   }).ToListAsync();

        }

        public async Task UpdateProduct(UpdateProductCmd cmd, int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product is null)
            {
                throw new KeyNotFoundException("product not found");
            }
            cmd.UpdateProduct(product);
            await _context.SaveChangesAsync();

        }
    }
}
