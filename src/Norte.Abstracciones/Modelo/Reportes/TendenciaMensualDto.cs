namespace Norte.Abstracciones.Modelo.Reportes;

/// <summary>Ingresos, gastos y balance de un mes, para la gráfica de tendencia.</summary>
public record TendenciaMensualDto(int Anio, int Mes, decimal Ingresos, decimal Gastos, decimal Balance);
