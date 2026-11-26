import { useState } from 'react'
import AvisoVistaPrevia from '../../componentes/AvisoVistaPrevia.jsx'
import FilaMovimiento from '../../componentes/FilaMovimiento.jsx'
import Panel from '../../componentes/Panel.jsx'

// VISTA PREVIA DEL DISEÑO (responsable del módulo: Jessica).
// TODO (Jessica): cambiar MOVIMIENTOS_EJEMPLO por movimientosApi.js, agregar el formulario
// con onSubmit/onChange y llamar a publicarCambio(...) después de guardar. El filtro de abajo
// es provisional (filtra en la web): cambiarlo por el filtro del API por fecha, categoría y
// tipo, con su propio manejador. La forma de los datos es una PROPUESTA de contrato.
const MOVIMIENTOS_EJEMPLO = [
  { id: 1, descripcion: 'Supermercado Automercado', categoria: 'Alimentación', tipo: 'Gasto', monto: 38400, fecha: '25 nov' },
  { id: 2, descripcion: 'Pago freelance, cliente EE. UU.', categoria: 'Salario', tipo: 'Ingreso', monto: 420000, fecha: '24 nov' },
  { id: 3, descripcion: 'Netflix + Spotify', categoria: 'Entretenimiento', tipo: 'Gasto', monto: 9850, fecha: '23 nov' },
  { id: 4, descripcion: 'Gasolina', categoria: 'Transporte', tipo: 'Gasto', monto: 22000, fecha: '22 nov' },
  { id: 5, descripcion: 'Soda El Trébol, almuerzo', categoria: 'Alimentación', tipo: 'Gasto', monto: 5200, fecha: '21 nov' },
  { id: 6, descripcion: 'Pago CCSS independiente', categoria: 'Servicios', tipo: 'Gasto', monto: 68000, fecha: '20 nov' },
  { id: 7, descripcion: 'Venta de equipo usado', categoria: 'Otros ingresos', tipo: 'Ingreso', monto: 45000, fecha: '18 nov' },
  { id: 8, descripcion: 'Uber a la U', categoria: 'Transporte', tipo: 'Gasto', monto: 3600, fecha: '17 nov' },
]

const FILTROS = [
  { valor: '', nombre: 'Todos' },
  { valor: 'Ingreso', nombre: 'Ingresos' },
  { valor: 'Gasto', nombre: 'Gastos' },
]

export default function MovimientosPagina() {
  const [tipo, setTipo] = useState('')
  const visibles = MOVIMIENTOS_EJEMPLO.filter((m) => !tipo || m.tipo === tipo)

  return (
    <section>
      <div className="cabecera">
        <div>
          <h1>Movimientos</h1>
          <p>Todos tus ingresos y gastos.</p>
        </div>
        <button className="accion" type="button">
          + Agregar movimiento
        </button>
      </div>
      <AvisoVistaPrevia responsable="Jessica" />

      <div className="filtros">
        {FILTROS.map(({ valor, nombre }) => (
          <button key={nombre} className="filtro" type="button" aria-pressed={tipo === valor} onClick={() => setTipo(valor)}>
            {nombre}
          </button>
        ))}
      </div>

      <Panel>
        <div className="lista">
          {visibles.map((movimiento) => (
            <FilaMovimiento key={movimiento.id} {...movimiento} />
          ))}
        </div>
      </Panel>
    </section>
  )
}
