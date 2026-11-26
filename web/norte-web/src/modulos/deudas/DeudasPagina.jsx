import AvisoVistaPrevia from '../../componentes/AvisoVistaPrevia.jsx'
import { BarraProgreso, Chip } from '../../componentes/Indicadores.jsx'
import Panel from '../../componentes/Panel.jsx'
import { formatearColones } from '../../utilidades/formato.js'

// VISTA PREVIA DEL DISEÑO (responsable del módulo: Carlos).
// TODO (Carlos): cambiar DEUDAS_EJEMPLO por deudasApi.js. La forma de estos datos es solo
// una PROPUESTA de contrato: acordarla y ajustarla al DTO real. El estado (AlDia, Pendiente,
// Atrasado) y el porcentaje pagado los calcula el backend; la web solo los muestra.
const DEUDAS_EJEMPLO = [
  { id: 1, nombre: 'Tarjeta BAC Visa', montoPendiente: 184000, montoTotal: 300000, vence: '28 nov', estado: 'Atrasado', porcentajePagado: 61 },
  { id: 2, nombre: 'Préstamo personal BCR', montoPendiente: 65000, montoTotal: 162500, vence: '1 dic', estado: 'Pendiente', porcentajePagado: 40 },
  { id: 3, nombre: 'Alquiler del apartamento', montoPendiente: 0, montoTotal: 220000, vence: '5 dic', estado: 'AlDia', porcentajePagado: 100 },
  { id: 4, nombre: 'Gimnasio', montoPendiente: 0, montoTotal: 18500, vence: '10 dic', estado: 'AlDia', porcentajePagado: 100 },
]

// Cómo se ve cada estado de la propuesta: al día, pendiente o atrasado.
const PRESENTACION_ESTADO = {
  AlDia: { texto: 'Al día', estilo: 'ok' },
  Pendiente: { texto: 'Pendiente', estilo: 'pendiente' },
  Atrasado: { texto: 'Atrasado', estilo: 'alerta' },
}

export default function DeudasPagina() {
  return (
    <section>
      <div className="cabecera">
        <div>
          <h1>Deudas y pagos recurrentes</h1>
          <p>Tarjetas, préstamos y mensualidades.</p>
        </div>
        <button className="accion" type="button">
          + Agregar deuda
        </button>
      </div>
      <AvisoVistaPrevia responsable="Carlos" />

      <div className="tarjetas">
        {DEUDAS_EJEMPLO.map((deuda, posicion) => {
          const { texto, estilo } = PRESENTACION_ESTADO[deuda.estado]
          return (
            <Panel key={deuda.id} indice={posicion}>
              <div className="tarjeta__titulo">
                <b>{deuda.nombre}</b>
                <Chip estado={estilo}>{texto}</Chip>
              </div>
              <BarraProgreso porcentaje={deuda.porcentajePagado} estado={estilo} retraso={posicion * 0.1} etiqueta={`Pagado de ${deuda.nombre}`} />
              <div className="tarjeta__cifras">
                <span className="num">
                  {formatearColones(deuda.montoPendiente)} de {formatearColones(deuda.montoTotal)}
                </span>
                <span>vence {deuda.vence}</span>
              </div>
            </Panel>
          )
        })}
      </div>
    </section>
  )
}
