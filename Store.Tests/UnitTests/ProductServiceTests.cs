using AutoMapper;
using Moq;
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
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper>();
            _productService = new ProductService(_mockProductRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllProductsAsync_Returns_Products()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" }
            };

            _mockProductRepository.Setup(repo => repo.GetAllProductsAsync())
                                  .ReturnsAsync(products);

            _mockMapper.Setup(mapper => mapper.Map<List<ProductDto>>(It.IsAny<List<Product>>()))
                      .Returns((List<Product> p) => p.Select(x => new ProductDto { Id = x.Id, Name = x.Name }).ToList());

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(products.Count, result.Count);
            Assert.Equal("Product 1", result[0].Name);
            Assert.Equal("Product 2", result[1].Name);
        }

        [Fact]
        public async Task GetProductByIdAsync_Returns_Product()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1" };

            _mockProductRepository.Setup(repo => repo.GetProductByIdAsync(1))
                                  .ReturnsAsync(product);

            _mockMapper.Setup(mapper => mapper.Map<ProductDto>(It.IsAny<Product>()))
                      .Returns((Product p) => new ProductDto { Id = p.Id, Name = p.Name });

            // Act
            var result = await _productService.GetProductByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Name, result.Name);
        }

        [Fact]
        public async Task CreateProductAsync_Returns_CreatedProduct()
        {
            // Arrange
            var productDto = new ProductDto { Id = 1, Name = "Product 1" };
            var product = new Product { Id = 1, Name = "Product 1" };

            _mockMapper.Setup(mapper => mapper.Map<Product>(It.IsAny<ProductDto>()))
                      .Returns(product);

            _mockProductRepository.Setup(repo => repo.CreateProductAsync(It.IsAny<Product>()))
                                  .ReturnsAsync(product);

            _mockMapper.Setup(mapper => mapper.Map<ProductDto>(It.IsAny<Product>()))
                      .Returns(productDto);

            // Act
            var result = await _productService.CreateProductAsync(productDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productDto.Name, result.Name);
        }

        [Fact]
        public async Task UpdateProductAsync_Calls_UpdateProduct()
        {
            // Arrange
            var productDto = new ProductDto { Id = 1, Name = "Product 1" };
            var product = new Product { Id = 1, Name = "Product 1" };

            _mockMapper.Setup(mapper => mapper.Map<Product>(It.IsAny<ProductDto>()))
                      .Returns(product);

            _mockProductRepository.Setup(repo => repo.UpdateProductAsync(It.IsAny<Product>()));

            // Act
            await _productService.UpdateProductAsync(productDto);

            // Assert
            _mockProductRepository.Verify(repo => repo.UpdateProductAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_Calls_DeleteProduct()
        {
            // Arrange
            var productId = 1;

            _mockProductRepository.Setup(repo => repo.DeleteProductAsync(productId));

            // Act
            await _productService.DeleteProductAsync(productId);

            // Assert
            _mockProductRepository.Verify(repo => repo.DeleteProductAsync(productId), Times.Once);
        }
    }
}
