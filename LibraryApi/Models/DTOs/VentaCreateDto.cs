namespace FarmaciaApi.Models.DTOs
{        public class VentaCreateDto
        {
            public string? Cliente { get; set; }
            public string? Usuario { get; set; }
            public List<VentaDetalleDto> Detalles { get; set; } = new();
        }
    }

