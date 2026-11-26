namespace Norte.Pruebas.Unitarias.Fakes;

/// <summary>Reloj que siempre devuelve la misma fecha (UTC), para que "hoy" no cambie en las pruebas.</summary>
public class RelojFijo : TimeProvider
{
    private readonly DateTimeOffset _ahora;

    public RelojFijo(int anio, int mes, int dia)
    {
        _ahora = new DateTimeOffset(anio, mes, dia, 12, 0, 0, TimeSpan.Zero);
    }

    public override DateTimeOffset GetUtcNow() => _ahora;
}
