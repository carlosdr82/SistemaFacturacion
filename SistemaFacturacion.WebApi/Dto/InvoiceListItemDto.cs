﻿namespace SistemaFacturacion.WebApi.Dto
{
    public class InvoiceListItemDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public string CustomerName { get; set; }
    }
}
