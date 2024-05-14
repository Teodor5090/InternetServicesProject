using Microsoft.AspNetCore.Mvc;
using Store.API.Interfaces;
using System.Threading.Tasks;

namespace Store.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpPost("calculate")]
        public async Task<ActionResult<decimal>> CalculateDiscount(PurchaseRequestDto purchaseRequest)
        {
            try
            {
                decimal discount = await _discountService.CalculateDiscountAsync(purchaseRequest.ProductIds);
                return Ok(discount);
            }
            catch (InvalidProductException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
