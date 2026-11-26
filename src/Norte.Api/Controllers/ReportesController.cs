using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Norte.Abstracciones.API;
using Norte.Abstracciones.Flujo;
using Norte.Abstracciones.Modelo.Reportes;
using Norte.Abstracciones.Servicios;

namespace Norte.Api.Controllers;

/// <summary>Reportes del Dashboard. Todo se filtra por el usuario del token.</summary>
[ApiController]
[Route("api/reportes")]
[Produces("application/json")]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class ReportesController : ControllerBase, IReportesController
{
    private readonly IReporteFlujo _reporteFlujo;
    private readonly IUsuarioActual _usuarioActual;

    public ReportesController(IReporteFlujo reporteFlujo, IUsuarioActual usuarioActual)
    {
        _reporteFlujo = reporteFlujo;
        _usuarioActual = usuarioActual;
    }

    /// <summary>Totales del mes, variación del balance, deudas activas y metas en curso.</summary>
    [HttpGet("resumen")]
    [ProducesResponseType<ResumenFinancieroDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ObtenerResumenAsync([FromQuery] int? anio, [FromQuery] int? mes) =>
        Ok(await _reporteFlujo.ObtenerResumenAsync(_usuarioActual.ObtenerId(), anio, mes));

    /// <summary>Gasto del mes por categoría, de mayor a menor, con su porcentaje.</summary>
    [HttpGet("gasto-por-categoria")]
    [ProducesResponseType<IReadOnlyList<GastoPorCategoriaDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ObtenerGastoPorCategoriaAsync([FromQuery] int? anio, [FromQuery] int? mes) =>
        Ok(await _reporteFlujo.ObtenerGastoPorCategoriaAsync(_usuarioActual.ObtenerId(), anio, mes));

    /// <summary>Ingresos, gastos y balance de los últimos meses (por defecto 6).</summary>
    [HttpGet("tendencia")]
    [ProducesResponseType<IReadOnlyList<TendenciaMensualDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ObtenerTendenciaAsync([FromQuery] int meses = 6) =>
        Ok(await _reporteFlujo.ObtenerTendenciaAsync(_usuarioActual.ObtenerId(), meses));
}
