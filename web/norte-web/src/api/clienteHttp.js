// Único punto de salida HTTP de la web. Todos los módulos llaman al API por aquí.
const URL_BASE = import.meta.env.VITE_API_URL ?? 'http://localhost:5147'
const CLAVE_TOKEN = 'norte.token'
const CLAVE_USUARIO = 'norte.usuario'

// sessionStorage: la sesión termina al cerrar la pestaña. Puede fallar en navegación
// privada; en ese caso la app sigue funcionando, solo que sin recordar la sesión.
function leer(clave) {
  try {
    return sessionStorage.getItem(clave)
  } catch {
    return null
  }
}

function escribir(clave, valor) {
  try {
    if (valor === null) sessionStorage.removeItem(clave)
    else sessionStorage.setItem(clave, valor)
  } catch {
    // Sin almacenamiento disponible.
  }
}

/** Guarda lo que responde el API al registrarse o iniciar sesión (SesionRespuestaDto). */
export function guardarSesion({ token, expiraUtc, usuarioId, nombre, correo }) {
  escribir(CLAVE_TOKEN, token)
  escribir(CLAVE_USUARIO, JSON.stringify({ expiraUtc, usuarioId, nombre, correo }))
}

export function obtenerUsuario() {
  try {
    return JSON.parse(leer(CLAVE_USUARIO)) ?? null
  } catch {
    return null
  }
}

export function cerrarSesion() {
  escribir(CLAVE_TOKEN, null)
  escribir(CLAVE_USUARIO, null)
}

/** Hay sesión si existe el token y todavía no venció. Una sesión vencida se borra aquí. */
export function haySesion() {
  if (!leer(CLAVE_TOKEN)) return false
  const usuario = obtenerUsuario()
  const vencida = usuario?.expiraUtc && new Date(usuario.expiraUtc) <= new Date()
  if (vencida) cerrarSesion()
  return !vencida
}

/**
 * Hace la petición al API. `conToken: false` es para registro e inicio de sesión: no mandan
 * el token y su 401 significa "credenciales incorrectas", no "sesión vencida".
 */
export async function solicitar(ruta, { metodo = 'GET', cuerpo, conToken = true } = {}) {
  const token = conToken ? leer(CLAVE_TOKEN) : null
  const respuesta = await fetch(`${URL_BASE}${ruta}`, {
    method: metodo,
    headers: {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    },
    body: cuerpo ? JSON.stringify(cuerpo) : undefined,
  })

  // Token vencido o inválido: se borra la sesión y se vuelve a iniciar sesión.
  // (Sin token, un 401 es "credenciales incorrectas" y se muestra como error normal.)
  if (respuesta.status === 401 && token) {
    cerrarSesion()
    window.location.assign('/iniciar-sesion')
    throw new Error('Su sesión venció. Inicie sesión de nuevo.')
  }

  if (!respuesta.ok) {
    const detalle = await respuesta.json().catch(() => ({}))
    throw new Error(mensajeDeError(detalle, respuesta.status))
  }

  return respuesta.status === 204 ? null : respuesta.json()
}

// El API responde { mensaje } en errores de negocio y ProblemDetails ({ errors }) cuando
// fallan las validaciones de los DTO.
function mensajeDeError(detalle, estado) {
  if (detalle.mensaje) return detalle.mensaje
  if (detalle.errors) return Object.values(detalle.errors).flat().join(' ')
  return `El servidor respondió ${estado}.`
}
