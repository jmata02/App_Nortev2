import { formatearColones } from '../utilidades/formato.js'

// Token de color por categoría (solo presentación). Una categoría que no esté aquí toma el
// color de su tipo: turquesa si es ingreso, frambuesa si es gasto.
const TOKEN_CATEGORIA = {
  Alimentación: 'rosa-chicle',
  Transporte: 'pervinca',
  Servicios: 'violeta',
  Entretenimiento: 'rosa-fuerte',
}

/** Una fila de movimiento: ícono de categoría, descripción, fecha y monto con su signo. */
export default function FilaMovimiento({ descripcion, categoria, tipo, monto, fecha }) {
  const esIngreso = tipo === 'Ingreso'
  const token = TOKEN_CATEGORIA[categoria] ?? (esIngreso ? 'ingreso' : 'gasto')
  const color = `var(--color-${token})`

  return (
    <div className="mov">
      <span className="mov__icono" style={{ background: `color-mix(in srgb, ${color} 14%, transparent)`, color }} aria-hidden="true">
        {categoria.slice(0, 2).toUpperCase()}
      </span>
      <div className="mov__desc">
        <b>{descripcion}</b>
        <small>{categoria}</small>
      </div>
      <span className="mov__fecha">{fecha}</span>
      <span className={`num ${esIngreso ? 'ingreso' : 'gasto'}`}>
        {esIngreso ? '+' : '−'}
        {formatearColones(monto)}
      </span>
    </div>
  )
}
