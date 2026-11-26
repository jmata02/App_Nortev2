namespace Norte.Abstracciones.Reglas;

/// <summary>Requisitos mínimos de una contraseña nueva.</summary>
public interface IReglaContrasenaSegura
{
    /// <summary>Devuelve los requisitos que no se cumplen; vacío si la contraseña es válida.</summary>
    IReadOnlyList<string> ObtenerIncumplimientos(string contrasena);
}
