/**
 * Piezas pequeñas de estado: Chip (pastilla de color) y BarraProgreso.
 * `estado` es uno de: 'ok' (turquesa), 'pendiente' (ámbar), 'alerta' (frambuesa), 'meta' (degradado de marca).
 */

export function Chip({ estado = 'ok', children }) {
  return <span className={`chip chip--${estado}`}>{children}</span>
}

export function BarraProgreso({ porcentaje, estado = 'ok', retraso = 0, etiqueta }) {
  // La barra visual llega hasta 100 % aunque el dato pase de ahí (por ejemplo, 110 % excedido).
  const ancho = Math.min(Math.max(porcentaje, 0), 100)
  return (
    <div
      className={`barra barra--${estado}`}
      role="progressbar"
      aria-valuenow={porcentaje}
      aria-valuemin={0}
      aria-valuemax={100}
      aria-label={etiqueta}
    >
      <i style={{ '--p': `${ancho}%`, '--d': `${retraso}s` }} />
    </div>
  )
}

export function Anillo({ porcentaje }) {
  return (
    <div className="anillo" style={{ '--p': Math.min(porcentaje, 100) }} role="img" aria-label={`${porcentaje} % de avance`}>
      <span>{porcentaje}%</span>
    </div>
  )
}
