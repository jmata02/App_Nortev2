import { solicitar } from './clienteHttp.js'

// Módulo Dashboard y reportes (Jurgen). Sin año y mes, el API usa el mes actual.
const consultaPeriodo = (anio, mes) => (anio && mes ? `?anio=${anio}&mes=${mes}` : '')

/** ResumenFinancieroDto: { anio, mes, totalIngresos, totalGastos, balance, variacionPorcentual, deudasActivas, metasEnCurso } */
export const obtenerResumen = (anio, mes) => solicitar(`/api/reportes/resumen${consultaPeriodo(anio, mes)}`)

/** GastoPorCategoriaDto[]: { categoriaId, categoria, total, porcentaje }, de mayor a menor. */
export const obtenerGastoPorCategoria = (anio, mes) =>
  solicitar(`/api/reportes/gasto-por-categoria${consultaPeriodo(anio, mes)}`)

/** TendenciaMensualDto[]: { anio, mes, ingresos, gastos, balance }, del más viejo al actual. */
export const obtenerTendencia = (meses = 6) => solicitar(`/api/reportes/tendencia?meses=${meses}`)
