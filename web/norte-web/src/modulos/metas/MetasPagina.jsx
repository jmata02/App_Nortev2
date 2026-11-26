import AvisoVistaPrevia from '../../componentes/AvisoVistaPrevia.jsx'
import { Anillo } from '../../componentes/Indicadores.jsx'
import Panel from '../../componentes/Panel.jsx'
import { formatearColones } from '../../utilidades/formato.js'

// VISTA PREVIA DEL DISEÑO (responsable del módulo: Jessica).
// TODO (Jessica): cambiar METAS_EJEMPLO por metasApi.js y agregar el formulario de aporte.
// La forma de estos datos es solo una PROPUESTA de contrato: acordarla y ajustarla al DTO
// real. El porcentaje lo calcula el backend (MetaAhorro.PorcentajeAvance).
const METAS_EJEMPLO = [
  { id: 1, nombre: 'Viaje a Bocas del Toro', fechaLimite: 'diciembre 2026', ahorrado: 210000, objetivo: 400000, porcentajeAvance: 52 },
  { id: 2, nombre: 'Fondo de emergencia', fechaLimite: 'sin fecha fija', ahorrado: 340000, objetivo: 500000, porcentajeAvance: 68 },
  { id: 3, nombre: 'Laptop nueva', fechaLimite: 'marzo 2027', ahorrado: 95000, objetivo: 650000, porcentajeAvance: 15 },
]

export default function MetasPagina() {
  return (
    <section>
      <div className="cabecera">
        <div>
          <h1>Metas de ahorro</h1>
          <p>Objetivos activos y su avance.</p>
        </div>
        <button className="accion" type="button">
          + Nueva meta
        </button>
      </div>
      <AvisoVistaPrevia responsable="Jessica" />

      <div className="tarjetas">
        {METAS_EJEMPLO.map((meta, posicion) => (
          <Panel key={meta.id} indice={posicion}>
            <div className="tarjeta__titulo">
              <div>
                <div className="meta__nombre">{meta.nombre}</div>
                <small className="meta__fecha">Fecha límite: {meta.fechaLimite}</small>
              </div>
              <Anillo porcentaje={meta.porcentajeAvance} />
            </div>
            <div className="tarjeta__cifras">
              <span className="num">{formatearColones(meta.ahorrado)} ahorrado</span>
              <span className="num">meta {formatearColones(meta.objetivo)}</span>
            </div>
          </Panel>
        ))}
      </div>
    </section>
  )
}
