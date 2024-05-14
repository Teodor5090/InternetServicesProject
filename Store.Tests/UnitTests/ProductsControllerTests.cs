using Microsoft.AspNetCore.Mvc;
using Moq;
using Store.API.Controllers;
using Store.API.DTOs;
using Store.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Tests.UnitTests
{
    public class ProductsControllerTests
    {
        [Fact]
        public async Task GetAllProducts_Returns_OkObjectResult_With_Products()
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();
            mockProductService.Setup(service => service.GetAllProductsAsync())
                              .ReturnsAsync(new List<ProductDto>
                              {
                                  new ProductDto { Id = 1, Name = "Product 1" },
                                  new ProductDto { Id = 2, Name = "Product 2" }
                              });
            var controller = new ProductsController(mockProductService.Object);

            // Act
            var result = await controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var products = Assert.IsAssignableFrom<List<ProductDto>>(okResult.Value);
            Assert.Equal(2, products.Count);
        }
    }
}
