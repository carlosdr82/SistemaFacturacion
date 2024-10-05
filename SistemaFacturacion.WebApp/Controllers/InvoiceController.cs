using Microsoft.AspNetCore.Mvc;
using SistemaFacturacion.WebApp.Dto;
using SistemaFacturacion.WebApp.Models;
using System.Text.Json;

namespace SistemaFacturacion.WebApp.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public InvoiceController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient("ProductsApi");
            var response = await client.GetAsync("api/invoice");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var invoices = JsonSerializer.Deserialize<List<InvoiceListItemDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(invoices);
            }

            return View(new List<InvoiceListItemDto>());
        }
    }
}
