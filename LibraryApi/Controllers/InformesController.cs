using LibraryApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Xml.Linq;

namespace FarmaciaApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "SoloAdmins")]
    public class InformesController :ControllerBase
    {
        private readonly FarmaciaDbContext _context;

        public InformesController(FarmaciaDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("diarias")]
        public async Task<IActionResult> GetDailySales()
        {
            var hoy = DateTime.UtcNow.Date;
            var haceDiezDias = hoy.AddDays(-9); // incluye el día de hoy (10 días en total)

            var ventas = await _context.Venta
                .Include(v => v.Detalles)
                .ThenInclude(d => d.Medicamento)
                .Where(v => v.Fecha >= haceDiezDias && v.Fecha <= hoy.AddDays(1))
                .ToListAsync();

            return Ok(ventas);
        }

        [Authorize]
        [HttpGet("diarias/xml")]
        public async Task<IActionResult> ExportDailySalesToXml()
        {
            var hoy = DateTime.UtcNow.Date;
            var haceDiezDias = hoy.AddDays(-9); // incluye el día de hoy (10 días en total)

            var ventas = await _context.Venta
                .Include(v => v.Detalles)
                .ThenInclude(d => d.Medicamento)
                .Where(v => v.Fecha >= haceDiezDias && v.Fecha <= hoy.AddDays(1))
                .ToListAsync();

            var xml = new XElement("Ventas",
                from v in ventas
                select new XElement("Venta",
                    new XAttribute("Id", v.Id),
                    new XElement("Fecha", v.Fecha),
                    new XElement("Cliente", v.Cliente ?? ""),
                    new XElement("Usuario", v.Usuario ?? ""),
                    new XElement("Total", v.Total),
                    new XElement("Detalles",
                        from d in v.Detalles
                        select new XElement("Detalle",
                            new XElement("Medicamento", d.MedicamentoId),
                            new XElement("Cantidad", d.Cantidad),
                            new XElement("PrecioUnitario", d.PrecioUnitario),
                            new XElement("Subtotal", d.Subtotal)
                        )
                    )
                )
            );

            return File(
                System.Text.Encoding.UTF8.GetBytes(xml.ToString()),
                "application/xml",
                $"ventas_{hoy:yyyyMMdd}.xml"
            );
        }

        [AllowAnonymous]
        [HttpGet("ultimos10dias/xml")]
        public async Task<IActionResult> ExportSalesToHtml()
        {
            var hoy = DateTime.UtcNow.Date;
            var haceDiezDias = hoy.AddDays(-9);

            var ventas = await _context.Venta
                .Include(v => v.Detalles)
                .ThenInclude(d => d.Medicamento)
                .Where(v => v.Fecha >= haceDiezDias && v.Fecha <= hoy.AddDays(1))
                .ToListAsync();

            var html = new StringBuilder();
            html.Append("<html><head><title>Ventas ultimos 10 días</title></head><body>");
            html.Append("<meta charset='UTF-8'>");
            html.Append("<h2>Reporte de Ventas (ultimos 10 dias)</h2>");

            foreach (var venta in ventas)
            {
                html.Append($"<h3>Venta #{venta.Id} - {venta.Fecha:dd/MM/yyyy}</h3>");
                html.Append("<table border='1' cellpadding='4' cellspacing='0'>");
                html.Append("<tr><th>Medicamento</th><th>Cantidad</th><th>Precio</th><th>Subtotal</th></tr>");

                foreach (var d in venta.Detalles)
                {
                    html.Append($"<tr><td>{d.Medicamento?.Nombre}</td><td>{d.Cantidad}</td><td>{d.PrecioUnitario:C}</td><td>{d.Subtotal:C}</td></tr>");
                }

                html.Append($"<tr><td colspan='3'><b>Total</b></td><td><b>{venta.Total:C}</b></td></tr>");
                html.Append("</table><hr/>");
            }

            html.Append("</body></html>");

            return Content(html.ToString(), "text/html");
        }
    }
}
