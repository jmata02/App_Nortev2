namespace Norte.Abstracciones.Servicios;

/// <summary>
/// Convierte contraseñas en hash y las verifica. La implementación usa el PasswordHasher de
/// ASP.NET Core Identity; el Flujo solo conoce esta interfaz y se puede probar con un falso.
/// </summary>
public interface IHasherContrasena
{
    string Hashear(string contrasena);
    bool Verificar(string hash, string contrasena);
}
