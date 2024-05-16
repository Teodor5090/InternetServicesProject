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
          
            var products = await _context.Products
                .Include(p => p.ProductCategories).ThenInclude(pc => pc.Category)
                .Where(p => productIds.Distinct().Contains(p.Id))
                .ToListAsync();

            ValidateProductIds(productIds, products);

            return CalculateDiscount(products, productIds);
        }

        private void ValidateProductIds(List<int> productIds, List<Product> products)
        {
      
            if (products.Count != productIds.Distinct().Count())
            {
                throw new InvalidProductException("Invalid product IDs or insufficient stock.");
            }
        }

        private decimal CalculateDiscount(List<Product> products, List<int> productIds)
        {
            decimal totalDiscount = 0;

            var productCount = productIds.GroupBy(id => id)
                                         .ToDictionary(g => g.Key, g => g.Count());

            foreach (var product in products)
            {
                if (productCount[product.Id] > 1)
                {
                    totalDiscount += product.Price * 0.05m * productCount[product.Id];
                }
            }

            return totalDiscount;
        }
    }
}
