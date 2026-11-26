using Microsoft.EntityFrameworkCore;
using Norte.Abstracciones.DA;
using Norte.Abstracciones.Modelo.Comun;
using Norte.Abstracciones.Modelo.Usuarios;
using Norte.DA.Contexto;
using Npgsql;

namespace Norte.DA;

public class UsuarioDA : IUsuarioDA
{
    private readonly NorteDbContext _contexto;

    public UsuarioDA(NorteDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<Usuario?> ObtenerPorCorreoAsync(string correoNormalizado) =>
        await _contexto.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Correo == correoNormalizado);

    public async Task<bool> ExisteCorreoAsync(string correoNormalizado) =>
        await _contexto.Usuarios.AsNoTracking().AnyAsync(u => u.Correo == correoNormalizado);

    public async Task AgregarAsync(Usuario usuario)
    {
        _contexto.Usuarios.Add(usuario);
        try
        {
            await _contexto.SaveChangesAsync();
        }
        // Dos registros simultáneos con el mismo correo pueden pasar los dos la revisión
        // del Flujo; el índice único de PostgreSQL detiene al segundo (riesgo 6).
        catch (DbUpdateException error) when (error.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new ConflictoException("Ya existe una cuenta con ese correo.");
        }
    }
}
