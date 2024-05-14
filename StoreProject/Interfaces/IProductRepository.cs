// IProductRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Store.API.Entities;


namespace Store.API.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}