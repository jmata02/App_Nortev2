// Stop: no deja terminar la sesión si hay cambios sin registrar en la bitácora.
import {
  BITACORA,
  cambiosQueSeRegistran,
  directorioProyecto,
  git,
  leerEntrada,
  responder,
  rutasDeStatus,
} from './comun.mjs'

const entrada = leerEntrada()

// Evita un ciclo infinito si Claude ya está respondiendo a este mismo aviso.
if (entrada.stop_hook_active) process.exit(0)

const cwd = directorioProyecto(entrada)
const rutas = rutasDeStatus(git('status --porcelain', cwd))
const pendientes = cambiosQueSeRegistran(rutas)

if (pendientes.length > 0 && !rutas.includes(BITACORA)) {
  responder({
    decision: 'block',
    reason:
      `Hay cambios sin registrar en ${BITACORA}: ${pendientes.slice(0, 8).join(', ')}` +
      `${pendientes.length > 8 ? '…' : ''}. Usa la skill registrar-bitacora para agregar la entrada ` +
      'de esta sesión antes de terminar.',
  })
}

process.exit(0)
