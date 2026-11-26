using Norte.Abstracciones.Modelo.Categorias;
using Norte.Abstracciones.Modelo.Comun;
using Norte.Abstracciones.Modelo.Movimientos;
using Norte.Flujo;
using Norte.Pruebas.Unitarias.Fakes;
using Norte.Reglas;

namespace Norte.Pruebas.Unitarias.Flujo;

public class ReporteFlujoPruebas
{
    private static readonly Guid Usuario = Guid.NewGuid();
    private static readonly Guid OtroUsuario = Guid.NewGuid();
    private static readonly Categoria Comida = new() { Id = Guid.NewGuid(), Nombre = "Alimentación", Tipo = TipoMovimiento.Gasto };

    private readonly ReporteDAFake _reporteDA = new();
    private readonly ReporteFlujo _flujo;

    public ReporteFlujoPruebas()
    {
        // "Hoy" es el 26 de noviembre de 2026; la regla es la real (es pura).
        _flujo = new ReporteFlujo(_reporteDA, new ReglaResumenFinanciero(), new RelojFijo(2026, 11, 26));
        _reporteDA.Categorias.Add(Comida);
    }

    private void Agregar(Guid usuarioId, TipoMovimiento tipo, decimal monto, int mes) =>
        _reporteDA.Transacciones.Add(new Transaccion
        {
            Id = Guid.NewGuid(),
            UsuarioId = usuarioId,
            CategoriaId = Comida.Id,
            Tipo = tipo,
            Monto = monto,
            Fecha = new DateOnly(2026, mes, 5),
        });

    [Fact]
    public async Task ObtenerResumenAsync_SinPeriodo_UsaElMesActualYComparaConElAnterior()
    {
        Agregar(Usuario, TipoMovimiento.Ingreso, 1000m, mes: 10);
        Agregar(Usuario, TipoMovimiento.Ingreso, 1200m, mes: 11);
        _reporteDA.DeudasActivas = 4;
        _reporteDA.MetasEnCurso = 2;

        var resumen = await _flujo.ObtenerResumenAsync(Usuario, null, null);

        Assert.Equal((2026, 11), (resumen.Anio, resumen.Mes));
        Assert.Equal(1200m, resumen.Balance);
        Assert.Equal(20m, resumen.VariacionPorcentual);
        Assert.Equal((4, 2), (resumen.DeudasActivas, resumen.MetasEnCurso));
    }

    [Fact]
    public async Task ObtenerResumenAsync_PasaElUsuarioYLaFechaDelRelojALosConteos()
    {
        await _flujo.ObtenerResumenAsync(Usuario, 2026, 5);

        Assert.Equal(Usuario, _reporteDA.UsuarioDeudasRecibido);
        Assert.Equal((Usuario, new DateOnly(2026, 11, 26)), _reporteDA.ConsultaMetasRecibida);
    }

    [Fact]
    public async Task ObtenerResumenAsync_NoMezclaDatosDeOtroUsuario()
    {
        Agregar(OtroUsuario, TipoMovimiento.Gasto, 500m, mes: 11);

        var resumen = await _flujo.ObtenerResumenAsync(Usuario, 2026, 11);

        Assert.Equal(0m, resumen.TotalGastos);
        Assert.Null(resumen.VariacionPorcentual);
    }

    [Theory]
    [InlineData(2026, 13)]
    [InlineData(2026, 0)]
    [InlineData(1999, 5)]
    public async Task ObtenerResumenAsync_PeriodoInvalido_LanzaReglaNegocio(int anio, int mes)
    {
        await Assert.ThrowsAsync<ReglaNegocioException>(() => _flujo.ObtenerResumenAsync(Usuario, anio, mes));
    }

    [Fact]
    public async Task ObtenerGastoPorCategoriaAsync_SoloAnioSinMes_LanzaReglaNegocio()
    {
        await Assert.ThrowsAsync<ReglaNegocioException>(() => _flujo.ObtenerGastoPorCategoriaAsync(Usuario, 2026, null));
    }

    [Fact]
    public async Task ObtenerGastoPorCategoriaAsync_DevuelveElNombreDeLaCategoria()
    {
        Agregar(Usuario, TipoMovimiento.Gasto, 300m, mes: 11);

        var resultado = await _flujo.ObtenerGastoPorCategoriaAsync(Usuario, null, null);

        Assert.Equal(("Alimentación", 300m, 100m), (resultado[0].Categoria, resultado[0].Total, resultado[0].Porcentaje));
    }

    [Fact]
    public async Task ObtenerTendenciaAsync_DevuelveLaCantidadDeMesesPedidaTerminandoEnElActual()
    {
        var tendencia = await _flujo.ObtenerTendenciaAsync(Usuario, 6);

        Assert.Equal(6, tendencia.Count);
        Assert.Equal((2026, 6), (tendencia[0].Anio, tendencia[0].Mes));
        Assert.Equal((2026, 11), (tendencia[^1].Anio, tendencia[^1].Mes));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public async Task ObtenerTendenciaAsync_MesesFueraDeRango_LanzaReglaNegocio(int meses)
    {
        await Assert.ThrowsAsync<ReglaNegocioException>(() => _flujo.ObtenerTendenciaAsync(Usuario, meses));
    }
}
