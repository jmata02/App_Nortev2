namespace Norte.Abstracciones.Reglas;

/// <summary>
/// Pone el correo en una forma única (sin espacios y en minúsculas) antes de buscarlo o
/// guardarlo, para que "Ana@Correo.com" y "ana@correo.com" sean la misma cuenta.
/// </summary>
public interface IReglaCorreoNormalizado
{
    string Normalizar(string correo);
}
