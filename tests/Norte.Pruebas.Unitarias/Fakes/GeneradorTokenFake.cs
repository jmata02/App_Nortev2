using Norte.Abstracciones.Modelo.Usuarios;
using Norte.Abstracciones.Servicios;

namespace Norte.Pruebas.Unitarias.Fakes;

public class GeneradorTokenFake : IGeneradorToken
{
    public TokenGenerado Generar(Usuario usuario) =>
        new("token-de-" + usuario.Id, DateTime.UtcNow.AddMinutes(120));
}
