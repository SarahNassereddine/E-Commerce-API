using lab3.Models;

namespace lab3.Services
{
    public interface IProductService
    {
        public Task<int> CreateProduct(CreateProductCmd cmd);

        public Task<DisplayableProduct> ReadProduct(int productId);
        public Task<List<DisplayableProductWithId>> ReadProducts();
        public Task UpdateProduct(UpdateProductCmd product, int productId);

        public Task DeleteProduct(int productId);


    }
}
