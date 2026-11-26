# Evidencia por paradigma

El enunciado pide, para cada paradigma: en qué módulo se aplica, qué problema resuelve,
qué tecnología usa, qué fragmento de código lo evidencia y por qué fue adecuado.
Si un paradigma no tiene evidencia clara, no cuenta. Cada responsable llena su sección
conforme avanza; esta tabla pasa casi tal cual al documento final.

| Paradigma | Responsable | Módulo | Evidencia en el código | Justificación |
|-----------|-------------|--------|------------------------|---------------|
| Orientado a objetos | Jessica | Movimientos, Metas | | |
| Funcional | Jurgen | Dashboard y reportes | `Norte.Reglas/ReglaResumenFinanciero.cs` → `CalcularTotales`, `CalcularGastoPorCategoria`, `CalcularTendencia` (`Where`, `GroupBy`, `Select`, `Aggregate`) | Los reportes son transformaciones de una colección de movimientos en resúmenes: con funciones puras salen siempre igual, no alteran los datos y se prueban sin base de datos |
| Concurrente | Carlos | Deudas y pagos recurrentes | | |
| Orientado a eventos | Jessica | Movimientos, Metas (web) | | |
| Reactivo | Mauricio | Presupuestos (web) | | |

## Funcional (Jurgen)

- **Módulo:** Dashboard y reportes. Lo usan los endpoints `GET /api/reportes/resumen`,
  `/gasto-por-categoria` y `/tendencia`, y la página del Dashboard en la web.
- **Problema que resuelve:** convertir la lista de movimientos de un usuario en la información
  que se ve en el Dashboard: total de ingresos y gastos del mes, variación del balance contra el
  mes anterior, gasto por categoría con su porcentaje y la tendencia de los últimos meses.
- **Tecnología:** C# con LINQ y expresiones lambda (`Where`, `GroupBy`, `Select`, `Aggregate`,
  `Sum`), `record` inmutables (`TotalesFinancieros`, `PeriodoMensual`) y la expresión `with`.
- **Archivo y método:** `src/Norte.Reglas/ReglaResumenFinanciero.cs` → `CalcularTotales` y
  `CalcularGastoPorCategoria` (también `CalcularTendencia` y `CalcularVariacionPorcentual`).
- **Fragmento:**

```csharp
public TotalesFinancieros CalcularTotales(IEnumerable<Transaccion> transacciones, PeriodoMensual periodo) =>
    transacciones
        .Where(t => periodo.Contiene(t.Fecha))
        .Aggregate(
            TotalesFinancieros.Vacios,
            (acumulado, t) => t.Tipo == TipoMovimiento.Ingreso
                ? acumulado with { Ingresos = acumulado.Ingresos + t.Monto }
                : acumulado with { Gastos = acumulado.Gastos + t.Monto });

// En CalcularGastoPorCategoria:
var totalesPorCategoria = transacciones
    .Where(t => t.Tipo == TipoMovimiento.Gasto && periodo.Contiene(t.Fecha))
    .GroupBy(t => t.CategoriaId)
    .Select(grupo => (CategoriaId: grupo.Key, Total: grupo.Sum(t => t.Monto)))
    .ToList();
```

- **Por qué este paradigma y no otro:** un reporte es, por naturaleza, una transformación:
  entra una colección de movimientos y sale un resumen. Escribirlo como funciones puras
  (sin estado, sin leer la base ni la hora, sin modificar la entrada) hace que el mismo dato dé
  siempre el mismo resultado, que el cálculo del dinero se pueda probar con datos en memoria
  (28 pruebas de reportes, incluido el redondeo de porcentajes) y que la regla no dependa de EF Core ni del
  API. La parte con efectos (leer la base, en paralelo con `Task.WhenAll`) queda separada en
  `ReporteDA` y `ReporteFlujo`.

**Para la defensa:**
- "La regla de reportes no tiene variables que vayan cambiando: cada paso de LINQ recibe una
  colección y devuelve otra nueva."
- "`Aggregate` es una reducción: parte de unos totales vacíos y por cada movimiento devuelve
  unos totales nuevos con `with`; nunca hacemos `total += monto`."
- "Como la regla es pura, la probamos sin base de datos: le pasamos una lista en memoria y
  comparamos el resultado, y también comprobamos que la lista original no cambió."
- "El Flujo trae los datos en paralelo y la regla solo calcula: los efectos quedan afuera."

**Pregunta probable:** *¿Qué hace que esa función sea "pura"?* Su resultado depende solo de
sus parámetros y no produce efectos: no consulta la base, no lee la hora (la fecha "hoy" se la
pasa el Flujo), no guarda nada y no modifica la colección que recibe. Por eso, con la misma
entrada, siempre devuelve lo mismo.

## Plantilla por paradigma

### <Paradigma>

- **Módulo:**
- **Problema que resuelve:**
- **Tecnología:**
- **Archivo y método:** `ruta/al/Archivo.cs` → `NombreMetodo`
- **Fragmento:**

```csharp
// pegar aquí las líneas clave (10 a 20 como máximo)
```

- **Por qué este paradigma y no otro:**
