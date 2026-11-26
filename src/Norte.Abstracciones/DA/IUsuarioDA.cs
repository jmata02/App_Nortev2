using Norte.Abstracciones.Modelo.Usuarios;

namespace Norte.Abstracciones.DA;

/// <summary>Acceso a la tabla usuarios. El correo siempre llega ya normalizado.</summary>
public interface IUsuarioDA
{
    Task<Usuario?> ObtenerPorCorreoAsync(string correoNormalizado);
    Task<bool> ExisteCorreoAsync(string correoNormalizado);

    /// <summary>Guarda el usuario. Lanza ConflictoException si el correo ya existe.</summary>
    Task AgregarAsync(Usuario usuario);
}
