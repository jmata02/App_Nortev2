using Microsoft.AspNetCore.Mvc;

namespace Norte.Abstracciones.API;

public interface IReportesController
{
    Task<IActionResult> ObtenerResumenAsync(int? anio, int? mes);
    Task<IActionResult> ObtenerGastoPorCategoriaAsync(int? anio, int? mes);
    Task<IActionResult> ObtenerTendenciaAsync(int meses);
}
