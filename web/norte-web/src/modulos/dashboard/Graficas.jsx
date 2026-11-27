import { Bar, Line } from 'react-chartjs-2'
import { brilloTornasol, colorToken, degradado, ejesColones, PALETA_GRAFICAS, tooltipColones } from '../../componentes/graficas.js'
import { prefiereMenosMovimiento } from '../../utilidades/formato.js'

/** Barras de gasto por categoría: cada barra con un color de la paleta, en degradado y con brillo. */
export function GraficaCategorias({ datos }) {
  const colores = datos.map((_, posicion) => colorToken(PALETA_GRAFICAS[posicion % PALETA_GRAFICAS.length]))
  return (
    <Bar
      plugins={[brilloTornasol]}
      aria-label="Gasto por categoría"
      data={{
        labels: datos.map((d) => d.categoria),
        datasets: [
          {
            data: datos.map((d) => d.total),
            borderRadius: 8,
            barThickness: 28,
            borderColor: colorToken('violeta'),
            backgroundColor: (contexto) => degradado(colores[contexto.dataIndex])(contexto),
          },
        ],
      }}
      options={{
        maintainAspectRatio: false,
        animation: { delay: (contexto) => (prefiereMenosMovimiento() ? 0 : contexto.dataIndex * 120) },
        plugins: { legend: { display: false }, tooltip: tooltipColones() },
        scales: ejesColones(),
      }}
    />
  )
}

/** Líneas de ingresos contra gastos de los últimos meses, con área degradada. */
export function GraficaTendencia({ datos }) {
  const ultimo = datos.length - 1
  const ejes = ejesColones()
  const serie = (etiqueta, campo, color) => ({
    label: etiqueta,
    data: datos.map((d) => d[campo]),
    borderColor: color,
    backgroundColor: degradado(color, { desde: 'arriba' }),
    fill: true,
    tension: 0.4,
    borderWidth: 2.5,
    pointRadius: (contexto) => (contexto.dataIndex === ultimo ? 5 : 0),
    pointBackgroundColor: color,
  })

  return (
    <Line
      plugins={[brilloTornasol]}
      aria-label="Tendencia de ingresos y gastos"
      data={{
        labels: datos.map((d) => d.mes),
        datasets: [serie('Ingresos', 'ingresos', colorToken('ingreso')), serie('Gastos', 'gastos', colorToken('gasto'))],
      }}
      options={{
        maintainAspectRatio: false,
        interaction: { mode: 'index', intersect: false },
        plugins: {
          legend: { position: 'top', align: 'end', labels: { boxWidth: 10, boxHeight: 10, usePointStyle: true } },
          tooltip: tooltipColones(true),
        },
        scales: { ...ejes, y: { ...ejes.y, beginAtZero: false } },
      }}
    />
  )
}
