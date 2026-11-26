using Microsoft.AspNetCore.Identity;
using Norte.Abstracciones.Modelo.Usuarios;
using Norte.Abstracciones.Servicios;

namespace Norte.Api.Servicios;

/// <summary>
/// Usa el PasswordHasher de ASP.NET Core Identity (PBKDF2 con sal aleatoria). Se usa solo
/// este componente de Identity, sin sus tablas (decisión registrada en la bitácora).
/// </summary>
public class HasherContrasena : IHasherContrasena
{
    private readonly PasswordHasher<Usuario> _hasher = new();

    // PasswordHasher pide un usuario por si alguien quiere personalizarlo, pero la
    // implementación por defecto no lo usa; por eso se pasa null.
    public string Hashear(string contrasena) => _hasher.HashPassword(null!, contrasena);

    public bool Verificar(string hash, string contrasena) =>
        _hasher.VerifyHashedPassword(null!, hash, contrasena) != PasswordVerificationResult.Failed;
}
