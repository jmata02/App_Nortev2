using Norte.Abstracciones.DA;
using Norte.Abstracciones.Modelo.Usuarios;

namespace Norte.Pruebas.Unitarias.Fakes;

/// <summary>DA en memoria: las pruebas del Flujo no tocan la base de datos.</summary>
public class UsuarioDAFake : IUsuarioDA
{
    public List<Usuario> Usuarios { get; } = [];

    public Task<Usuario?> ObtenerPorCorreoAsync(string correoNormalizado) =>
        Task.FromResult(Usuarios.FirstOrDefault(u => u.Correo == correoNormalizado));

    public Task<bool> ExisteCorreoAsync(string correoNormalizado) =>
        Task.FromResult(Usuarios.Any(u => u.Correo == correoNormalizado));

    public Task AgregarAsync(Usuario usuario)
    {
        Usuarios.Add(usuario);
        return Task.CompletedTask;
    }
}
