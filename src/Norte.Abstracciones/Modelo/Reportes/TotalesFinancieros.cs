namespace Norte.Abstracciones.Modelo.Reportes;

/// <summary>Suma de ingresos y de gastos de un periodo. El balance es la diferencia.</summary>
public record TotalesFinancieros(decimal Ingresos, decimal Gastos)
{
    public static readonly TotalesFinancieros Vacios = new(0m, 0m);

    public decimal Balance => Ingresos - Gastos;
}
