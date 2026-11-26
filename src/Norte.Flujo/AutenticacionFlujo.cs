using Norte.Abstracciones.DA;
using Norte.Abstracciones.Flujo;
using Norte.Abstracciones.Modelo.Comun;
using Norte.Abstracciones.Modelo.Usuarios;
using Norte.Abstracciones.Reglas;
using Norte.Abstracciones.Servicios;

namespace Norte.Flujo;

/// <summary>
/// Casos de uso de registro e inicio de sesión. Solo conoce interfaces: no sabe que el hash
/// es de Identity ni que el token es JWT, por eso se prueba con falsos.
/// </summary>
public class AutenticacionFlujo : IAutenticacionFlujo
{
    private readonly IUsuarioDA _usuarioDA;
    private readonly IHasherContrasena _hasher;
    private readonly IGeneradorToken _generadorToken;
    private readonly IReglaContrasenaSegura _reglaContrasena;
    private readonly IReglaCorreoNormalizado _reglaCorreo;

    public AutenticacionFlujo(
        IUsuarioDA usuarioDA,
        IHasherContrasena hasher,
        IGeneradorToken generadorToken,
        IReglaContrasenaSegura reglaContrasena,
        IReglaCorreoNormalizado reglaCorreo)
    {
        _usuarioDA = usuarioDA;
        _hasher = hasher;
        _generadorToken = generadorToken;
        _reglaContrasena = reglaContrasena;
        _reglaCorreo = reglaCorreo;
    }

    public async Task<SesionRespuestaDto> RegistrarAsync(RegistroSolicitudDto solicitud)
    {
        var correo = _reglaCorreo.Normalizar(solicitud.Correo);

        var incumplimientos = _reglaContrasena.ObtenerIncumplimientos(solicitud.Contrasena);
        if (incumplimientos.Count > 0)
            throw new ReglaNegocioException("La contraseña no es segura. " + string.Join(" ", incumplimientos));

        if (await _usuarioDA.ExisteCorreoAsync(correo))
            throw new ConflictoException("Ya existe una cuenta con ese correo.");

        var usuario = Usuario.Crear(solicitud.Nombre, correo, _hasher.Hashear(solicitud.Contrasena), DateTime.UtcNow);
        await _usuarioDA.AgregarAsync(usuario);

        // El registro deja la sesión iniciada: devuelve el token de una vez.
        return CrearSesion(usuario);
    }

    public async Task<SesionRespuestaDto> IniciarSesionAsync(InicioSesionSolicitudDto solicitud)
    {
        var correo = _reglaCorreo.Normalizar(solicitud.Correo);
        var usuario = await _usuarioDA.ObtenerPorCorreoAsync(correo);

        // Mismo error si el correo no existe o si la contraseña no coincide.
        if (usuario is null || !_hasher.Verificar(usuario.HashContrasena, solicitud.Contrasena))
            throw new CredencialesInvalidasException();

        return CrearSesion(usuario);
    }

    private SesionRespuestaDto CrearSesion(Usuario usuario)
    {
        var token = _generadorToken.Generar(usuario);
        return new SesionRespuestaDto(token.Token, token.ExpiraUtc, usuario.Id, usuario.Nombre, usuario.Correo);
    }
}
