using Microsoft.Extensions.DependencyInjection.Extensions;
using Norte.Abstracciones.DA;
using Norte.Abstracciones.Flujo;
using Norte.Abstracciones.Reglas;
using Norte.DA;
using Norte.Flujo;
using Norte.Reglas;

namespace Norte.Api.Configuracion.Modulos;

/// <summary>
/// Dashboard y reportes (consultas en paralelo con IDbContextFactory).
/// Responsable: Jurgen.
/// </summary>
public static class ModuloReportes
{
    public static IServiceCollection AgregarModuloReportes(this IServiceCollection servicios)
    {
        servicios.AddScoped<IReporteDA, ReporteDA>();
        servicios.AddScoped<IReporteFlujo, ReporteFlujo>();
        servicios.AddScoped<IReglaResumenFinanciero, ReglaResumenFinanciero>();

        // Reloj del sistema. Las pruebas usan uno fijo para que "hoy" no cambie.
        servicios.TryAddSingleton(TimeProvider.System);
        return servicios;
    }
}
