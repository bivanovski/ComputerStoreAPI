using ComputerStore.Service.DTOs;
using ComputerStore.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComputerStore.WebApi.Controllers
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
        public async Task<IActionResult> Calculate([FromBody] List<BasketItemDto> basket)
        {
            try
            {
                var result = await _discountService.CalculateDiscountAsync(basket);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
