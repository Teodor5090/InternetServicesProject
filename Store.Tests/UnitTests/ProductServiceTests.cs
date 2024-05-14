using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Store.API.Data;
using Store.API.DTOs;
using Store.API.Entities;
using Store.API.Interfaces;
using Store.API.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Store.Tests.UnitTests
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task GetAllProductsAsync_Returns_Products()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" }
            };

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(repo => repo.GetAllProductsAsync())
                                  .ReturnsAsync(products);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<List<ProductDto>>(It.IsAny<List<Product>>()))
                      .Returns((List<Product> p) => p.Select(x => new ProductDto { Id = x.Id, Name = x.Name }).ToList());

            var mockContext = new Mock<StoreDbContext>(new DbContextOptions<StoreDbContext>());

            var productService = new ProductService(mockContext.Object, mockMapper.Object);

            // Act
            var result = await productService.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(products.Count, result.Count);
        }
    }
}
