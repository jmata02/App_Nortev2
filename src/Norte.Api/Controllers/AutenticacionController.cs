using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Norte.Abstracciones.API;
using Norte.Abstracciones.Flujo;
using Norte.Abstracciones.Modelo.Usuarios;

namespace Norte.Api.Controllers;

/// <summary>Registro e inicio de sesión. Es público: todavía no hay usuario autenticado.</summary>
[ApiController]
[Route("api/autenticacion")]
[Produces("application/json")]
[AllowAnonymous]
public class AutenticacionController : ControllerBase, IAutenticacionController
{
    private readonly IAutenticacionFlujo _autenticacionFlujo;

    public AutenticacionController(IAutenticacionFlujo autenticacionFlujo)
    {
        _autenticacionFlujo = autenticacionFlujo;
    }

    /// <summary>Crea la cuenta y devuelve la sesión iniciada.</summary>
    [HttpPost("registro")]
    [ProducesResponseType<SesionRespuestaDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegistrarAsync([FromBody] RegistroSolicitudDto solicitud) =>
        StatusCode(StatusCodes.Status201Created, await _autenticacionFlujo.RegistrarAsync(solicitud));

    [HttpPost("inicio-sesion")]
    [ProducesResponseType<SesionRespuestaDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> IniciarSesionAsync([FromBody] InicioSesionSolicitudDto solicitud) =>
        Ok(await _autenticacionFlujo.IniciarSesionAsync(solicitud));
}
