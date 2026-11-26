using System.ComponentModel.DataAnnotations;

namespace Norte.Abstracciones.Modelo.Usuarios;

/// <summary>Datos para crear una cuenta. [ApiController] responde 400 si no cumplen.</summary>
public record RegistroSolicitudDto(
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    string Nombre,

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [StringLength(150, ErrorMessage = "El correo no puede pasar de 150 caracteres.")]
    string Correo,

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, ErrorMessage = "La contraseña no puede pasar de 100 caracteres.")]
    string Contrasena);
