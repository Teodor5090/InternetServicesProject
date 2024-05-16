using Microsoft.AspNetCore.Mvc;
using Store.API.Interfaces;
using System.Collections.Generic;
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
        public async Task<ActionResult<decimal>> CalculateDiscount([FromBody] List<int> productIds)
        {
            try
            {
                decimal discount = await _discountService.CalculateDiscountAsync(productIds);
                return Ok(discount);
            }
            catch (InvalidProductException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
