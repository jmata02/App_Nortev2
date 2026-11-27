using Norte.Abstracciones.DA;
using Norte.Abstracciones.Modelo.Categorias;
using Norte.Abstracciones.Modelo.Movimientos;

namespace Norte.Pruebas.Unitarias.Fakes;

/// <summary>
/// Demuestra que el Flujo pide las consultas EN PARALELO (Task.WhenAll): ninguna consulta
/// responde hasta que todas las esperadas ya se pidieron. Si el Flujo las pidiera una por una
/// (await tras await), la primera nunca terminaría y la prueba fallaría por tiempo.
/// </summary>
public class ReporteDAParaleloFake : IReporteDA
{
    private readonly int _consultasEsperadas;
    private readonly TaskCompletionSource _todasPedidas = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private int _consultasPedidas;

    public ReporteDAParaleloFake(int consultasEsperadas)
    {
        _consultasEsperadas = consultasEsperadas;
    }

    /// <summary>Cuántas consultas estaban pedidas a la vez antes de que alguna respondiera.</summary>
    public int ConsultasSimultaneas => _consultasPedidas;

    private async Task<T> ResponderCuandoTodasEstenPedidasAsync<T>(T resultado)
    {
        if (Interlocked.Increment(ref _consultasPedidas) == _consultasEsperadas)
            _todasPedidas.SetResult();
        await _todasPedidas.Task.WaitAsync(TimeSpan.FromSeconds(5));
        return resultado;
    }

    public Task<IReadOnlyList<Transaccion>> ObtenerTransaccionesAsync(Guid usuarioId, DateOnly desde, DateOnly hasta) =>
        ResponderCuandoTodasEstenPedidasAsync<IReadOnlyList<Transaccion>>([]);

    public Task<IReadOnlyList<Categoria>> ObtenerCategoriasAsync() =>
        ResponderCuandoTodasEstenPedidasAsync<IReadOnlyList<Categoria>>([]);

    public Task<int> ContarDeudasActivasAsync(Guid usuarioId) => ResponderCuandoTodasEstenPedidasAsync(0);

    public Task<int> ContarMetasEnCursoAsync(Guid usuarioId, DateOnly hoy) => ResponderCuandoTodasEstenPedidasAsync(0);
}
