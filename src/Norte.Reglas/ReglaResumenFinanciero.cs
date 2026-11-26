using Norte.Abstracciones.Modelo.Categorias;
using Norte.Abstracciones.Modelo.Comun;
using Norte.Abstracciones.Modelo.Movimientos;
using Norte.Abstracciones.Modelo.Reportes;
using Norte.Abstracciones.Reglas;

namespace Norte.Reglas;

/// <summary>
/// PARADIGMA FUNCIONAL (módulo Dashboard y reportes, Jurgen).
///
/// Cada método es una función pura: el resultado depende solo de sus parámetros, no lee
/// la base de datos ni la hora, no guarda estado y nunca modifica las colecciones que recibe.
/// Los reportes se arman encadenando transformaciones de LINQ con lambdas:
///   Where (filtrar) → GroupBy (agrupar) → Select (transformar) → Aggregate / Sum (reducir).
/// Por eso se pueden probar con datos en memoria y dan siempre el mismo resultado.
/// </summary>
public class ReglaResumenFinanciero : IReglaResumenFinanciero
{
    private const int DecimalesPorcentaje = 2;

    /// <summary>
    /// Ingresos y gastos del periodo en una sola pasada: Aggregate parte de unos totales
    /// vacíos y, por cada transacción, devuelve unos totales NUEVOS (no suma sobre una variable).
    /// </summary>
    public TotalesFinancieros CalcularTotales(IEnumerable<Transaccion> transacciones, PeriodoMensual periodo) =>
        transacciones
            .Where(t => periodo.Contiene(t.Fecha))
            .Aggregate(
                TotalesFinancieros.Vacios,
                // TipoMovimiento solo tiene Ingreso y Gasto: lo que no es ingreso es gasto.
                (acumulado, t) => t.Tipo == TipoMovimiento.Ingreso
                    ? acumulado with { Ingresos = acumulado.Ingresos + t.Monto }
                    : acumulado with { Gastos = acumulado.Gastos + t.Monto });

    /// <summary>Cuánto cambió el balance respecto al anterior, en porcentaje con 2 decimales.</summary>
    public decimal? CalcularVariacionPorcentual(decimal anterior, decimal actual) =>
        anterior == 0m
            ? null
            : RedondearPorcentaje((actual - anterior) / Math.Abs(anterior) * 100m);

    /// <summary>
    /// Gastos del periodo agrupados por categoría, con su porcentaje del total, de mayor a menor.
    /// </summary>
    public IReadOnlyList<GastoPorCategoriaDto> CalcularGastoPorCategoria(
        IEnumerable<Transaccion> transacciones, IEnumerable<Categoria> categorias, PeriodoMensual periodo)
    {
        var nombres = categorias.ToDictionary(c => c.Id, c => c.Nombre);

        var totalesPorCategoria = transacciones
            .Where(t => t.Tipo == TipoMovimiento.Gasto && periodo.Contiene(t.Fecha))
            .GroupBy(t => t.CategoriaId)
            .Select(grupo => (CategoriaId: grupo.Key, Total: grupo.Sum(t => t.Monto)))
            .ToList();

        var gastoTotal = totalesPorCategoria.Sum(c => c.Total);

        return totalesPorCategoria
            .Select(c => new GastoPorCategoriaDto(
                c.CategoriaId,
                nombres.GetValueOrDefault(c.CategoriaId, "Sin categoría"),
                c.Total,
                gastoTotal == 0m ? 0m : RedondearPorcentaje(c.Total / gastoTotal * 100m)))
            .OrderByDescending(c => c.Total)
            .ThenBy(c => c.Categoria)
            .ToList();
    }

    /// <summary>
    /// Ingresos, gastos y balance de los últimos meses. Los meses sin movimientos salen en 0,
    /// para que la gráfica no tenga huecos.
    /// </summary>
    public IReadOnlyList<TendenciaMensualDto> CalcularTendencia(
        IEnumerable<Transaccion> transacciones, PeriodoMensual ultimoPeriodo, int cantidadMeses)
    {
        var periodos = Enumerable.Range(0, cantidadMeses)
            .Select(atras => ultimoPeriodo.Retroceder(cantidadMeses - 1 - atras))
            .ToList();

        // Una sola pasada por las transacciones: se agrupan por mes y se reducen a totales.
        var totalesPorMes = transacciones
            .GroupBy(t => PeriodoMensual.DeFecha(t.Fecha))
            .ToDictionary(grupo => grupo.Key, grupo => CalcularTotales(grupo, grupo.Key));

        return periodos
            .Select(periodo =>
            {
                var totales = totalesPorMes.GetValueOrDefault(periodo, TotalesFinancieros.Vacios);
                return new TendenciaMensualDto(periodo.Anio, periodo.Mes, totales.Ingresos, totales.Gastos, totales.Balance);
            })
            .ToList();
    }

    // Dinero y porcentajes: decimal, 2 decimales, y los medios (x,xx5) se redondean hacia
    // arriba en valor absoluto, como se hace a mano (riesgo 3 del documento).
    private static decimal RedondearPorcentaje(decimal valor) =>
        Math.Round(valor, DecimalesPorcentaje, MidpointRounding.AwayFromZero);
}
