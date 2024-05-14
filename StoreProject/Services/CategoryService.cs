using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Store.API.Data;
using Store.API.DTOs;
using Store.API.Entities;
using Store.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services
{
    // CategoryService.cs
    public class CategoryService : ICategoryService
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;
        private ICategoryRepository object1;
        private IMapper object2;

        public CategoryService(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

    

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.Include(c => c.ProductCategories).ToListAsync();
            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.Include(c => c.ProductCategories).FirstOrDefaultAsync(c => c.Id == id);
            return category == null ? null : _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task UpdateCategoryAsync(CategoryDto categoryDto)
        {
            var category = await _context.Categories.Include(c => c.ProductCategories).FirstOrDefaultAsync(c => c.Id == categoryDto.Id);
            if (category != null)
            {
                _mapper.Map(categoryDto, category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }

}
