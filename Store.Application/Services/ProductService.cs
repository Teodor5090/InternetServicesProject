using AutoMapper;
using Store.Application.Interfaces;
using Store.Domain.Entities;
using Store.A

namespace Store.Application.Services
{
    // ProductService.cs
    public class ProductService : IProductService
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var products = await _context.Products.Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).ToListAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).FirstOrDefaultAsync(p => p.Id == id);
            return product == null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            foreach (var categoryId in productDto.CategoryIds)
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category != null)
                {
                    product.ProductCategories.Add(new ProductCategory { Product = product, Category = category });
                }
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task UpdateProductAsync(ProductDto productDto)
        {
            var product = await _context.Products.Include(p => p.ProductCategories).FirstOrDefaultAsync(p => p.Id == productDto.Id);
            if (product != null)
            {
                _mapper.Map(productDto, product);

                // Update categories
                product.ProductCategories.Clear();
                foreach (var categoryId in productDto.CategoryIds)
                {
                    var category = await _context.Categories.FindAsync(categoryId);
                    if (category != null)
                    {
                        product.ProductCategories.Add(new ProductCategory { Product = product, Category = category });
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ImportStockAsync(List<ProductDto> products)
        {
            foreach (var productDto in products)
            {
                var product = await _context.Products.Include(p => p.ProductCategories).FirstOrDefaultAsync(p => p.Name == productDto.Name);
                if (product == null)
                {
                    product = _mapper.Map<Product>(productDto);
                    foreach (var categoryId in productDto.CategoryIds)
                    {
                        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                        if (category == null)
                        {
                            category = _mapper.Map<Category>(new CategoryDto { Id = categoryId });
                            _context.Categories.Add(category);
                        }
                        product.ProductCategories.Add(new ProductCategory { Product = product, Category = category });
                    }
                    _context.Products.Add(product);
                }
                else
                {
                    product.StockQuantity += productDto.StockQuantity;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<decimal> CalculateDiscountAsync(List<int> productIds)
        {
            var products = await _context.Products.Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).Where(p => productIds.Contains(p.Id)).ToListAsync();
            if (products.Count != productIds.Count)
            {
                throw new Exception("Invalid product IDs or insufficient stock.");
            }

            decimal totalDiscount = 0;

            var groupedByCategory = products.SelectMany(p => p.ProductCategories).GroupBy(pc => pc.Category.Name);
            foreach (var group in groupedByCategory)
            {
                if (group.Count() > 1)
                {
                    totalDiscount += group.First().Product.Price * 0.05m;
                }
            }

            return totalDiscount;
        }
    }

}
