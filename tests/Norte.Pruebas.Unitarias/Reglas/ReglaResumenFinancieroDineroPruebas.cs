using Norte.Abstracciones.Modelo.Categorias;
using Norte.Abstracciones.Modelo.Comun;
using Norte.Abstracciones.Modelo.Movimientos;
using Norte.Abstracciones.Modelo.Reportes;
using Norte.Reglas;

namespace Norte.Pruebas.Unitarias.Reglas;

/// <summary>
/// Riesgo 3 de la propuesta (errores con dinero): sumas exactas con decimal, redondeo de
/// porcentajes y bordes de los periodos. Los valores van como decimal (no double) para que la
/// prueba de redondeo sea exacta.
/// </summary>
public class ReglaResumenFinancieroDineroPruebas
{
    private static readonly PeriodoMensual Noviembre = PeriodoMensual.Crear(2026, 11);
    private readonly ReglaResumenFinanciero _regla = new();

    private static Transaccion Movimiento(TipoMovimiento tipo, decimal monto, DateOnly fecha, Guid? categoriaId = null) => new()
    {
        Id = Guid.NewGuid(),
        CategoriaId = categoriaId ?? Guid.Empty,
        Tipo = tipo,
        Monto = monto,
        Fecha = fecha,
    };

    public static TheoryData<decimal, decimal, decimal> CasosDeRedondeo => new()
    {
        { 800m, 801m, 0.13m },    // +0.125 → 0.13
        { 800m, 799m, -0.13m },   // −0.125 → −0.13 (AwayFromZero: se aleja del cero, no va a −0.12)
        { 300m, 200m, -33.33m },
    };

    [Theory]
    [MemberData(nameof(CasosDeRedondeo))]
    public void CalcularVariacionPorcentual_PuntoMedio_SeAlejaDelCero(decimal anterior, decimal actual, decimal esperado)
    {
        Assert.Equal(esperado, _regla.CalcularVariacionPorcentual(anterior, actual));
    }

    [Fact]
    public void CalcularTotales_MontosConCentimos_SumaExactaSinErroresDeRedondeo()
    {
        // Con double, 0.10 + 0.20 da 0.30000000000000004; con decimal da 0.30 exacto.
        var transacciones = new[]
        {
            Movimiento(TipoMovimiento.Gasto, 0.10m, new DateOnly(2026, 11, 3)),
            Movimiento(TipoMovimiento.Gasto, 0.20m, new DateOnly(2026, 11, 4)),
            Movimiento(TipoMovimiento.Ingreso, 100.05m, new DateOnly(2026, 11, 5)),
        };

        var totales = _regla.CalcularTotales(transacciones, Noviembre);

        Assert.Equal(0.30m, totales.Gastos);
        Assert.Equal(99.75m, totales.Balance);
    }

    [Fact]
    public void CalcularTotales_PrimerYUltimoDiaDelMes_CuentanYElDiaSiguienteNo()
    {
        var transacciones = new[]
        {
            Movimiento(TipoMovimiento.Gasto, 1m, new DateOnly(2026, 11, 1)),
            Movimiento(TipoMovimiento.Gasto, 2m, new DateOnly(2026, 11, 30)),
            Movimiento(TipoMovimiento.Gasto, 50m, new DateOnly(2026, 12, 1)),
            Movimiento(TipoMovimiento.Gasto, 70m, new DateOnly(2025, 11, 15)), // mismo mes, otro año
        };

        Assert.Equal(3m, _regla.CalcularTotales(transacciones, Noviembre).Gastos);
    }

    [Fact]
    public void CalcularGastoPorCategoria_TresCategoriasIguales_LosPorcentajesRedondeadosSuman99_99()
    {
        // Comportamiento documentado: cada porcentaje se redondea por separado, así que la
        // suma puede no dar exactamente 100.
        var categorias = Enumerable.Range(1, 3)
            .Select(n => new Categoria { Id = Guid.NewGuid(), Nombre = $"Categoría {n}", Tipo = TipoMovimiento.Gasto })
            .ToList();
        var transacciones = categorias
            .Select(c => Movimiento(TipoMovimiento.Gasto, 100m, new DateOnly(2026, 11, 10), c.Id));

        var resultado = _regla.CalcularGastoPorCategoria(transacciones, categorias, Noviembre);

        Assert.All(resultado, c => Assert.Equal(33.33m, c.Porcentaje));
        Assert.Equal(99.99m, resultado.Sum(c => c.Porcentaje));
    }

    [Fact]
    public void CalcularGastoPorCategoria_NoModificaLaListaDeCategorias()
    {
        var categoria = new Categoria { Id = Guid.NewGuid(), Nombre = "Transporte", Tipo = TipoMovimiento.Gasto };
        var categorias = new List<Categoria> { categoria };

        _regla.CalcularGastoPorCategoria(
            [Movimiento(TipoMovimiento.Gasto, 10m, new DateOnly(2026, 11, 2), categoria.Id)], categorias, Noviembre);

        Assert.Single(categorias);
        Assert.Equal(("Transporte", TipoMovimiento.Gasto), (categorias[0].Nombre, categorias[0].Tipo));
    }
}
