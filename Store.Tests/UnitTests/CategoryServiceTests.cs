using AutoMapper;
using Moq;
using Store.API.Data;
using Store.API.DTOs;
using Store.API.Entities;
using Store.API.Interfaces;
using Store.API.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Store.Tests.UnitTests
{
    public class CategoryServiceTests
    {
        [Fact]
        public async Task GetAllCategoriesAsync_Returns_Categories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            };

            var mockDbSet = new Mock<DbSet<Category>>();
            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                     .ReturnsAsync((object[] ids) => categories.Find(c => c.Id == (int)ids[0]));

            var mockDbContext = new Mock<StoreDbContext>();
            mockDbContext.Setup(m => m.Categories).Returns(mockDbSet.Object);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<List<CategoryDto>>(It.IsAny<List<Category>>()))
                      .Returns((List<Category> source) => source.ConvertAll(c => new CategoryDto { Id = c.Id, Name = c.Name }));

            var service = new CategoryService(mockDbContext.Object, mockMapper.Object);

            // Act
            var result = await service.GetAllCategoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categories.Count, result.Count);
        }
    }
}
