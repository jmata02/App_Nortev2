import { Navigate, Outlet, useLocation } from 'react-router-dom'
import { haySesion } from '../api/clienteHttp.js'

/** Deja pasar a las páginas internas solo si hay sesión; si no, manda a iniciar sesión. */
export default function RutaProtegida() {
  const ubicacion = useLocation()

  if (!haySesion()) {
    return <Navigate to="/iniciar-sesion" replace state={{ desde: ubicacion.pathname }} />
  }
  return <Outlet />
}
