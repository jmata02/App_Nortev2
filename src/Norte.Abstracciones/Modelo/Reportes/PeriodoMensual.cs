using Norte.Abstracciones.Modelo.Comun;

namespace Norte.Abstracciones.Modelo.Reportes;

/// <summary>
/// Un mes de un año (por ejemplo noviembre de 2026). Es inmutable: moverse a otro mes
/// devuelve un periodo nuevo y nunca cambia este.
/// </summary>
public record PeriodoMensual
{
    public int Anio { get; }
    public int Mes { get; }

    private PeriodoMensual(int anio, int mes)
    {
        Anio = anio;
        Mes = mes;
    }

    /// <summary>Crea el periodo validando que el mes y el año tengan sentido.</summary>
    public static PeriodoMensual Crear(int anio, int mes)
    {
        if (mes is < 1 or > 12)
            throw new ReglaNegocioException("El mes debe estar entre 1 y 12.");
        if (anio is < 2000 or > 2100)
            throw new ReglaNegocioException("El año debe estar entre 2000 y 2100.");
        return new PeriodoMensual(anio, mes);
    }

    public static PeriodoMensual DeFecha(DateOnly fecha) => new(fecha.Year, fecha.Month);

    public DateOnly Inicio => new(Anio, Mes, 1);
    public DateOnly Fin => Inicio.AddMonths(1).AddDays(-1);

    public bool Contiene(DateOnly fecha) => fecha.Year == Anio && fecha.Month == Mes;

    /// <summary>El periodo que queda <paramref name="meses"/> meses antes (o después, si es negativo).</summary>
    public PeriodoMensual Retroceder(int meses) => DeFecha(Inicio.AddMonths(-meses));
}
