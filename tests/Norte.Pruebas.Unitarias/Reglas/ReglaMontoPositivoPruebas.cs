using Norte.Reglas;

namespace Norte.Pruebas.Unitarias.Reglas;

public class ReglaMontoPositivoPruebas
{
    private readonly ReglaMontoPositivo _regla = new();

    [Theory]
    [InlineData(1)]
    [InlineData(15000.50)]
    public void EsValido_MontoPositivoConDosDecimales_DevuelveTrue(decimal monto) =>
        Assert.True(_regla.EsValido(monto));

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    [InlineData(10.555)]
    public void EsValido_MontoCeroNegativoOConMasDeDosDecimales_DevuelveFalse(decimal monto) =>
        Assert.False(_regla.EsValido(monto));

    // Los bordes de los céntimos van como decimal exacto (no como double).
    public static TheoryData<decimal, bool> LimitesDeCentimos => new()
    {
        { 0.01m, true },     // el monto más pequeño válido: un céntimo
        { 0.001m, false },   // menos de un céntimo
        { -0.01m, false },
        { 10.500m, true },   // un cero de más no es un tercer decimal real
        { 999_999_999_999.99m, true },
    };

    [Theory]
    [MemberData(nameof(LimitesDeCentimos))]
    public void EsValido_LimitesDeCentimos_ClasificaCorrectamente(decimal monto, bool esperado) =>
        Assert.Equal(esperado, _regla.EsValido(monto));
}
