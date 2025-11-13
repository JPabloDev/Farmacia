using FarmaciaApi.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LibraryApi.Models.Entities
{
    public class Medicamentos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string Presentacion { get; set; } = string.Empty;
        public string Concentracion { get; set; } = string.Empty;
        public string UnidadMedida { get; set; } = string.Empty;
        public string? Laboratorio { get; set; }
        public string? Lote { get; set; }
        public DateTime? FechaFabricacion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public int CantidadDisponible { get; set; }
        public int StockMinimo { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public string? Ubicacion { get; set; }
        public bool RequiereReceta { get; set; }
        public bool Estado { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public string? UsuarioRegistro { get; set; }

        // Relación
        public ICollection<VentaDetalle>? VentaDetalles { get; set; }

    }
}
