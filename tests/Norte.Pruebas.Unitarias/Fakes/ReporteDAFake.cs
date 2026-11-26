using Norte.Abstracciones.DA;
using Norte.Abstracciones.Modelo.Categorias;
using Norte.Abstracciones.Modelo.Movimientos;

namespace Norte.Pruebas.Unitarias.Fakes;

/// <summary>DA en memoria para el Flujo de reportes. Filtra igual que el DA real.</summary>
public class ReporteDAFake : IReporteDA
{
    public List<Transaccion> Transacciones { get; } = [];
    public List<Categoria> Categorias { get; } = [];
    public int DeudasActivas { get; set; }
    public int MetasEnCurso { get; set; }

    // Lo que recibieron los conteos, para comprobar que el Flujo pasa el usuario y el "hoy" correctos.
    public Guid? UsuarioDeudasRecibido { get; private set; }
    public (Guid Usuario, DateOnly Hoy)? ConsultaMetasRecibida { get; private set; }

    public Task<IReadOnlyList<Transaccion>> ObtenerTransaccionesAsync(Guid usuarioId, DateOnly desde, DateOnly hasta) =>
        Task.FromResult<IReadOnlyList<Transaccion>>(
            Transacciones.Where(t => t.UsuarioId == usuarioId && t.Fecha >= desde && t.Fecha <= hasta).ToList());

    public Task<IReadOnlyList<Categoria>> ObtenerCategoriasAsync() =>
        Task.FromResult<IReadOnlyList<Categoria>>(Categorias);

    public Task<int> ContarDeudasActivasAsync(Guid usuarioId)
    {
        UsuarioDeudasRecibido = usuarioId;
        return Task.FromResult(DeudasActivas);
    }

    public Task<int> ContarMetasEnCursoAsync(Guid usuarioId, DateOnly hoy)
    {
        ConsultaMetasRecibida = (usuarioId, hoy);
        return Task.FromResult(MetasEnCurso);
    }
}
