using Norte.Abstracciones.Modelo.Categorias;
using Norte.Abstracciones.Modelo.Comun;
using Norte.Abstracciones.Modelo.Movimientos;
using Norte.Flujo;
using Norte.Pruebas.Unitarias.Fakes;
using Norte.Reglas;

namespace Norte.Pruebas.Unitarias.Flujo;

/// <summary>
/// Casos límite de los reportes que pide la propuesta: fechas en UTC (riesgo 4), datos aislados
/// por usuario (riesgo 7) y consultas en paralelo (paradigma concurrente).
/// </summary>
public class ReporteFlujoCasosLimitePruebas
{
    private static readonly Guid Usuario = Guid.NewGuid();
    private static readonly Guid OtroUsuario = Guid.NewGuid();
    private static readonly TimeSpan HoraCostaRica = TimeSpan.FromHours(-6);

    private readonly Categoria _comida = new() { Id = Guid.NewGuid(), Nombre = "Alimentación", Tipo = TipoMovimiento.Gasto };
    private readonly ReporteDAFake _reporteDA = new();

    private ReporteFlujo CrearFlujo(TimeProvider reloj) => new(_reporteDA, new ReglaResumenFinanciero(), reloj);

    private void Agregar(Guid usuarioId, TipoMovimiento tipo, decimal monto, int anio, int mes) =>
        _reporteDA.Transacciones.Add(new Transaccion
        {
            Id = Guid.NewGuid(),
            UsuarioId = usuarioId,
            CategoriaId = _comida.Id,
            Tipo = tipo,
            Monto = monto,
            Fecha = new DateOnly(anio, mes, 5),
        });

    [Fact]
    public async Task ObtenerResumenAsync_NocheDel30DeNoviembreEnCostaRica_UsaDiciembrePorqueEnUtcYaEs1()
    {
        // 30/11 a las 8 p. m. en Costa Rica = 1/12 a las 2 a. m. en UTC: "hoy" se toma en UTC.
        var flujo = CrearFlujo(new RelojFijo(new DateTimeOffset(2026, 11, 30, 20, 0, 0, HoraCostaRica)));

        var resumen = await flujo.ObtenerResumenAsync(Usuario, null, null);

        Assert.Equal((2026, 12), (resumen.Anio, resumen.Mes));
    }

    [Fact]
    public async Task ObtenerResumenAsync_MesEnero_ComparaConDiciembreDelAnioAnterior()
    {
        Agregar(Usuario, TipoMovimiento.Ingreso, 1000m, 2026, 12);
        Agregar(Usuario, TipoMovimiento.Ingreso, 1500m, 2027, 1);
        var flujo = CrearFlujo(new RelojFijo(2027, 1, 15));

        var resumen = await flujo.ObtenerResumenAsync(Usuario, null, null);

        Assert.Equal((2027, 1), (resumen.Anio, resumen.Mes));
        Assert.Equal(50m, resumen.VariacionPorcentual);
    }

    [Fact]
    public async Task ObtenerGastoPorCategoriaAsync_TransaccionesDeOtroUsuario_NoLasIncluye()
    {
        _reporteDA.Categorias.Add(_comida);
        Agregar(Usuario, TipoMovimiento.Gasto, 300m, 2026, 11);
        Agregar(OtroUsuario, TipoMovimiento.Gasto, 9000m, 2026, 11);

        var resultado = await CrearFlujo(new RelojFijo(2026, 11, 26)).ObtenerGastoPorCategoriaAsync(Usuario, null, null);

        Assert.Equal(300m, Assert.Single(resultado).Total);
    }

    [Fact]
    public async Task ObtenerTendenciaAsync_TransaccionesDeOtroUsuario_NoLasIncluye()
    {
        Agregar(Usuario, TipoMovimiento.Ingreso, 500m, 2026, 11);
        Agregar(OtroUsuario, TipoMovimiento.Ingreso, 9000m, 2026, 11);

        var tendencia = await CrearFlujo(new RelojFijo(2026, 11, 26)).ObtenerTendenciaAsync(Usuario, 1);

        Assert.Equal(500m, Assert.Single(tendencia).Ingresos);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(12)]
    public async Task ObtenerTendenciaAsync_LimitesValidos_DevuelveEsaCantidadDeMeses(int meses)
    {
        var tendencia = await CrearFlujo(new RelojFijo(2026, 11, 26)).ObtenerTendenciaAsync(Usuario, meses);

        Assert.Equal(meses, tendencia.Count);
    }

    [Fact]
    public async Task ObtenerResumenAsync_SoloMesSinAnio_LanzaReglaNegocio()
    {
        await Assert.ThrowsAsync<ReglaNegocioException>(() =>
            CrearFlujo(new RelojFijo(2026, 11, 26)).ObtenerResumenAsync(Usuario, null, 5));
    }

    [Fact]
    public async Task ObtenerResumenAsync_LasTresConsultasSePidenALaVez()
    {
        // Transacciones, deudas y metas: el falso solo responde cuando las tres ya se pidieron.
        var reporteDA = new ReporteDAParaleloFake(consultasEsperadas: 3);
        var flujo = new ReporteFlujo(reporteDA, new ReglaResumenFinanciero(), new RelojFijo(2026, 11, 26));

        await flujo.ObtenerResumenAsync(Usuario, null, null);

        Assert.Equal(3, reporteDA.ConsultasSimultaneas);
    }

    [Fact]
    public async Task ObtenerGastoPorCategoriaAsync_LasDosConsultasSePidenALaVez()
    {
        var reporteDA = new ReporteDAParaleloFake(consultasEsperadas: 2);
        var flujo = new ReporteFlujo(reporteDA, new ReglaResumenFinanciero(), new RelojFijo(2026, 11, 26));

        await flujo.ObtenerGastoPorCategoriaAsync(Usuario, null, null);

        Assert.Equal(2, reporteDA.ConsultasSimultaneas);
    }
}
