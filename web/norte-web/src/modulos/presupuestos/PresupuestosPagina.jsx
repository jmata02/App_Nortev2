import AvisoVistaPrevia from '../../componentes/AvisoVistaPrevia.jsx'
import { BarraProgreso, Chip } from '../../componentes/Indicadores.jsx'
import Panel from '../../componentes/Panel.jsx'
import { formatearColones } from '../../utilidades/formato.js'

// VISTA PREVIA DEL DISEÑO (responsable del módulo: Mauricio).
// TODO (Mauricio): cambiar PRESUPUESTOS_EJEMPLO por presupuestosApi.js y suscribir la página
// al flujo reactivo (almacenFinanciero.js) para que las barras cambien solas. La forma de
// estos datos es solo una PROPUESTA de contrato (incluido el estado "CercaDelLimite", que la
// entidad Presupuesto todavía no tiene): acordarla y ajustarla al DTO real. El porcentaje y el
// estado los debe calcular el backend.
const PRESUPUESTOS_EJEMPLO = [
  { id: 1, categoria: 'Alimentación', gastado: 98000, limite: 150000, porcentajeUsado: 65, estado: 'Normal' },
  { id: 2, categoria: 'Transporte', gastado: 25600, limite: 50000, porcentajeUsado: 51, estado: 'Normal' },
  { id: 3, categoria: 'Entretenimiento', gastado: 33000, limite: 30000, porcentajeUsado: 110, estado: 'Excedido' },
  { id: 4, categoria: 'Servicios', gastado: 71200, limite: 80000, porcentajeUsado: 89, estado: 'CercaDelLimite' },
]

const ESTILO_ESTADO = { Normal: 'ok', CercaDelLimite: 'pendiente', Excedido: 'alerta' }

export default function PresupuestosPagina() {
  return (
    <section>
      <div className="cabecera">
        <div>
          <h1>Presupuestos</h1>
          <p>Lo planeado contra lo gastado este mes.</p>
        </div>
        <button className="accion" type="button">
          + Definir presupuesto
        </button>
      </div>
      <AvisoVistaPrevia responsable="Mauricio" />

      <div className="tarjetas">
        {PRESUPUESTOS_EJEMPLO.map((presupuesto, posicion) => {
          const estilo = ESTILO_ESTADO[presupuesto.estado]
          return (
            <Panel key={presupuesto.id} indice={posicion}>
              <div className="tarjeta__titulo">
                <b>{presupuesto.categoria}</b>
                <Chip estado={estilo}>{presupuesto.porcentajeUsado} %</Chip>
              </div>
              <BarraProgreso porcentaje={presupuesto.porcentajeUsado} estado={estilo} retraso={posicion * 0.1} etiqueta={`Uso de ${presupuesto.categoria}`} />
              <div className="tarjeta__cifras">
                <span className="num">{formatearColones(presupuesto.gastado)} gastado</span>
                <span className="num">límite {formatearColones(presupuesto.limite)}</span>
              </div>
            </Panel>
          )
        })}
      </div>
    </section>
  )
}
