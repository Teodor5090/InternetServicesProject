using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.API.Data;
using Store.API.Entities;
using Store.API.Interfaces;

namespace Store.API.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly StoreDbContext _context;

        public DiscountService(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> CalculateDiscountAsync(List<int> productIds)
        {
            // Retrieve products from the database based on the provided product IDs
            var products = await _context.Products
                .Include(p => p.ProductCategories).ThenInclude(pc => pc.Category)
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            // Validate if all product IDs are valid and in stock
            ValidateProductIds(productIds, products);

            // Calculate and return the discount
            return CalculateDiscount(products);
        }

        private void ValidateProductIds(List<int> productIds, List<Product> products)
        {
            // Check if the number of retrieved products matches the number of provided product IDs
            if (products.Count != productIds.Count)
            {
                throw new InvalidProductException("Invalid product IDs or insufficient stock.");
            }
        }

        private decimal CalculateDiscount(List<Product> products)
        {
            decimal totalDiscount = 0;
            var groupedByCategory = products.SelectMany(p => p.ProductCategories)
                .GroupBy(pc => pc.CategoryId);

            foreach (var group in groupedByCategory)
            {
                if (group.Count() > 1)
                {
                    // Apply discount for products of the same category
                    var product = group.First().Product;
                    totalDiscount += product.Price * 0.05m;
                }
            }

            return totalDiscount;
        }
    }
}
