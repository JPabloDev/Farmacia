using LibraryApi.Data;
using LibraryApi.Models.DTOs;
using LibraryApi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "SoloAdmins")]
    public class MedicamentosController : ControllerBase
    {
        private readonly FarmaciaDbContext _context;

        public MedicamentosController(FarmaciaDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.Medicamento.ToListAsync());

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MedicamentosDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Asignar valores automáticos o calculados si es necesario
                var medicamento = new Medicamentos
                {
                    Codigo = dto.Codigo,
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion,
                    Presentacion = dto.Presentacion,
                    Concentracion = dto.Concentracion,
                    UnidadMedida = dto.UnidadMedida,
                    Laboratorio = dto.Laboratorio,
                    FechaFabricacion = dto.FechaFabricacion,
                    FechaVencimiento = dto.FechaVencimiento,
                    CantidadDisponible = dto.CantidadDisponible,
                    StockMinimo = dto.StockMinimo,
                    PrecioCompra = dto.PrecioCompra,
                    PrecioVenta = dto.PrecioVenta,
                    RequiereReceta = dto.RequiereReceta,
                    Estado = true,
                    FechaRegistro = DateTime.UtcNow
                };

                await _context.Medicamento.AddAsync(medicamento);
                await _context.SaveChangesAsync();

                // Retornar 201 Created con la ubicación del recurso
                return Ok(medicamento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocurrió un error al crear el medicamento.", detalle = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Medicamentos dto)
        {
            var medicamento = await _context.Medicamento.FirstOrDefaultAsync(l => l.Id == id);
            if (medicamento == null) 
                return NotFound();

            _context.Entry(medicamento).CurrentValues.SetValues(dto);

            await _context.SaveChangesAsync();
            return Ok(medicamento);
        }


        [Authorize]
        [HttpPut("ChangeStatusToInActive/{id}")]
        public async Task<IActionResult> ChangeStatusToInActive(int id)
        {
            var book = await _context.Medicamento.FirstOrDefaultAsync(l => l.Id == id);
            if (book == null) 
                return NotFound();

            book.Estado = false;

            await _context.SaveChangesAsync();
            return Ok("El Medicamento ha Cambiado de estado Exitosamente.");
        }
    }
}