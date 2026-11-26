using Norte.Abstracciones.Modelo.Comun;
using Norte.Abstracciones.Modelo.Usuarios;
using Norte.Flujo;
using Norte.Pruebas.Unitarias.Fakes;
using Norte.Reglas;

namespace Norte.Pruebas.Unitarias.Flujo;

public class AutenticacionFlujoPruebas
{
    private readonly UsuarioDAFake _usuarioDA = new();
    private readonly AutenticacionFlujo _flujo;

    public AutenticacionFlujoPruebas()
    {
        // Reglas reales (son puras); DA, hasher y token falsos.
        _flujo = new AutenticacionFlujo(
            _usuarioDA, new HasherContrasenaFake(), new GeneradorTokenFake(),
            new ReglaContrasenaSegura(), new ReglaCorreoNormalizado());
    }

    [Fact]
    public async Task RegistrarAsync_DatosValidos_GuardaCorreoNormalizadoYHashYDevuelveToken()
    {
        var sesion = await _flujo.RegistrarAsync(new RegistroSolicitudDto("Ana", " Ana@Norte.com ", "Clave1234"));

        var guardado = Assert.Single(_usuarioDA.Usuarios);
        Assert.Equal("ana@norte.com", guardado.Correo);
        Assert.NotEqual("Clave1234", guardado.HashContrasena);
        Assert.Equal(DateTimeKind.Utc, guardado.FechaRegistroUtc.Kind);
        Assert.Equal(guardado.Id, sesion.UsuarioId);
        Assert.Equal("token-de-" + guardado.Id, sesion.Token);
    }

    [Fact]
    public async Task RegistrarAsync_CorreoRepetidoConOtrasMayusculas_LanzaConflicto()
    {
        await _flujo.RegistrarAsync(new RegistroSolicitudDto("Ana", "ana@norte.com", "Clave1234"));

        await Assert.ThrowsAsync<ConflictoException>(() =>
            _flujo.RegistrarAsync(new RegistroSolicitudDto("Otra", "ANA@norte.com", "Clave1234")));
        Assert.Single(_usuarioDA.Usuarios);
    }

    [Fact]
    public async Task RegistrarAsync_ContrasenaDebil_LanzaReglaNegocioYNoGuarda()
    {
        await Assert.ThrowsAsync<ReglaNegocioException>(() =>
            _flujo.RegistrarAsync(new RegistroSolicitudDto("Ana", "ana@norte.com", "abc")));
        Assert.Empty(_usuarioDA.Usuarios);
    }

    [Fact]
    public async Task IniciarSesionAsync_CredencialesCorrectasConCorreoEnMayusculas_DevuelveSesion()
    {
        var registro = await _flujo.RegistrarAsync(new RegistroSolicitudDto("Ana", "ana@norte.com", "Clave1234"));

        var sesion = await _flujo.IniciarSesionAsync(new InicioSesionSolicitudDto("ANA@NORTE.COM", "Clave1234"));

        Assert.Equal(registro.UsuarioId, sesion.UsuarioId);
    }

    [Fact]
    public async Task IniciarSesionAsync_ContrasenaIncorrecta_LanzaCredencialesInvalidas()
    {
        await _flujo.RegistrarAsync(new RegistroSolicitudDto("Ana", "ana@norte.com", "Clave1234"));

        await Assert.ThrowsAsync<CredencialesInvalidasException>(() =>
            _flujo.IniciarSesionAsync(new InicioSesionSolicitudDto("ana@norte.com", "Otra12345")));
    }

    [Fact]
    public async Task IniciarSesionAsync_CorreoInexistente_LanzaElMismoErrorQueContrasenaIncorrecta()
    {
        var error = await Assert.ThrowsAsync<CredencialesInvalidasException>(() =>
            _flujo.IniciarSesionAsync(new InicioSesionSolicitudDto("nadie@norte.com", "Clave1234")));

        Assert.Equal(new CredencialesInvalidasException().Message, error.Message);
    }
}
