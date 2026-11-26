import { Navigate, Route, Routes } from 'react-router-dom'
import FondoNeon from './componentes/FondoNeon.jsx'
import Layout from './componentes/Layout.jsx'
import RutaProtegida from './componentes/RutaProtegida.jsx'
import InicioSesionPagina from './modulos/autenticacion/InicioSesionPagina.jsx'
import RegistroPagina from './modulos/autenticacion/RegistroPagina.jsx'
import CategoriasPagina from './modulos/categorias/CategoriasPagina.jsx'
import DashboardPagina from './modulos/dashboard/DashboardPagina.jsx'
import DeudasPagina from './modulos/deudas/DeudasPagina.jsx'
import MetasPagina from './modulos/metas/MetasPagina.jsx'
import MovimientosPagina from './modulos/movimientos/MovimientosPagina.jsx'
import PresupuestosPagina from './modulos/presupuestos/PresupuestosPagina.jsx'

// Cada módulo es dueño de su carpeta en src/modulos. Aquí solo se agregan rutas.
// Las páginas internas van dentro de RutaProtegida: sin sesión, redirigen a /iniciar-sesion.
export default function App() {
  return (
    <>
      <FondoNeon />
      <Routes>
        <Route path="/iniciar-sesion" element={<InicioSesionPagina />} />
        <Route path="/registro" element={<RegistroPagina />} />
        <Route element={<RutaProtegida />}>
          <Route element={<Layout />}>
            <Route index element={<Navigate to="/dashboard" replace />} />
            <Route path="/dashboard" element={<DashboardPagina />} />
            <Route path="/movimientos" element={<MovimientosPagina />} />
            <Route path="/deudas" element={<DeudasPagina />} />
            <Route path="/presupuestos" element={<PresupuestosPagina />} />
            <Route path="/metas" element={<MetasPagina />} />
            <Route path="/categorias" element={<CategoriasPagina />} />
          </Route>
        </Route>
        <Route path="*" element={<Navigate to="/dashboard" replace />} />
      </Routes>
    </>
  )
}
