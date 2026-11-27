import { useEffect, useState } from 'react'
import { debounceTime } from 'rxjs'
import { obtenerUsuario } from '../../api/clienteHttp.js'
import { obtenerGastoPorCategoria, obtenerResumen, obtenerTendencia } from '../../api/reportesApi.js'
import AvisoVistaPrevia from '../../componentes/AvisoVistaPrevia.jsx'
import CifraAnimada from '../../componentes/CifraAnimada.jsx'
import FilaMovimiento from '../../componentes/FilaMovimiento.jsx'
import Icono from '../../componentes/Icono.jsx'
import { BarraProgreso, Chip } from '../../componentes/Indicadores.jsx'
import MiniGrafica from '../../componentes/MiniGrafica.jsx'
import Panel from '../../componentes/Panel.jsx'
import { cambiosFinancieros$ } from '../../reactivo/almacenFinanciero.js'
import { formatearColones, formatearEntero } from '../../utilidades/formato.js'
import { alertaEjemplo, metasEjemplo, ultimosMovimientosEjemplo } from './datosEjemplo.js'
import { GraficaCategorias, GraficaTendencia } from './Graficas.jsx'
import './Dashboard.css'

const MESES_TENDENCIA = 6
const ESPERA_ANTES_DE_RECARGAR_MS = 300
const formatoMes = new Intl.DateTimeFormat('es-CR', { month: 'long' })
const formatoMesCorto = new Intl.DateTimeFormat('es-CR', { month: 'short' })
const nombreMes = (anio, mes, formato = formatoMes) => formato.format(new Date(anio, mes - 1, 1)).replace('.', '')

function saludoSegunHora(hora) {
  if (hora < 12) return 'Buenos días'
  if (hora < 19) return 'Buenas tardes'
  return 'Buenas noches'
}

/** Pide las tres piezas del Dashboard al API al mismo tiempo. */
async function cargarReportes() {
  const [resumen, categorias, tendencia] = await Promise.all([
    obtenerResumen(),
    obtenerGastoPorCategoria(),
    obtenerTendencia(MESES_TENDENCIA),
  ])
  return { resumen, categorias, tendencia }
}

/**
 * Dashboard y reportes (Jurgen). Los números vienen de /api/reportes, donde el paradigma
 * funcional (ReglaResumenFinanciero) hace los cálculos. La web solo los presenta.
 */
export default function DashboardPagina() {
  const usuario = obtenerUsuario()
  // La fecha se toma una vez al abrir la página (no en cada render).
  const [ahora] = useState(() => new Date())
  const [reportes, setReportes] = useState(null)
  const [error, setError] = useState(null)
  // Cada vez que cambia, se vuelven a pedir los reportes.
  const [version, setVersion] = useState(0)

  useEffect(() => {
    let vigente = true
    cargarReportes()
      .then((datos) => {
        if (!vigente) return
        setReportes(datos)
        setError(null)
      })
      .catch((e) => vigente && setError(e.message))
    return () => {
      vigente = false
    }
  }, [version])

  // Suscripción al flujo reactivo (capa de Mauricio): cuando otro módulo publica un cambio
  // (por ejemplo, Movimientos registra un gasto), el Dashboard se recarga solo, sin que ese
  // módulo sepa que el Dashboard existe. debounceTime junta una ráfaga de cambios seguidos
  // en una sola recarga.
  useEffect(() => {
    const suscripcion = cambiosFinancieros$
      .pipe(debounceTime(ESPERA_ANTES_DE_RECARGAR_MS))
      .subscribe(() => setVersion((anterior) => anterior + 1))
    return () => suscripcion.unsubscribe()
  }, [])

  const primerNombre = usuario?.nombre?.split(' ')[0] ?? ''

  return (
    <section>
      <div className="cabecera">
        <div>
          <h1>
            {saludoSegunHora(ahora.getHours())}
            {primerNombre && `, ${primerNombre}`}
          </h1>
          <p>
            Así va {formatoMes.format(ahora)}, al día {ahora.getDate()}.
          </p>
        </div>
      </div>

      {error && (
        <div className="alerta">
          <span className="alerta__pulso" />
          <div>
            <p>
              <b>No se pudieron cargar los reportes.</b> {error}
            </p>
            <button className="accion" type="button" onClick={() => setVersion((anterior) => anterior + 1)}>
              Reintentar
            </button>
          </div>
        </div>
      )}

      {!reportes && !error && <p className="dashboard__cargando">Cargando tu resumen…</p>}

      {reportes && <Reportes {...reportes} />}

      <AvisoVistaPrevia texto="Metas, alerta de presupuesto y últimos movimientos: datos de ejemplo hasta que sus módulos tengan API" />

      <div className="alerta entrada" style={{ '--i': 7 }}>
        <span className="alerta__pulso" />
        <div>
          <p>
            <b>{alertaEjemplo.categoria} pasó su límite:</b> llevás {formatearColones(alertaEjemplo.gastado)} de{' '}
            {formatearColones(alertaEjemplo.limite)}.
          </p>
          <small>Las alertas de presupuesto llegarán por el flujo reactivo de Presupuestos.</small>
        </div>
      </div>

      <div className="rejilla">
        <Panel titulo="Progreso de metas" indice={8}>
          {metasEjemplo.map((meta, posicion) => (
            <div key={meta.nombre} className="fila-meta">
              <div>
                <span>{meta.nombre}</span>
                <span className="num">
                  {formatearColones(meta.ahorrado)} / {formatearColones(meta.objetivo)}
                </span>
              </div>
              <BarraProgreso porcentaje={meta.porcentaje} estado="meta" retraso={posicion * 0.15} etiqueta={meta.nombre} />
            </div>
          ))}
        </Panel>
        <Panel titulo="Últimos movimientos" indice={9}>
          <div className="lista">
            {ultimosMovimientosEjemplo.map((movimiento) => (
              <FilaMovimiento key={movimiento.id} {...movimiento} />
            ))}
          </div>
        </Panel>
      </div>
    </section>
  )
}

/** Paneles que vienen de /api/reportes: saldo, indicadores y las dos gráficas. */
function Reportes({ resumen, categorias, tendencia }) {
  const sube = (resumen.variacionPorcentual ?? 0) >= 0
  const datosTendencia = tendencia.map((t) => ({ ...t, mes: nombreMes(t.anio, t.mes, formatoMesCorto) }))

  // token = nombre del color en tokens.css que identifica a cada indicador.
  const indicadores = [
    { etiqueta: 'Ingresos', valor: resumen.totalIngresos, clase: 'ingreso', icono: 'subir', token: 'ingreso', serie: tendencia.map((t) => t.ingresos) },
    { etiqueta: 'Gastos', valor: resumen.totalGastos, clase: 'gasto', icono: 'bajar', token: 'gasto', serie: tendencia.map((t) => t.gastos) },
    { etiqueta: 'Deudas activas', valor: resumen.deudasActivas, icono: 'tarjeta', token: 'pendiente', entero: true },
    { etiqueta: 'Metas en curso', valor: resumen.metasEnCurso, icono: 'diana', token: 'violeta', entero: true },
  ]

  return (
    <>
      <div className="heroe">
        <div className="saldo entrada" style={{ '--i': 1 }}>
          <small>Balance de {nombreMes(resumen.anio, resumen.mes)} (ingresos menos gastos)</small>
          <CifraAnimada valor={resumen.balance} className="saldo__cifra" />
          {resumen.variacionPorcentual !== null && (
            <Chip estado={sube ? 'ok' : 'alerta'}>
              {sube ? '▲' : '▼'} {Math.abs(resumen.variacionPorcentual)} % respecto al mes anterior
            </Chip>
          )}
          <MiniGrafica
            serie={tendencia.map((t) => t.balance)}
            color="var(--color-sobre-tornasol)"
            alto={54}
            etiqueta={`Balance de los últimos ${tendencia.length} meses`}
          />
        </div>

        <Panel indice={2} className="indicadores">
          {indicadores.map((indicador, posicion) => {
            const color = `var(--color-${indicador.token})`
            return (
              <div key={indicador.etiqueta} className="indicador">
                <span className="indicador__icono" style={{ background: `color-mix(in srgb, ${color} 15%, transparent)`, color }}>
                  <Icono nombre={indicador.icono} />
                </span>
                <small>{indicador.etiqueta}</small>
                <CifraAnimada
                  valor={indicador.valor}
                  formato={indicador.entero ? formatearEntero : formatearColones}
                  className={`indicador__valor ${indicador.clase ?? ''}`}
                />
                {indicador.serie && <MiniGrafica serie={indicador.serie} color={color} retraso={0.2 + posicion * 0.08} />}
              </div>
            )
          })}
        </Panel>
      </div>

      <div className="rejilla">
        <Panel titulo="Gasto por categoría" detalle={nombreMes(resumen.anio, resumen.mes)} indice={3}>
          {categorias.length > 0 ? (
            <div className="lienzo">
              <GraficaCategorias datos={categorias} />
            </div>
          ) : (
            <p className="dashboard__vacio">Todavía no hay gastos este mes.</p>
          )}
        </Panel>
        <Panel titulo="Ingresos vs gastos" detalle={`últimos ${tendencia.length} meses`} indice={4}>
          <div className="lienzo">
            <GraficaTendencia datos={datosTendencia} />
          </div>
        </Panel>
      </div>
    </>
  )
}
