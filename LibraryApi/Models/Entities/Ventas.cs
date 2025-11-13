namespace FarmaciaApi.Models.Entities
{
    public class Ventas
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public string? Cliente { get; set; }
        public string? Usuario { get; set; }

        // Relación
        public ICollection<VentaDetalle>? Detalles { get; set; }
    }
}
