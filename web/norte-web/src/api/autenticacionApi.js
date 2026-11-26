import { solicitar } from './clienteHttp.js'

// Módulo Autenticación (Jurgen). Ambos devuelven SesionRespuestaDto:
// { token, expiraUtc, usuarioId, nombre, correo }. No mandan token (todavía no hay sesión).
export const registrar = ({ nombre, correo, contrasena }) =>
  solicitar('/api/autenticacion/registro', { metodo: 'POST', cuerpo: { nombre, correo, contrasena }, conToken: false })

export const iniciarSesion = ({ correo, contrasena }) =>
  solicitar('/api/autenticacion/inicio-sesion', { metodo: 'POST', cuerpo: { correo, contrasena }, conToken: false })
