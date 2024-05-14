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
    public class  CategoriesControllerTests
    {
        [Fact]
        public async Task GetAllCategories_Returns_OkObjectResult_With_Categories()
        {
            // Arrange
            var mockCategoryService = new Mock<ICategoryService>();
            mockCategoryService.Setup(service => service.GetAllCategoriesAsync())
                               .ReturnsAsync(new List<CategoryDto>
                               {
                                   new CategoryDto { Id = 1, Name = "Category 1" },
                                   new CategoryDto { Id = 2, Name = "Category 2" }
                               });
            var controller = new CategoriesController(mockCategoryService.Object);

            // Act
            var result = await controller.GetAllCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var categories = Assert.IsAssignableFrom<List<CategoryDto>>(okResult.Value);
            Assert.Equal(2, categories.Count);
        }
    }



}
