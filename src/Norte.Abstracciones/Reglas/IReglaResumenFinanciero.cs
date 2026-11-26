using Norte.Abstracciones.Modelo.Categorias;
using Norte.Abstracciones.Modelo.Movimientos;
using Norte.Abstracciones.Modelo.Reportes;

namespace Norte.Abstracciones.Reglas;

/// <summary>
/// Cálculos de los reportes. Son funciones puras: reciben colecciones, devuelven resultados
/// nuevos y nunca modifican lo que reciben.
/// </summary>
public interface IReglaResumenFinanciero
{
    TotalesFinancieros CalcularTotales(IEnumerable<Transaccion> transacciones, PeriodoMensual periodo);

    decimal? CalcularVariacionPorcentual(decimal anterior, decimal actual);

    IReadOnlyList<GastoPorCategoriaDto> CalcularGastoPorCategoria(
        IEnumerable<Transaccion> transacciones, IEnumerable<Categoria> categorias, PeriodoMensual periodo);

    /// <summary>Espera <paramref name="cantidadMeses"/> de 1 en adelante (el Flujo lo valida).</summary>
    IReadOnlyList<TendenciaMensualDto> CalcularTendencia(
        IEnumerable<Transaccion> transacciones, PeriodoMensual ultimoPeriodo, int cantidadMeses);
}
