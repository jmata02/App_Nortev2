using Norte.Abstracciones.Modelo.Categorias;
using Norte.Abstracciones.Modelo.Movimientos;

namespace Norte.Abstracciones.DA;

/// <summary>
/// Lecturas para los reportes. Cada método abre su propio contexto, así el Flujo puede
/// pedir varias piezas a la vez con Task.WhenAll.
/// </summary>
public interface IReporteDA
{
    Task<IReadOnlyList<Transaccion>> ObtenerTransaccionesAsync(Guid usuarioId, DateOnly desde, DateOnly hasta);
    Task<IReadOnlyList<Categoria>> ObtenerCategoriasAsync();

    /// <summary>Deudas con saldo pendiente mayor que cero (definición a confirmar con Carlos).</summary>
    Task<int> ContarDeudasActivasAsync(Guid usuarioId);

    /// <summary>Metas cuya fecha límite no ha pasado (definición a confirmar con Jessica).</summary>
    Task<int> ContarMetasEnCursoAsync(Guid usuarioId, DateOnly hoy);
}
