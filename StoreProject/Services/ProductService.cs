using AutoMapper;
using Store.API.DTOs;
using Store.API.Entities;
using Store.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            return product == null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            var createdProduct = await _productRepository.CreateProductAsync(product);
            return _mapper.Map<ProductDto>(createdProduct);
        }

        public async Task UpdateProductAsync(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            await _productRepository.UpdateProductAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteProductAsync(id);
        }

        public async Task ImportStockAsync(List<ProductDto> products)
        {
            var mappedProducts = _mapper.Map<List<Product>>(products);
            await _productRepository.ImportStockAsync(mappedProducts);
        }

        public async Task<decimal> CalculateDiscountAsync(List<int> productIds)
        {
            var products = await _productRepository.GetProductsByIdsAsync(productIds);
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
