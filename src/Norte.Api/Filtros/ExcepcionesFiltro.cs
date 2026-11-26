using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Norte.Abstracciones.Modelo.Comun;

namespace Norte.Api.Filtros;

/// <summary>
/// Traduce las excepciones de negocio a códigos HTTP. Los controllers no usan try/catch.
/// </summary>
public class ExcepcionesFiltro : IExceptionFilter
{
    public void OnException(ExceptionContext contexto)
    {
        IActionResult? resultado = contexto.Exception switch
        {
            RecursoNoEncontradoException e => new ObjectResult(Cuerpo(e)) { StatusCode = StatusCodes.Status404NotFound },
            ReglaNegocioException e => new ObjectResult(Cuerpo(e)) { StatusCode = StatusCodes.Status400BadRequest },
            ConflictoException e => new ObjectResult(Cuerpo(e)) { StatusCode = StatusCodes.Status409Conflict },
            CredencialesInvalidasException e => new ObjectResult(Cuerpo(e)) { StatusCode = StatusCodes.Status401Unauthorized },
            DbUpdateConcurrencyException => new ObjectResult(new { mensaje = "El registro cambió mientras se editaba. Recargue e intente de nuevo." })
            { StatusCode = StatusCodes.Status409Conflict },
            UnauthorizedAccessException => new UnauthorizedResult(),
            _ => null
        };

        if (resultado is null)
            return;

        contexto.Result = resultado;
        contexto.ExceptionHandled = true;
    }

    private static object Cuerpo(Exception excepcion) => new { mensaje = excepcion.Message };
}
