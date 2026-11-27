import {
  BarElement,
  CategoryScale,
  Chart,
  Filler,
  Legend,
  LinearScale,
  LineElement,
  PointElement,
  Tooltip,
} from 'chart.js'
import { formatearColones, formatearColonesCorto, prefiereMenosMovimiento } from '../utilidades/formato.js'

/**
 * Lee un color de tokens.css. Chart.js dibuja en un canvas y no entiende var(--...),
 * así que se le pasa el valor real del token (por ejemplo "#4b3ce0"). Solo sirve para tokens
 * que son un color hexadecimal, no para los que usan color-mix().
 */
export const colorToken = (nombre) =>
  getComputedStyle(document.documentElement).getPropertyValue(`--color-${nombre}`).trim()

/** Orden de colores para series y categorías de las gráficas (nombres de token, tornasol). */
export const PALETA_GRAFICAS = ['rosa-chicle', 'violeta', 'pervinca', 'rosa-fuerte', 'lavanda']

// Configuración común de Chart.js para el tema tornasol. Se importa una vez desde main.jsx,
// después de tokens.css.
Chart.register(CategoryScale, LinearScale, BarElement, LineElement, PointElement, Filler, Tooltip, Legend)
Chart.defaults.color = colorToken('texto-suave')
Chart.defaults.font.family = getComputedStyle(document.documentElement).getPropertyValue('--fuente-texto').trim()
Chart.defaults.animation.duration = prefiereMenosMovimiento() ? 0 : 1100
Chart.defaults.plugins.tooltip.backgroundColor = colorToken('superficie')
Chart.defaults.plugins.tooltip.titleColor = colorToken('tinta')
Chart.defaults.plugins.tooltip.bodyColor = colorToken('texto-suave')
Chart.defaults.plugins.tooltip.borderColor = colorToken('violeta')
Chart.defaults.plugins.tooltip.borderWidth = 1
Chart.defaults.plugins.tooltip.padding = 10

/** Brillo tornasol: dibuja cada serie con una sombra suave de su propio color. */
export const brilloTornasol = {
  id: 'brilloTornasol',
  beforeDatasetDraw(grafica, { meta }) {
    const contexto = grafica.ctx
    const serie = grafica.data.datasets[meta.index]
    const color = typeof serie.borderColor === 'string' ? serie.borderColor : colorToken('violeta')
    contexto.save()
    // Sobre fondo blanco el brillo es más suave (66 = 40 % de opacidad en hexadecimal).
    contexto.shadowColor = `${color}66`
    contexto.shadowBlur = meta.type === 'line' ? 16 : 12
    contexto.shadowOffsetY = meta.type === 'line' ? 6 : 0
  },
  afterDatasetDraw(grafica) {
    grafica.ctx.restore()
  },
}

/** Degradado vertical de un color (hex): opaco en un extremo y transparente en el otro. */
export const degradado = (color, { desde = 'abajo', opacidad = '33' } = {}) => (contexto) => {
  const { chartArea: area, ctx } = contexto.chart
  if (!area) return color
  const gradiente =
    desde === 'abajo'
      ? ctx.createLinearGradient(0, area.bottom, 0, area.top)
      : ctx.createLinearGradient(0, area.top, 0, area.bottom)
  gradiente.addColorStop(0, desde === 'abajo' ? color + opacidad : color + '55')
  gradiente.addColorStop(1, desde === 'abajo' ? color : color + '00')
  return gradiente
}

export const ejesColones = () => ({
  y: {
    beginAtZero: true,
    grid: { color: `${colorToken('tinta')}14` }, // tinta al 8 % (14 en hexadecimal)
    border: { display: false },
    ticks: {
      font: { family: getComputedStyle(document.documentElement).getPropertyValue('--fuente-numeros').trim(), size: 10 },
      callback: formatearColonesCorto,
    },
  },
  x: { grid: { display: false }, border: { display: false } },
})

export const tooltipColones = (conEtiqueta = false) => ({
  callbacks: {
    label: (item) => (conEtiqueta ? `${item.dataset.label}: ` : '') + formatearColones(item.raw),
  },
})
