import { prefiereMenosMovimiento } from '../utilidades/formato.js'

const puedeInclinar =
  typeof window !== 'undefined' &&
  window.matchMedia('(hover: hover) and (pointer: fine)').matches &&
  !prefiereMenosMovimiento()

/**
 * Tarjeta de vidrio que todos los módulos usan para agrupar contenido.
 * Al mover el mouse encima se inclina un poco en 3D y una luz sigue al cursor: los
 * manejadores solo actualizan variables CSS (--rx, --ry, --mx, --my) que usa global.css.
 *
 * Props: titulo y detalle (encabezado opcional), indice (orden en la entrada escalonada).
 */
export default function Panel({ titulo, detalle, indice = 0, className = '', children, ...resto }) {
  const manejarMovimiento = (evento) => {
    if (!puedeInclinar) return
    const panel = evento.currentTarget
    const caja = panel.getBoundingClientRect()
    const x = (evento.clientX - caja.left) / caja.width
    const y = (evento.clientY - caja.top) / caja.height
    panel.style.setProperty('--mx', `${x * 100}%`)
    panel.style.setProperty('--my', `${y * 100}%`)
    panel.style.setProperty('--rx', `${((0.5 - y) * 5).toFixed(2)}deg`)
    panel.style.setProperty('--ry', `${((x - 0.5) * 6).toFixed(2)}deg`)
  }

  const manejarSalida = (evento) => {
    evento.currentTarget.style.setProperty('--rx', '0deg')
    evento.currentTarget.style.setProperty('--ry', '0deg')
  }

  return (
    <section
      className={`panel entrada ${className}`}
      style={{ '--i': indice }}
      onPointerMove={manejarMovimiento}
      onPointerLeave={manejarSalida}
      {...resto}
    >
      {titulo && (
        <h2 className="panel__titulo">
          {titulo}
          {detalle && <em>{detalle}</em>}
        </h2>
      )}
      {children}
    </section>
  )
}
