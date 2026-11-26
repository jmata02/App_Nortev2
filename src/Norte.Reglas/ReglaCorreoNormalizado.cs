using Norte.Abstracciones.Reglas;

namespace Norte.Reglas;

/// <summary>Función pura: la misma entrada siempre da la misma salida y no cambia la entrada.</summary>
public class ReglaCorreoNormalizado : IReglaCorreoNormalizado
{
    public string Normalizar(string correo) => (correo ?? string.Empty).Trim().ToLowerInvariant();
}
