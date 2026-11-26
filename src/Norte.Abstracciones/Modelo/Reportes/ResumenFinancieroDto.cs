namespace Norte.Abstracciones.Modelo.Reportes;

/// <summary>
/// Resumen del mes para el Dashboard. VariacionPorcentual compara el balance con el del mes
/// anterior; es null cuando el mes anterior tuvo balance 0 (no se puede dividir entre 0).
/// </summary>
public record ResumenFinancieroDto(
    int Anio,
    int Mes,
    decimal TotalIngresos,
    decimal TotalGastos,
    decimal Balance,
    decimal? VariacionPorcentual,
    int DeudasActivas,
    int MetasEnCurso);
