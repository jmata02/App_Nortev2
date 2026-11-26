using Norte.Reglas;

namespace Norte.Pruebas.Unitarias.Reglas;

public class ReglaContrasenaSeguraPruebas
{
    private readonly ReglaContrasenaSegura _regla = new();

    [Theory]
    [InlineData("Clave1234")]
    [InlineData("a1b2c3d4")]
    [InlineData("contraseña con espacios 9")]
    public void ObtenerIncumplimientos_ContrasenaValida_DevuelveVacio(string contrasena)
    {
        Assert.Empty(_regla.ObtenerIncumplimientos(contrasena));
    }

    [Theory]
    [InlineData("Clave12", "al menos 8 caracteres")]
    [InlineData("SoloLetras", "un número")]
    [InlineData("12345678", "una letra")]
    [InlineData(" Clave1234", "espacios")]
    public void ObtenerIncumplimientos_FaltaUnRequisito_LoInforma(string contrasena, string mensajeEsperado)
    {
        var incumplimientos = _regla.ObtenerIncumplimientos(contrasena);

        Assert.Single(incumplimientos);
        Assert.Contains(mensajeEsperado, incumplimientos[0]);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void ObtenerIncumplimientos_Vacia_InformaLargoLetraYNumero(string? contrasena)
    {
        Assert.Equal(3, _regla.ObtenerIncumplimientos(contrasena!).Count);
    }
}
