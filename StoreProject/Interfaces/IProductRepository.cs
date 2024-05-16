using Store.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.API.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task ImportStockAsync(List<Product> products);
        Task<List<Product>> GetProductsByIdsAsync(List<int> productIds);
    }
}
