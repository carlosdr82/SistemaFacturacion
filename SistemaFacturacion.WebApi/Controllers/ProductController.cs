using Dapper;
using Microsoft.AspNetCore.Mvc;
using SistemaFacturacion.WebApi.Model;
using System.Data;
using System.Data.SqlClient;

namespace SistemaFacturacion.WebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductController : ControllerBase
    {

        private readonly IDbConnection _dbConnection;
        public ProductController(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _dbConnection.QueryAsync<Product>("SELECT * FROM Products");

            return Ok(products);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _dbConnection.QueryFirstOrDefaultAsync<Product>(
                "SELECT * FROM Products WHERE Id = @Id", new { Id = id });

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var sql = @"INSERT INTO Products (Name, Description, Price, StockQuantity) 
                    VALUES (@Name, @Description, @Price, @StockQuantity);
                    SELECT CAST(SCOPE_IDENTITY() as int)";

            var id = await _dbConnection.QuerySingleAsync<int>(sql, product);
            product.Id = id;

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            product.Id = id;
            var sql = @"UPDATE Products 
                        SET Name = @Name, Description = @Description, 
                        Price = @Price, StockQuantity = @StockQuantity 
                        WHERE Id = @Id";

            var affected = await _dbConnection.ExecuteAsync(sql, product);

            if (affected == 0)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var affected = await _dbConnection.ExecuteAsync("DELETE FROM Products WHERE Id = @Id", new { Id = id });

            if (affected == 0)
                return NotFound();

            return NoContent();
        }
    }
}
