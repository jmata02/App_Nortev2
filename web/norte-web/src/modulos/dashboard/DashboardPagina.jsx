import { useState } from 'react'
import AvisoVistaPrevia from '../../componentes/AvisoVistaPrevia.jsx'
import CifraAnimada from '../../componentes/CifraAnimada.jsx'
import FilaMovimiento from '../../componentes/FilaMovimiento.jsx'
import Icono from '../../componentes/Icono.jsx'
import { BarraProgreso, Chip } from '../../componentes/Indicadores.jsx'
import MiniGrafica from '../../componentes/MiniGrafica.jsx'
import Panel from '../../componentes/Panel.jsx'
import { obtenerUsuario } from '../../api/clienteHttp.js'
import { formatearColones, formatearEntero } from '../../utilidades/formato.js'
import {
  alertaEjemplo,
  gastoPorCategoriaEjemplo,
  metasEjemplo,
  resumenEjemplo,
  tendenciaEjemplo,
  ultimosMovimientosEjemplo,
} from './datosEjemplo.js'
import { GraficaCategorias, GraficaTendencia } from './Graficas.jsx'
import './Dashboard.css'

function saludoSegunHora(hora) {
  if (hora < 12) return 'Buenos días'
  if (hora < 19) return 'Buenas tardes'
  return 'Buenas noches'
}

const formatoMes = new Intl.DateTimeFormat('es-CR', { month: 'long' })

/**
 * Dashboard y reportes (Jurgen). Por ahora usa datos de ejemplo con la forma de las
 * respuestas de /api/reportes; el paradigma funcional vivirá en Norte.Reglas (backend).
 */
export default function DashboardPagina() {
  const usuario = obtenerUsuario()
  // La fecha se toma una vez al abrir la página (no en cada render).
  const [ahora] = useState(() => new Date())
  const primerNombre = usuario?.nombre?.split(' ')[0] ?? ''
  const resumen = resumenEjemplo

  // token = nombre del color en tokens.css que identifica a cada indicador.
  const indicadores = [
    { etiqueta: 'Ingresos', valor: resumen.ingresos, clase: 'ingreso', icono: 'subir', token: 'ingreso', serie: tendenciaEjemplo.map((m) => m.ingresos) },
    { etiqueta: 'Gastos', valor: resumen.gastos, clase: 'gasto', icono: 'bajar', token: 'gasto', serie: tendenciaEjemplo.map((m) => m.gastos) },
    { etiqueta: 'Deudas activas', valor: resumen.deudasActivas, icono: 'tarjeta', token: 'pendiente', serie: resumen.historialDeudasActivas, entero: true },
    { etiqueta: 'Metas en curso', valor: resumen.metasEnCurso, icono: 'diana', token: 'violeta', serie: resumen.historialAhorroMetas, entero: true },
  ]

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

      <AvisoVistaPrevia texto="Datos de ejemplo · se conectarán a /api/reportes" />

      <div className="alerta entrada">
        <span className="alerta__pulso" />
        <div>
          <p>
            <b>{alertaEjemplo.categoria} pasó su límite:</b> llevás {formatearColones(alertaEjemplo.gastado)} de{' '}
            {formatearColones(alertaEjemplo.limite)}.
          </p>
          <small>Las alertas de presupuesto llegarán por el flujo reactivo de Presupuestos.</small>
        </div>
      </div>

      <div className="heroe">
        <div className="saldo entrada" style={{ '--i': 1 }}>
          <small>Balance disponible este mes</small>
          <CifraAnimada valor={resumen.balance} className="saldo__cifra" />
          <Chip estado="ok">▲ {resumen.variacionPorcentual} % respecto al mes anterior</Chip>
          <MiniGrafica serie={resumen.historialBalance} color="var(--color-ambar)" alto={54} etiqueta="Balance de los últimos 7 meses" />
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
              <MiniGrafica serie={indicador.serie} color={color} retraso={0.2 + posicion * 0.08} />
            </div>
            )
          })}
        </Panel>
      </div>

      <div className="rejilla">
        <Panel titulo="Gasto por categoría" detalle={formatoMes.format(ahora)} indice={3}>
          <div className="lienzo">
            <GraficaCategorias datos={gastoPorCategoriaEjemplo} />
          </div>
        </Panel>
        <Panel titulo="Progreso de metas" indice={4}>
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
      </div>

      <div className="rejilla">
        <Panel titulo="Ingresos vs gastos" detalle="últimos 6 meses" indice={5}>
          <div className="lienzo">
            <GraficaTendencia datos={tendenciaEjemplo} />
          </div>
        </Panel>
        <Panel titulo="Últimos movimientos" indice={6}>
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
