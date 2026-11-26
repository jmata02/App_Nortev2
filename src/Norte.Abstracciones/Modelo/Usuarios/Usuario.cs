using Norte.Abstracciones.Modelo.Comun;

namespace Norte.Abstracciones.Modelo.Usuarios;

/// <summary>
/// Dueño de toda la información financiera. Toda consulta se filtra por UsuarioId
/// para aislar los datos entre usuarios (riesgo 7).
///
/// Paradigma orientado a objetos: los setters son privados y la única forma de crear un
/// usuario es <see cref="Crear"/>, que protege sus reglas (encapsulamiento). Así no puede
/// existir un usuario sin correo, sin hash o con una fecha que no esté en UTC.
/// </summary>
public class Usuario
{
    public const int LargoMinimoNombre = 2;

    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Correo { get; private set; } = string.Empty;
    public string HashContrasena { get; private set; } = string.Empty;

    /// <summary>Siempre en UTC (riesgo 4).</summary>
    public DateTime FechaRegistroUtc { get; private set; }

    // EF Core usa este constructor para leer usuarios de la base.
    private Usuario() { }

    /// <summary>
    /// Fábrica: crea un usuario nuevo validando sus datos. Recibe el correo ya normalizado
    /// y la contraseña ya convertida en hash: la entidad nunca ve la contraseña en claro.
    /// </summary>
    public static Usuario Crear(string nombre, string correoNormalizado, string hashContrasena, DateTime fechaRegistroUtc)
    {
        // Se mide el nombre ya recortado: "  a  " no cuenta como un nombre de 2 letras.
        if (string.IsNullOrWhiteSpace(nombre) || nombre.Trim().Length < LargoMinimoNombre)
            throw new ReglaNegocioException($"El nombre debe tener al menos {LargoMinimoNombre} caracteres.");
        if (string.IsNullOrWhiteSpace(correoNormalizado))
            throw new ReglaNegocioException("El correo es obligatorio.");
        if (string.IsNullOrWhiteSpace(hashContrasena))
            throw new ReglaNegocioException("La contraseña es obligatoria.");
        if (fechaRegistroUtc.Kind != DateTimeKind.Utc)
            throw new ReglaNegocioException("La fecha de registro debe estar en UTC.");

        return new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = nombre.Trim(),
            Correo = correoNormalizado,
            HashContrasena = hashContrasena,
            FechaRegistroUtc = fechaRegistroUtc
        };
    }
}
