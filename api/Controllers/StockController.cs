using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllStocks()
        {
            var stocks = _context.Stocks.ToList()
            .Select(stock => stock.ToStockDto());
            if(stocks.Count() == 0 || stocks == null)
            {
                return NotFound();
            }

            else return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetStock([FromRoute]int id)
        {
            var stock = _context.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();
            }
            else return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public IActionResult CreateStock([FromBody] CreateStockRequestDto stockDto)
        {
            var stock = stockDto.ToStockFromCreateDTO();

            _context.Stocks.Add(stock);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetStock), new { id = stock.Id }, stock.ToStockDto());
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto) {

            var stock = _context.Stocks.FirstOrDefault(stock => stock.Id == id);
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
        public IActionResult DeleteStock([FromRoute] int id)
        {
            var stockModel = _context.Stocks.FirstOrDefault(stock => stock.Id == id);

            if (stockModel == null)
            {
                return NotFound();
            }

            _context.Stocks.Remove(stockModel);
            _context.SaveChanges();

            return NoContent();
        }
    }
}