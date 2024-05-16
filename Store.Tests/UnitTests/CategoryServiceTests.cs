using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Store.API.Data;
using Store.API.DTOs;
using Store.API.Entities;
using Store.API.Interfaces;
using Store.API.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Store.Tests.UnitTests
{
    public class CategoryServiceTests
    {
        private readonly IMapper _mapper;

        public CategoryServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryDto>();
                cfg.CreateMap<CategoryDto, Category>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GetAllCategoriesAsync_Returns_Categories()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StoreDbContext>()
                .UseInMemoryDatabase(databaseName: "StoreDb")
                .Options;

            using (var context = new StoreDbContext(options))
            {
                context.Categories.AddRange(new List<Category>
                {
                    new Category { Id = 1, Name = "Category 1" },
                    new Category { Id = 2, Name = "Category 2" }
                });
                context.SaveChanges();
            }

            using (var context = new StoreDbContext(options))
            {
                var service = new CategoryService(context, _mapper);

                // Act
                var result = await service.GetAllCategoriesAsync();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.Equal("Category 1", result[0].Name);
                Assert.Equal("Category 2", result[1].Name);
            }
        }
    }
}
