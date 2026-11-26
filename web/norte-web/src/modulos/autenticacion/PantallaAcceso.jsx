import { useState } from 'react'
import { Link, Navigate, useLocation, useNavigate } from 'react-router-dom'
import { iniciarSesion, registrar } from '../../api/autenticacionApi.js'
import { guardarSesion, haySesion } from '../../api/clienteHttp.js'
import Logo from '../../componentes/Logo.jsx'
import './Acceso.css'

const FORMATO_CORREO = /^[^\s@]+@[^\s@]+\.[^\s@]+$/

/**
 * Validación previa en la web, igual a la del backend, para avisar sin ir al servidor.
 * La regla que manda es la del API (ReglaContrasenaSegura).
 */
function validar(datos, esRegistro) {
  if (esRegistro && datos.nombre.trim().length < 2) return { campo: 'nombre', mensaje: 'Escriba su nombre.' }
  if (!FORMATO_CORREO.test(datos.correo.trim())) return { campo: 'correo', mensaje: 'El correo no tiene un formato válido.' }
  if (!datos.contrasena) return { campo: 'contrasena', mensaje: 'Escriba su contraseña.' }
  if (esRegistro && (datos.contrasena.length < 8 || !/\d/.test(datos.contrasena) || !/\p{L}/u.test(datos.contrasena)))
    return { campo: 'contrasena', mensaje: 'La contraseña necesita 8 o más caracteres, una letra y un número.' }
  return null
}

/**
 * Pantalla de inicio de sesión y registro (módulo Autenticación).
 * modo: 'inicio' | 'registro'. Las dos rutas comparten esta pantalla y cambian con las pestañas.
 */
export default function PantallaAcceso({ modo }) {
  const esRegistro = modo === 'registro'
  const navegar = useNavigate()
  const ubicacion = useLocation()
  const [datos, setDatos] = useState({ nombre: '', correo: '', contrasena: '' })
  const [error, setError] = useState(null)
  const [enviando, setEnviando] = useState(false)

  if (haySesion()) return <Navigate to="/dashboard" replace />

  // Paradigma orientado a eventos: cada tecla actualiza el campo y limpia el error.
  const manejarCambio = (evento) => {
    const { name, value } = evento.target
    setDatos((anteriores) => ({ ...anteriores, [name]: value }))
    setError(null)
  }

  // Paradigma orientado a eventos: el envío valida, llama al API y guarda la sesión.
  const manejarEnvio = async (evento) => {
    evento.preventDefault()
    const problema = validar(datos, esRegistro)
    if (problema) {
      setError(problema)
      return
    }

    setEnviando(true)
    try {
      const sesion = esRegistro ? await registrar(datos) : await iniciarSesion(datos)
      guardarSesion(sesion)
      navegar(ubicacion.state?.desde ?? '/dashboard', { replace: true })
    } catch (e) {
      setError({ campo: null, mensaje: e.message })
      setEnviando(false)
    }
  }

  return (
    <div className="acceso">
      <div className="escena">
        <Logo />
        <div className="sol" aria-hidden="true" />
        <div className="horizonte" aria-hidden="true" />
        <div className="lema">
          <h1>
            Tu plata, <span>bajo control</span> de noche y de día.
          </h1>
          <p>Registrá gastos, controlá deudas y llegá a tus metas de ahorro.</p>
        </div>
      </div>

      <div className="panel-acceso">
        <form className="tarjeta-acceso" onSubmit={manejarEnvio} noValidate>
          <div className="pestanas">
            <Link to="/iniciar-sesion" replace aria-current={esRegistro ? undefined : 'page'}>
              Iniciar sesión
            </Link>
            <Link to="/registro" replace aria-current={esRegistro ? 'page' : undefined}>
              Crear cuenta
            </Link>
          </div>

          {esRegistro && (
            <div className="campo">
              <label htmlFor="nombre">Nombre</label>
              <input
                id="nombre"
                name="nombre"
                autoComplete="name"
                value={datos.nombre}
                onChange={manejarCambio}
                aria-invalid={error?.campo === 'nombre'}
              />
            </div>
          )}

          <div className="campo">
            <label htmlFor="correo">Correo</label>
            <input
              id="correo"
              name="correo"
              type="email"
              autoComplete="email"
              value={datos.correo}
              onChange={manejarCambio}
              aria-invalid={error?.campo === 'correo'}
            />
          </div>

          <div className="campo">
            <label htmlFor="contrasena">Contraseña</label>
            <input
              id="contrasena"
              name="contrasena"
              type="password"
              autoComplete={esRegistro ? 'new-password' : 'current-password'}
              value={datos.contrasena}
              onChange={manejarCambio}
              aria-invalid={error?.campo === 'contrasena'}
            />
            {esRegistro && <span className="campo__ayuda">Mínimo 8 caracteres, con una letra y un número.</span>}
          </div>

          <p className="mensaje-error" role="alert">
            {error?.mensaje}
          </p>

          <button className="boton" type="submit" disabled={enviando}>
            {enviando ? 'Un momento…' : esRegistro ? 'Crear cuenta' : 'Entrar'}
          </button>
        </form>
      </div>
    </div>
  )
}
