using Norte.Abstracciones.Modelo.Comun;
using Norte.Abstracciones.Modelo.Usuarios;
using Norte.Flujo;
using Norte.Pruebas.Unitarias.Fakes;
using Norte.Reglas;

namespace Norte.Pruebas.Unitarias.Flujo;

/// <summary>
/// Riesgo 6 de la propuesta (condiciones de carrera): dos personas se registran con el mismo
/// correo exactamente al mismo tiempo.
/// </summary>
public class AutenticacionConcurrenciaPruebas
{
    [Fact]
    public async Task RegistrarAsync_DosRegistrosSimultaneosMismoCorreo_SeGuardaUnoYElOtroTerminaEnConflicto()
    {
        var usuarioDA = new UsuarioDASimultaneoFake(solicitudesEsperadas: 2);
        var flujo = new AutenticacionFlujo(
            usuarioDA, new HasherContrasenaFake(), new GeneradorTokenFake(),
            new ReglaContrasenaSegura(), new ReglaCorreoNormalizado());

        // Las dos solicitudes salen a la vez; ninguna espera a la otra.
        var primera = flujo.RegistrarAsync(new RegistroSolicitudDto("Ana", "ana@norte.com", "Clave1234"));
        var segunda = flujo.RegistrarAsync(new RegistroSolicitudDto("Ana Dos", "ANA@norte.com", "Clave1234"));
        var resultados = await Task.WhenAll(Resultado(primera), Resultado(segunda));

        Assert.Single(usuarioDA.Usuarios);
        Assert.Equal(1, resultados.Count(r => r is SesionRespuestaDto));
        Assert.Equal(1, resultados.Count(r => r is ConflictoException));
    }

    // Convierte la tarea en su resultado o en la excepción que lanzó, para comparar las dos.
    private static async Task<object> Resultado(Task<SesionRespuestaDto> tarea)
    {
        try
        {
            return await tarea;
        }
        catch (ConflictoException error)
        {
            return error;
        }
    }
}
