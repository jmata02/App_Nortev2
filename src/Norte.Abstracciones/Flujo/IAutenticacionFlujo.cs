using Norte.Abstracciones.Modelo.Usuarios;

namespace Norte.Abstracciones.Flujo;

public interface IAutenticacionFlujo
{
    Task<SesionRespuestaDto> RegistrarAsync(RegistroSolicitudDto solicitud);
    Task<SesionRespuestaDto> IniciarSesionAsync(InicioSesionSolicitudDto solicitud);
}
