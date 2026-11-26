using Microsoft.AspNetCore.Mvc;
using Norte.Abstracciones.Modelo.Usuarios;

namespace Norte.Abstracciones.API;

public interface IAutenticacionController
{
    Task<IActionResult> RegistrarAsync(RegistroSolicitudDto solicitud);
    Task<IActionResult> IniciarSesionAsync(InicioSesionSolicitudDto solicitud);
}
