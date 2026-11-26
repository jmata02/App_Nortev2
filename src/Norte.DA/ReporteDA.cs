using Microsoft.EntityFrameworkCore;
using Norte.Abstracciones.DA;
using Norte.Abstracciones.Modelo.Categorias;
using Norte.Abstracciones.Modelo.Movimientos;
using Norte.DA.Contexto;

namespace Norte.DA;

/// <summary>
/// Lecturas de los reportes. A diferencia de los otros DA, recibe la FÁBRICA de contextos:
/// cada método crea su propio NorteDbContext, porque un DbContext no se puede usar desde
/// varias tareas a la vez y el Flujo pide estas consultas en paralelo (Task.WhenAll).
/// </summary>
public class ReporteDA : IReporteDA
{
    private readonly IDbContextFactory<NorteDbContext> _fabricaContextos;

    public ReporteDA(IDbContextFactory<NorteDbContext> fabricaContextos)
    {
        _fabricaContextos = fabricaContextos;
    }

    public async Task<IReadOnlyList<Transaccion>> ObtenerTransaccionesAsync(Guid usuarioId, DateOnly desde, DateOnly hasta)
    {
        await using var contexto = await _fabricaContextos.CreateDbContextAsync();
        return await contexto.Transacciones.AsNoTracking()
            .Where(t => t.UsuarioId == usuarioId && t.Fecha >= desde && t.Fecha <= hasta)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Categoria>> ObtenerCategoriasAsync()
    {
        await using var contexto = await _fabricaContextos.CreateDbContextAsync();
        return await contexto.Categorias.AsNoTracking().ToListAsync();
    }

    public async Task<int> ContarDeudasActivasAsync(Guid usuarioId)
    {
        await using var contexto = await _fabricaContextos.CreateDbContextAsync();
        return await contexto.Deudas.AsNoTracking()
            .CountAsync(d => d.UsuarioId == usuarioId && d.MontoPendiente > 0);
    }

    public async Task<int> ContarMetasEnCursoAsync(Guid usuarioId, DateOnly hoy)
    {
        await using var contexto = await _fabricaContextos.CreateDbContextAsync();
        return await contexto.MetasAhorro.AsNoTracking()
            .CountAsync(m => m.UsuarioId == usuarioId && m.FechaLimite >= hoy);
    }
}
