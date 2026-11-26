using Norte.Abstracciones.Modelo.Usuarios;

namespace Norte.Abstracciones.Servicios;

/// <summary>Emite el JWT de un usuario, con su Id en el claim "sub" (lo lee IUsuarioActual).</summary>
public interface IGeneradorToken
{
    TokenGenerado Generar(Usuario usuario);
}
