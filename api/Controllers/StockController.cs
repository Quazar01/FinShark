using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController
    {
        private readonly ApplicationDBContext _context;
        
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllStocks()
        {
            var stocks = _context.Stocks.ToList();
            if (stocks.Count == 0)
            {
                return new NotFoundResult();
            }
            else if (stocks == null)
            {
                return new BadRequestResult();
            }
            else return new OkObjectResult(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetStock(int id)
        {
            var stock = _context.Stocks.Find(id);
            if (stock == null)
            {
                return new NotFoundResult();
            }
            else return new OkObjectResult(stock);
        }
    }
}