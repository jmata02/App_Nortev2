using Norte.Abstracciones.Modelo.Comun;
using Norte.Abstracciones.Modelo.Usuarios;

namespace Norte.Pruebas.Unitarias.Modelo;

public class UsuarioPruebas
{
    [Fact]
    public void Crear_DatosValidos_AsignaIdYRecortaElNombre()
    {
        var ahora = DateTime.UtcNow;

        var usuario = Usuario.Crear("  Ana  ", "ana@norte.com", "hash", ahora);

        Assert.NotEqual(Guid.Empty, usuario.Id);
        Assert.Equal("Ana", usuario.Nombre);
        Assert.Equal(ahora, usuario.FechaRegistroUtc);
    }

    [Theory]
    [InlineData("", "ana@norte.com", "hash")]
    [InlineData("  a  ", "ana@norte.com", "hash")]
    [InlineData("Ana", " ", "hash")]
    [InlineData("Ana", "ana@norte.com", "")]
    public void Crear_DatoObligatorioVacio_LanzaReglaNegocio(string nombre, string correo, string hash)
    {
        Assert.Throws<ReglaNegocioException>(() => Usuario.Crear(nombre, correo, hash, DateTime.UtcNow));
    }

    [Fact]
    public void Crear_FechaQueNoEstaEnUtc_LanzaReglaNegocio()
    {
        Assert.Throws<ReglaNegocioException>(() => Usuario.Crear("Ana", "ana@norte.com", "hash", DateTime.Now));
    }
}
