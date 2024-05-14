using System;

namespace Store.API.Interfaces
{
    public interface IDiscountService
    {
        Task<decimal> CalculateDiscountAsync(List<int> productIds);
    }

    public class InvalidProductException : Exception
    {
        public InvalidProductException(string message) : base(message)
        {
        }
    }
}
