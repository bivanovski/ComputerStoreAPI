using ComputerStore.Service.DTOs;
using ComputerStore.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComputerStore.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockImportService _stockImportService;

        public StockController(IStockImportService stockImportService)
        {
            _stockImportService = stockImportService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportStock([FromBody] List<ImportStockDto> stockDtos)
        {
            if (stockDtos == null || stockDtos.Count == 0)
                return BadRequest("Stock list is empty.");

            await _stockImportService.ImportAsync(stockDtos);
            return Ok("Stock imported successfully.");
        }
    }
}
