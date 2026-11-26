using Norte.Abstracciones.Modelo.Reportes;

namespace Norte.Abstracciones.Flujo;

public interface IReporteFlujo
{
    /// <summary>Sin año y mes, usa el mes actual.</summary>
    Task<ResumenFinancieroDto> ObtenerResumenAsync(Guid usuarioId, int? anio, int? mes);

    Task<IReadOnlyList<GastoPorCategoriaDto>> ObtenerGastoPorCategoriaAsync(Guid usuarioId, int? anio, int? mes);

    /// <summary>Los últimos <paramref name="meses"/> meses, terminando en el actual.</summary>
    Task<IReadOnlyList<TendenciaMensualDto>> ObtenerTendenciaAsync(Guid usuarioId, int meses);
}
