using Norte.Reglas;

namespace Norte.Pruebas.Unitarias.Reglas;

public class ReglaCorreoNormalizadoPruebas
{
    private readonly ReglaCorreoNormalizado _regla = new();

    [Fact]
    public void Normalizar_QuitaEspaciosYPasaAMinusculas_SinCambiarLaEntrada()
    {
        var original = "  Ana@Correo.COM ";

        var normalizado = _regla.Normalizar(original);

        Assert.Equal("ana@correo.com", normalizado);
        Assert.Equal("  Ana@Correo.COM ", original);
    }

    [Fact]
    public void Normalizar_MismaEntrada_SiempreDaLaMismaSalida()
    {
        Assert.Equal(_regla.Normalizar("X@Y.com"), _regla.Normalizar("X@Y.com"));
    }
}
