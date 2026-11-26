using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Norte.Abstracciones.Modelo.Usuarios;
using Norte.Abstracciones.Servicios;

namespace Norte.Api.Servicios;

/// <summary>
/// Firma el JWT con la misma configuración (Jwt:Emisor, Jwt:Audiencia, Jwt:Clave) que
/// Program.cs usa para validarlo.
/// </summary>
public class GeneradorTokenJwt : IGeneradorToken
{
    private readonly IConfiguration _configuracion;

    public GeneradorTokenJwt(IConfiguration configuracion)
    {
        _configuracion = configuracion;
    }

    public TokenGenerado Generar(Usuario usuario)
    {
        var minutos = _configuracion.GetValue("Jwt:MinutosExpiracion", 120);
        var expiraUtc = DateTime.UtcNow.AddMinutes(minutos);
        var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuracion["Jwt:Clave"]!));

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuracion["Jwt:Emisor"],
            Audience = _configuracion["Jwt:Audiencia"],
            Expires = expiraUtc,
            SigningCredentials = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(
            [
                // "sub" es el que lee UsuarioActual para filtrar los datos de cada usuario.
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
                new Claim(JwtRegisteredClaimNames.Name, usuario.Nombre),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ])
        };

        return new TokenGenerado(new JsonWebTokenHandler().CreateToken(descriptor), expiraUtc);
    }
}
