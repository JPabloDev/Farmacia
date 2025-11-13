namespace LibraryApi.Models.DTOs
{
    public class MedicamentosDto
    {
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string Presentacion { get; set; } = null!;
        public string Concentracion { get; set; } = null!;
        public string UnidadMedida { get; set; } = null!;
        public string? Laboratorio { get; set; }
        public DateTime? FechaFabricacion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public int CantidadDisponible { get; set; }
        public int StockMinimo { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public bool RequiereReceta { get; set; }
        public bool Estado { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        public string? UsuarioRegistro { get; set; }

    }
}
