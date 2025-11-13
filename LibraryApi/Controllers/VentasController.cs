using FarmaciaApi.Models.DTOs;
using FarmaciaApi.Models.Entities;
using LibraryApi.Data;
using LibraryApi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmaciaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "SoloAdmins")]
    public class VentasController :ControllerBase
    {
        private readonly FarmaciaDbContext _context;

        public VentasController(FarmaciaDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VentaCreateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var venta = new Ventas
                {
                    Cliente = dto.Cliente,
                    Usuario = dto.Usuario,
                    Fecha = DateTime.UtcNow
                };

                decimal total = 0;

                foreach (var item in dto.Detalles)
                {
                    var medicamento = await _context.Medicamento.FindAsync(item.MedicamentoId);
                    if (medicamento == null)
                        return BadRequest($"El medicamento con ID {item.MedicamentoId} no existe.");

                    if (medicamento.CantidadDisponible < item.Cantidad)
                        return BadRequest($"Stock insuficiente para {medicamento.Nombre}.");

                    // Actualizar stock
                    medicamento.CantidadDisponible -= item.Cantidad;

                    // Crear detalle
                    var detalle = new VentaDetalle
                    {
                        MedicamentoId = medicamento.Id,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = medicamento.PrecioVenta
                    };

                    venta.Detalles.Add(detalle);
                    total += detalle.Subtotal;
                }

                venta.Total = total;

                _context.Venta.Add(venta);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(venta);

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { error = "Error al registrar la venta.", detalle = ex.Message });
            }
        }
    }
}
