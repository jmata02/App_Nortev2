// DATOS DE EJEMPLO que quedan en el Dashboard: solo los paneles que dependen de módulos
// que todavía no tienen API. Cada uno se cambia por su API cuando exista:
//   metasEjemplo              → metasApi.js (Jessica)
//   alertaEjemplo             → flujo reactivo de alertas de Presupuestos (Mauricio)
//   ultimosMovimientosEjemplo → movimientosApi.js (Jessica)
// El resumen, el gasto por categoría y la tendencia ya vienen de /api/reportes.

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
