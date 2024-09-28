using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using SistemaFacturacion.WebApi.Model;
namespace SistemaFacturacion.WebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IDbConnection _connection;
        public CustomerController(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("SqlConnection"));
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var customer = _connection.Query<Customer>("SELECT * FORM Customers");
            return Ok(customer);
        }

        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            var customer = await _connection.QueryAsync<Customer>("SELECT * FORM Customers");
            return Ok(customer);
        }

        [HttpGet("ObtenerPorIdAsync{Id}")]
        public async Task<IActionResult> ObtenerPorIdAsync(int id)
        {
            var parametros = new
            {
                Id = id
            };
            var customer = await _connection.QueryFirstOrDefaultAsync<Customer>("SELECT * FROM Customers WHERE Id = @Id", parametros);
            return Ok(customer);
        }

        [HttpGet("GetByNameAsync/{nombre}")]
        public async Task<IActionResult> GetByNameAsync(string nombre)
        {
            var customer = await _connection.QueryFirstOrDefaultAsync<Customer>("SELECT * FROM Customers WHERE Name = @NombreCompleto55", new { NombreCompleto55 = nombre });
            return Ok(customer);
        }

        [HttpGet("BuscarClientePorNombreAsync/{nombre}")]
        public async Task<IActionResult> BuscarClientePorNombreAsync(string nombre)
        {
            var customer = await _connection.QueryAsync<Customer>("SELECT * FROM Customers WHERE Name LIKE @Nombre", new { Nombre = $"%{nombre}%" });
            return Ok(customer);
        }
    }
}
