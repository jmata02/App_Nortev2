namespace Norte.Pruebas.Unitarias.Fakes;

/// <summary>Reloj que siempre devuelve el mismo instante, para que "hoy" no cambie en las pruebas.</summary>
public class RelojFijo : TimeProvider
{
    private readonly DateTimeOffset _ahora;

    /// <summary>Mediodía UTC de esa fecha.</summary>
    public RelojFijo(int anio, int mes, int dia)
        : this(new DateTimeOffset(anio, mes, dia, 12, 0, 0, TimeSpan.Zero))
    {
    }

    /// <summary>Un instante exacto con su zona horaria (por ejemplo, la hora de Costa Rica, -06:00).</summary>
    public RelojFijo(DateTimeOffset ahora)
    {
        _ahora = ahora;
    }

    public override DateTimeOffset GetUtcNow() => _ahora.ToUniversalTime();
}
