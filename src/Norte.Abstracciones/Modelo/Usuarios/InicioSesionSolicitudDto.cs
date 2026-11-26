using System.ComponentModel.DataAnnotations;

namespace Norte.Abstracciones.Modelo.Usuarios;

/// <summary>Credenciales para iniciar sesión. No se valida el largo: no se dan pistas.</summary>
public record InicioSesionSolicitudDto(
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    string Correo,

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    string Contrasena);
