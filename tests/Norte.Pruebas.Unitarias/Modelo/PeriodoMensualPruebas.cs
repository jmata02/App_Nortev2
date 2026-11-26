using Norte.Abstracciones.Modelo.Comun;
using Norte.Abstracciones.Modelo.Reportes;

namespace Norte.Pruebas.Unitarias.Modelo;

public class PeriodoMensualPruebas
{
    [Fact]
    public void Fin_FebreroDeAnioBisiesto_Es29()
    {
        Assert.Equal(new DateOnly(2028, 2, 29), PeriodoMensual.Crear(2028, 2).Fin);
    }

    [Fact]
    public void Retroceder_CruzaElCambioDeAnioEnAmbasDirecciones()
    {
        var enero = PeriodoMensual.Crear(2027, 1);

        Assert.Equal(PeriodoMensual.Crear(2026, 12), enero.Retroceder(1));
        Assert.Equal(PeriodoMensual.Crear(2027, 2), enero.Retroceder(-1));
    }

    [Fact]
    public void Retroceder_NoCambiaElPeriodoOriginal()
    {
        var noviembre = PeriodoMensual.Crear(2026, 11);

        noviembre.Retroceder(3);

        Assert.Equal((2026, 11), (noviembre.Anio, noviembre.Mes));
    }

    [Theory]
    [InlineData(2101, 1)]
    [InlineData(2026, 0)]
    public void Crear_FueraDeRango_LanzaReglaNegocio(int anio, int mes)
    {
        Assert.Throws<ReglaNegocioException>(() => PeriodoMensual.Crear(anio, mes));
    }
}
