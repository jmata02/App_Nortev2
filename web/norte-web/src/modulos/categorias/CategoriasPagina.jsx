import { useEffect, useState } from 'react'
import { listarCategorias } from '../../api/categoriasApi.js'
import Panel from '../../componentes/Panel.jsx'

const FILTROS = [
  { valor: '', nombre: 'Todas' },
  { valor: 'Ingreso', nombre: 'Ingresos' },
  { valor: 'Gasto', nombre: 'Gastos' },
]

/**
 * Página de referencia: estado con useState, carga con useEffect y un manejador de
 * evento (onClick del filtro). Copien esta forma en cada módulo.
 */
export default function CategoriasPagina() {
  const [tipo, setTipo] = useState('')
  const [categorias, setCategorias] = useState([])
  const [error, setError] = useState(null)

  useEffect(() => {
    listarCategorias(tipo)
      .then((datos) => {
        setCategorias(datos)
        setError(null)
      })
      .catch((e) => setError(e.message))
  }, [tipo])

  // Paradigma orientado a eventos: el clic cambia el filtro y el efecto vuelve a pedir datos.
  const manejarCambioTipo = (valor) => setTipo(valor)

  return (
    <section>
      <div className="cabecera">
        <div>
          <h1>Categorías</h1>
          <p>Catálogo que usan Movimientos y Presupuestos.</p>
        </div>
      </div>

      <div className="filtros">
        {FILTROS.map(({ valor, nombre }) => (
          <button key={nombre} className="filtro" type="button" aria-pressed={tipo === valor} onClick={() => manejarCambioTipo(valor)}>
            {nombre}
          </button>
        ))}
      </div>

      {error && (
        <p className="mensaje-error" role="alert">
          No se pudieron cargar las categorías: {error}
        </p>
      )}

      <div className="tarjetas">
        {categorias.map((categoria, posicion) => (
          <Panel key={categoria.id} indice={posicion} className="categoria">
            <div className="tarjeta__titulo">
              <b>{categoria.nombre}</b>
              <span className={`chip ${categoria.tipo === 'Ingreso' ? 'chip--ok' : 'chip--alerta'}`}>
                {categoria.tipo === 'Ingreso' ? 'Ingreso' : 'Gasto'}
              </span>
            </div>
          </Panel>
        ))}
      </div>
    </section>
  )
}
