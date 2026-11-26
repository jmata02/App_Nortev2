using Norte.Abstracciones.Modelo.Categorias;
using Norte.Abstracciones.Modelo.Comun;
using Norte.Abstracciones.Modelo.Movimientos;
using Norte.Abstracciones.Modelo.Reportes;
using Norte.Reglas;

namespace Norte.Pruebas.Unitarias.Reglas;

public class ReglaResumenFinancieroPruebas
{
    private static readonly Categoria Comida = new() { Id = Guid.NewGuid(), Nombre = "Alimentación", Tipo = TipoMovimiento.Gasto };
    private static readonly Categoria Transporte = new() { Id = Guid.NewGuid(), Nombre = "Transporte", Tipo = TipoMovimiento.Gasto };
    private static readonly Categoria Salario = new() { Id = Guid.NewGuid(), Nombre = "Salario", Tipo = TipoMovimiento.Ingreso };
    private static readonly PeriodoMensual Noviembre = PeriodoMensual.Crear(2026, 11);

    private readonly ReglaResumenFinanciero _regla = new();

    private static Transaccion Movimiento(Categoria categoria, decimal monto, int mes = 11, int dia = 10, int anio = 2026) => new()
    {
        Id = Guid.NewGuid(),
        CategoriaId = categoria.Id,
        Tipo = categoria.Tipo,
        Monto = monto,
        Fecha = new DateOnly(anio, mes, dia),
    };

    [Fact]
    public void CalcularTotales_SumaIngresosYGastosSoloDelPeriodo()
    {
        var transacciones = new[]
        {
            Movimiento(Salario, 950000m),
            Movimiento(Comida, 38400.50m),
            Movimiento(Transporte, 22000.25m, dia: 30),
            Movimiento(Comida, 99999m, mes: 10), // otro mes: no cuenta
        };

        var totales = _regla.CalcularTotales(transacciones, Noviembre);

        Assert.Equal(950000m, totales.Ingresos);
        Assert.Equal(60400.75m, totales.Gastos);
        Assert.Equal(889599.25m, totales.Balance);
    }

    [Fact]
    public void CalcularTotales_SinMovimientos_DevuelveCeros()
    {
        Assert.Equal(TotalesFinancieros.Vacios, _regla.CalcularTotales([], Noviembre));
    }

    [Fact]
    public void CalcularTotales_NoModificaLaColeccionRecibida()
    {
        var transacciones = new List<Transaccion> { Movimiento(Salario, 100m), Movimiento(Comida, 40m) };
        var copia = transacciones.Select(t => (t.Id, t.Monto, t.Fecha)).ToList();

        _regla.CalcularTotales(transacciones, Noviembre);
        _regla.CalcularGastoPorCategoria(transacciones, [Comida, Salario], Noviembre);
        _regla.CalcularTendencia(transacciones, Noviembre, 3);

        Assert.Equal(copia, transacciones.Select(t => (t.Id, t.Monto, t.Fecha)).ToList());
    }

    [Theory]
    [InlineData(100, 112, 12)]
    [InlineData(300, 200, -33.33)]
    [InlineData(-100, 50, 150)]
    [InlineData(800, 801, 0.13)] // 0.125 se redondea a 0.13 (AwayFromZero), no a 0.12
    public void CalcularVariacionPorcentual_RedondeaADosDecimales(double anterior, double actual, double esperado)
    {
        var variacion = _regla.CalcularVariacionPorcentual((decimal)anterior, (decimal)actual);

        Assert.Equal((decimal)esperado, variacion);
    }

    [Fact]
    public void CalcularVariacionPorcentual_AnteriorEnCero_DevuelveNull()
    {
        Assert.Null(_regla.CalcularVariacionPorcentual(0m, 5000m));
    }

    [Fact]
    public void CalcularGastoPorCategoria_AgrupaOrdenaYCalculaPorcentajes()
    {
        var transacciones = new[]
        {
            Movimiento(Comida, 100m),
            Movimiento(Comida, 100m),
            Movimiento(Transporte, 100m),
            Movimiento(Salario, 5000m), // ingreso: no es gasto
        };

        var resultado = _regla.CalcularGastoPorCategoria(transacciones, [Comida, Transporte, Salario], Noviembre);

        Assert.Equal(2, resultado.Count);
        Assert.Equal(("Alimentación", 200m, 66.67m), (resultado[0].Categoria, resultado[0].Total, resultado[0].Porcentaje));
        Assert.Equal(("Transporte", 100m, 33.33m), (resultado[1].Categoria, resultado[1].Total, resultado[1].Porcentaje));
    }

    [Fact]
    public void CalcularGastoPorCategoria_CategoriaDesconocida_SeLlamaSinCategoria()
    {
        var huerfana = new Categoria { Id = Guid.NewGuid(), Nombre = "Borrada", Tipo = TipoMovimiento.Gasto };

        var resultado = _regla.CalcularGastoPorCategoria([Movimiento(huerfana, 10m)], [Comida], Noviembre);

        Assert.Equal("Sin categoría", Assert.Single(resultado).Categoria);
        Assert.Equal(100m, resultado[0].Porcentaje);
    }

    [Fact]
    public void CalcularTendencia_DevuelveCadaMesEnOrdenConCerosDondeNoHayDatos()
    {
        var transacciones = new[]
        {
            Movimiento(Salario, 1000m, mes: 9),
            Movimiento(Comida, 400m, mes: 9),
            Movimiento(Comida, 250m, mes: 11),
        };

        var tendencia = _regla.CalcularTendencia(transacciones, Noviembre, 3);

        Assert.Equal(
            [
                new TendenciaMensualDto(2026, 9, 1000m, 400m, 600m),
                new TendenciaMensualDto(2026, 10, 0m, 0m, 0m),
                new TendenciaMensualDto(2026, 11, 0m, 250m, -250m),
            ],
            tendencia);
    }

    [Fact]
    public void CalcularTendencia_CruzaElCambioDeAnio()
    {
        var febrero = PeriodoMensual.Crear(2027, 2);

        var tendencia = _regla.CalcularTendencia([Movimiento(Salario, 10m, mes: 12)], febrero, 4);

        Assert.Equal([(2026, 11), (2026, 12), (2027, 1), (2027, 2)], tendencia.Select(t => (t.Anio, t.Mes)));
        Assert.Equal(10m, tendencia[1].Ingresos);
    }
}
