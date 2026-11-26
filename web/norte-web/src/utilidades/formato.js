// Formato para mostrar datos. No calcula dinero: solo da forma a lo que manda el API.

const formatoColones = new Intl.NumberFormat('es-CR', {
  style: 'currency',
  currency: 'CRC',
  maximumFractionDigits: 0,
})

export const formatearColones = (monto) => formatoColones.format(monto)

/** "₡98k": para los ejes de las gráficas. */
export const formatearColonesCorto = (monto) => `₡${Math.round(monto / 1000)}k`

export const formatearEntero = (valor) => Math.round(valor).toLocaleString('es-CR')

export const iniciales = (nombre = '') =>
  nombre
    .trim()
    .split(/\s+/)
    .slice(0, 2)
    .map((parte) => parte[0]?.toUpperCase() ?? '')
    .join('')

export const prefiereMenosMovimiento = () =>
  typeof window !== 'undefined' && window.matchMedia('(prefers-reduced-motion: reduce)').matches
