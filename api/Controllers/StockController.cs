// Purpose: This file contains the StockController class which is responsible for handling all the requests related to the Stock model. It contains methods to get all stocks, get a stock by id, create a stock, update a stock and delete a stock.

using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepository;
        
        public StockController(ApplicationDBContext context, IStockRepository stockRepository)
        {
            _context = context;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStocks()
        {
            var stocks = await _stockRepository.GetAllStocks();
            var stocksDto = stocks.Select(stock => stock.ToStockDto());

            if(stocksDto.Count() == 0 || stocksDto == null)
            {
                return NotFound("No stocks found");
            }

            else return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStock([FromRoute]int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            else return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto stockDto)
        {
            var stock = stockDto.ToStockFromCreateDTO();

            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStock), new { id = stock.Id }, stock.ToStockDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto) {

            var stock = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);
            if (stock == null) {
                return NotFound();
            }

            stock.Symbol = updateDto.Symbol;
            stock.CompanyName = updateDto.CompanyName;
            stock.Purchase = updateDto.Purchase;
            stock.LastDiv = updateDto.LastDiv;
            stock.Industry = updateDto.Industry;
            stock.MarketCap = updateDto.MarketCap;

            _context.SaveChanges();

            return Ok(stock.ToStockDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);

            if (stockModel == null)
            {
                return NotFound();
            }

            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}