namespace Norte.Abstracciones.Modelo.Usuarios;

/// <summary>Respuesta de registro e inicio de sesión. Nunca incluye el hash de la contraseña.</summary>
public record SesionRespuestaDto(string Token, DateTime ExpiraUtc, Guid UsuarioId, string Nombre, string Correo);
