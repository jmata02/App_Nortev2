using Norte.Abstracciones.DA;
using Norte.Abstracciones.Flujo;
using Norte.Abstracciones.Reglas;
using Norte.Abstracciones.Servicios;
using Norte.Api.Servicios;
using Norte.DA;
using Norte.Flujo;
using Norte.Reglas;

namespace Norte.Api.Configuracion.Modulos;

/// <summary>
/// Registro, inicio de sesión y generación del token JWT.
/// Responsable: Jurgen.
/// </summary>
public static class ModuloAutenticacion
{
    public static IServiceCollection AgregarModuloAutenticacion(this IServiceCollection servicios)
    {
        servicios.AddScoped<IUsuarioDA, UsuarioDA>();
        servicios.AddScoped<IAutenticacionFlujo, AutenticacionFlujo>();
        servicios.AddScoped<IReglaContrasenaSegura, ReglaContrasenaSegura>();
        servicios.AddScoped<IReglaCorreoNormalizado, ReglaCorreoNormalizado>();

        // Sin estado por solicitud: una sola instancia sirve para toda la aplicación.
        servicios.AddSingleton<IHasherContrasena, HasherContrasena>();
        servicios.AddSingleton<IGeneradorToken, GeneradorTokenJwt>();
        return servicios;
    }
}
