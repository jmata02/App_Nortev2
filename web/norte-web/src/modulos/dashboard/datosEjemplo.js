// DATOS DE EJEMPLO del Dashboard (Jurgen). Tienen la forma que tendrán las respuestas de
// /api/reportes/resumen, /gasto-por-categoria y /tendencia. Cuando esos endpoints existan,
// este archivo se reemplaza por reportesApi.js y la página no cambia.
// Los colores no vienen del API: los pone la web desde tokens.css.

export const resumenEjemplo = {
  balance: 482350,
  variacionPorcentual: 12,
  ingresos: 950000,
  gastos: 467650,
  deudasActivas: 4,
  metasEnCurso: 2,
  historialBalance: [210000, 280000, 240000, 330000, 300000, 410000, 482350],
  historialDeudasActivas: [6, 6, 5, 5, 4, 4],
  historialAhorroMetas: [120000, 190000, 260000, 330000, 420000, 545000],
}

export const gastoPorCategoriaEjemplo = [
  { categoria: 'Alimentación', total: 98000 },
  { categoria: 'Servicios', total: 71200 },
  { categoria: 'Entretenimiento', total: 33000 },
  { categoria: 'Transporte', total: 25600 },
  { categoria: 'Suscripciones', total: 9850 },
]

export const tendenciaEjemplo = [
  { mes: 'jun', ingresos: 820000, gastos: 610000 },
  { mes: 'jul', ingresos: 860000, gastos: 580000 },
  { mes: 'ago', ingresos: 800000, gastos: 640000 },
  { mes: 'set', ingresos: 910000, gastos: 520000 },
  { mes: 'oct', ingresos: 850000, gastos: 560000 },
  { mes: 'nov', ingresos: 950000, gastos: 467650 },
]

export const metasEjemplo = [
  { nombre: 'Viaje a Bocas del Toro', ahorrado: 210000, objetivo: 400000, porcentaje: 52 },
  { nombre: 'Fondo de emergencia', ahorrado: 340000, objetivo: 500000, porcentaje: 68 },
  { nombre: 'Laptop nueva', ahorrado: 95000, objetivo: 650000, porcentaje: 15 },
]

export const alertaEjemplo = { categoria: 'Entretenimiento', gastado: 33000, limite: 30000 }

export const ultimosMovimientosEjemplo = [
  { id: 1, descripcion: 'Supermercado Automercado', categoria: 'Alimentación', tipo: 'Gasto', monto: 38400, fecha: '25 nov' },
  { id: 2, descripcion: 'Pago freelance, cliente EE. UU.', categoria: 'Salario', tipo: 'Ingreso', monto: 420000, fecha: '24 nov' },
  { id: 3, descripcion: 'Netflix + Spotify', categoria: 'Entretenimiento', tipo: 'Gasto', monto: 9850, fecha: '23 nov' },
  { id: 4, descripcion: 'Gasolina', categoria: 'Transporte', tipo: 'Gasto', monto: 22000, fecha: '22 nov' },
]
