using Norte.Abstracciones.DA;
using Norte.Abstracciones.Flujo;
using Norte.Abstracciones.Modelo.Comun;
using Norte.Abstracciones.Modelo.Reportes;
using Norte.Abstracciones.Reglas;

namespace Norte.Flujo;

/// <summary>
/// Casos de uso de los reportes: trae los datos (varias consultas a la vez) y se los pasa a
/// la regla funcional, que hace los cálculos.
/// </summary>
public class ReporteFlujo : IReporteFlujo
{
    public const int MesesMaximosTendencia = 12;

    private readonly IReporteDA _reporteDA;
    private readonly IReglaResumenFinanciero _reglaResumen;
    private readonly TimeProvider _reloj;

    public ReporteFlujo(IReporteDA reporteDA, IReglaResumenFinanciero reglaResumen, TimeProvider reloj)
    {
        _reporteDA = reporteDA;
        _reglaResumen = reglaResumen;
        _reloj = reloj;
    }

    public async Task<ResumenFinancieroDto> ObtenerResumenAsync(Guid usuarioId, int? anio, int? mes)
    {
        var periodo = ResolverPeriodo(anio, mes);
        var anterior = periodo.Retroceder(1);

        // Tres consultas independientes al mismo tiempo; cada una con su propio contexto.
        var transaccionesTarea = _reporteDA.ObtenerTransaccionesAsync(usuarioId, anterior.Inicio, periodo.Fin);
        var deudasTarea = _reporteDA.ContarDeudasActivasAsync(usuarioId);
        var metasTarea = _reporteDA.ContarMetasEnCursoAsync(usuarioId, Hoy());
        await Task.WhenAll(transaccionesTarea, deudasTarea, metasTarea);

        // Las tres ya terminaron: el await solo toma su resultado.
        var transacciones = await transaccionesTarea;
        var totales = _reglaResumen.CalcularTotales(transacciones, periodo);
        var totalesAnteriores = _reglaResumen.CalcularTotales(transacciones, anterior);

        return new ResumenFinancieroDto(
            periodo.Anio,
            periodo.Mes,
            totales.Ingresos,
            totales.Gastos,
            totales.Balance,
            _reglaResumen.CalcularVariacionPorcentual(totalesAnteriores.Balance, totales.Balance),
            await deudasTarea,
            await metasTarea);
    }

    public async Task<IReadOnlyList<GastoPorCategoriaDto>> ObtenerGastoPorCategoriaAsync(Guid usuarioId, int? anio, int? mes)
    {
        var periodo = ResolverPeriodo(anio, mes);

        var transaccionesTarea = _reporteDA.ObtenerTransaccionesAsync(usuarioId, periodo.Inicio, periodo.Fin);
        var categoriasTarea = _reporteDA.ObtenerCategoriasAsync();
        await Task.WhenAll(transaccionesTarea, categoriasTarea);

        return _reglaResumen.CalcularGastoPorCategoria(await transaccionesTarea, await categoriasTarea, periodo);
    }

    public async Task<IReadOnlyList<TendenciaMensualDto>> ObtenerTendenciaAsync(Guid usuarioId, int meses)
    {
        if (meses is < 1 or > MesesMaximosTendencia)
            throw new ReglaNegocioException($"La tendencia se pide de 1 a {MesesMaximosTendencia} meses.");

        var actual = PeriodoMensual.DeFecha(Hoy());
        var primero = actual.Retroceder(meses - 1);
        var transacciones = await _reporteDA.ObtenerTransaccionesAsync(usuarioId, primero.Inicio, actual.Fin);

        return _reglaResumen.CalcularTendencia(transacciones, actual, meses);
    }

    // Año y mes van juntos: sin ninguno se usa el mes actual; con uno solo es un error.
    private PeriodoMensual ResolverPeriodo(int? anio, int? mes) => (anio, mes) switch
    {
        (null, null) => PeriodoMensual.DeFecha(Hoy()),
        ({ } a, { } m) => PeriodoMensual.Crear(a, m),
        _ => throw new ReglaNegocioException("Indique el año y el mes juntos, o ninguno de los dos."),
    };

    private DateOnly Hoy() => DateOnly.FromDateTime(_reloj.GetUtcNow().UtcDateTime);
}
