using LibraryApi.Models.Entities;

namespace FarmaciaApi.Models.Entities
{
    public class VentaDetalle
    {
        public int Id { get; set; }

        public int VentaId { get; set; }
        public Ventas? Venta { get; set; }

        public int MedicamentoId { get; set; }
        public Medicamentos? Medicamento { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        // Campo calculado (no se mapea, lo calcula el código)
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}
