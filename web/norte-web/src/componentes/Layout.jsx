import { NavLink, Outlet, useLocation, useNavigate } from 'react-router-dom'
import { cerrarSesion, obtenerUsuario } from '../api/clienteHttp.js'
import { iniciales } from '../utilidades/formato.js'
import Icono from './Icono.jsx'
import Logo from './Logo.jsx'
import './Layout.css'

const SECCIONES = [
  { ruta: '/dashboard', nombre: 'Dashboard', icono: 'dashboard' },
  { ruta: '/movimientos', nombre: 'Movimientos', icono: 'movimientos' },
  { ruta: '/deudas', nombre: 'Deudas y pagos', icono: 'deudas' },
  { ruta: '/presupuestos', nombre: 'Presupuestos', icono: 'presupuestos' },
  { ruta: '/metas', nombre: 'Metas de ahorro', icono: 'metas' },
  { ruta: '/categorias', nombre: 'Categorías', icono: 'categorias' },
]

export default function Layout() {
  const navegar = useNavigate()
  const ubicacion = useLocation()
  const usuario = obtenerUsuario()

  // Paradigma orientado a eventos: el clic en "Salir" borra la sesión y vuelve al acceso.
  const manejarCerrarSesion = () => {
    cerrarSesion()
    navegar('/iniciar-sesion', { replace: true })
  }

  return (
    <div className="app">
      <aside className="menu">
        <Logo />
        <nav className="menu__navegacion" aria-label="Secciones">
          {SECCIONES.map(({ ruta, nombre, icono }) => (
            <NavLink key={ruta} to={ruta} className="enlace">
              <Icono nombre={icono} />
              <span>{nombre}</span>
            </NavLink>
          ))}
        </nav>
        <div className="menu__pie">
          <span className="avatar" aria-hidden="true">
            {iniciales(usuario?.nombre)}
          </span>
          <div className="menu__usuario">
            {usuario?.nombre}
            <small>{usuario?.correo}</small>
          </div>
          <button className="salir" type="button" onClick={manejarCerrarSesion}>
            Salir
          </button>
        </div>
      </aside>
      {/* La key hace que la animación de entrada se repita en cada página. */}
      <main className="contenido" key={ubicacion.pathname}>
        <Outlet />
      </main>
    </div>
  )
}
