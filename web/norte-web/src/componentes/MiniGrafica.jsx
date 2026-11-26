import { useId } from 'react'

/**
 * Mini gráfica de tendencia (sparkline) en SVG: relleno degradado y una línea que aparece
 * de izquierda a derecha.
 * `color` es un color CSS, normalmente un token: 'var(--color-cian)'.
 */
export default function MiniGrafica({ serie, color, alto = 34, retraso = 0.2, etiqueta }) {
  const idDegradado = useId()
  const minimo = Math.min(...serie)
  const rango = Math.max(...serie) - minimo || 1

  // Cada valor se convierte en un punto dentro de una caja de 100 x 34.
  const linea = serie
    .map((valor, indice) => {
      const x = (indice / (serie.length - 1)) * 100
      const y = 30 - ((valor - minimo) / rango) * 26
      return `${indice ? 'L' : 'M'}${x.toFixed(1)} ${y.toFixed(1)}`
    })
    .join(' ')

  return (
    <svg
      className="chispa"
      style={{ height: alto, '--d': `${retraso}s` }}
      viewBox="0 0 100 34"
      preserveAspectRatio="none"
      role={etiqueta ? 'img' : undefined}
      aria-label={etiqueta}
      aria-hidden={etiqueta ? undefined : true}
    >
      <defs>
        <linearGradient id={idDegradado} x1="0" y1="0" x2="0" y2="1">
          <stop offset="0" style={{ stopColor: color, stopOpacity: 0.45 }} />
          <stop offset="1" style={{ stopColor: color, stopOpacity: 0 }} />
        </linearGradient>
      </defs>
      <path d={`${linea} L100 34 L0 34 Z`} fill={`url(#${idDegradado})`} />
      <path
        d={linea}
        fill="none"
        strokeWidth="2"
        strokeLinecap="round"
        strokeLinejoin="round"
        vectorEffect="non-scaling-stroke"
        style={{ stroke: color, filter: `drop-shadow(0 0 4px ${color})` }}
      />
    </svg>
  )
}
