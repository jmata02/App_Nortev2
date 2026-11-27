using Norte.Abstracciones.DA;
using Norte.Abstracciones.Modelo.Comun;
using Norte.Abstracciones.Modelo.Usuarios;

namespace Norte.Pruebas.Unitarias.Fakes;

/// <summary>
/// Simula dos registros que llegan al mismo tiempo: ExisteCorreoAsync no responde hasta que las
/// dos solicitudes preguntaron, así las dos ven "no existe" (la peor carrera posible).
/// AgregarAsync se comporta como el índice único de PostgreSQL: el segundo choca y lanza
/// ConflictoException, igual que UsuarioDA con el error 23505.
/// </summary>
public class UsuarioDASimultaneoFake : IUsuarioDA
{
    private readonly int _solicitudesEsperadas;
    private readonly TaskCompletionSource _todasPreguntaron = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private readonly object _candado = new();
    private int _preguntas;

    public List<Usuario> Usuarios { get; } = [];

    public UsuarioDASimultaneoFake(int solicitudesEsperadas)
    {
        _solicitudesEsperadas = solicitudesEsperadas;
    }

    public async Task<bool> ExisteCorreoAsync(string correoNormalizado)
    {
        bool existe;
        lock (_candado)
        {
            existe = Usuarios.Any(u => u.Correo == correoNormalizado);
            if (++_preguntas == _solicitudesEsperadas)
                _todasPreguntaron.SetResult();
        }

        // Si el Flujo no corriera las solicitudes a la vez, esto se quedaría esperando: el
        // límite de tiempo hace que la prueba falle en vez de colgarse.
        await _todasPreguntaron.Task.WaitAsync(TimeSpan.FromSeconds(5));
        return existe;
    }

    public Task<Usuario?> ObtenerPorCorreoAsync(string correoNormalizado)
    {
        lock (_candado)
            return Task.FromResult(Usuarios.FirstOrDefault(u => u.Correo == correoNormalizado));
    }

    public Task AgregarAsync(Usuario usuario)
    {
        lock (_candado)
        {
            if (Usuarios.Any(u => u.Correo == usuario.Correo))
                throw new ConflictoException("Ya existe una cuenta con ese correo.");
            Usuarios.Add(usuario);
        }
        return Task.CompletedTask;
    }
}
