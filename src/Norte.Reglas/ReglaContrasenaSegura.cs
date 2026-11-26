using Norte.Abstracciones.Reglas;

namespace Norte.Reglas;

/// <summary>
/// Paradigma funcional: los requisitos son datos (una función que revisa y un mensaje) y
/// la validación es una consulta LINQ pura sobre ellos: filtra los que no se cumplen
/// (Where) y se queda con sus mensajes (Select). No modifica nada ni guarda estado.
/// Para agregar un requisito basta con agregar una línea a la lista.
/// </summary>
public class ReglaContrasenaSegura : IReglaContrasenaSegura
{
    public const int LargoMinimo = 8;

    private static readonly IReadOnlyList<(Func<string, bool> Cumple, string Mensaje)> Requisitos =
    [
        (c => c.Length >= LargoMinimo, $"Debe tener al menos {LargoMinimo} caracteres."),
        (c => c.Any(char.IsLetter), "Debe tener al menos una letra."),
        (c => c.Any(char.IsDigit), "Debe tener al menos un número."),
        (c => c == c.Trim(), "No puede empezar ni terminar con espacios.")
    ];

    public IReadOnlyList<string> ObtenerIncumplimientos(string contrasena) =>
        Requisitos
            .Where(requisito => !requisito.Cumple(contrasena ?? string.Empty))
            .Select(requisito => requisito.Mensaje)
            .ToList();
}
