// PreToolUse (Bash): protege las reglas del grupo antes de ejecutar comandos.
//  - Migraciones: solo Jurgen las genera (riesgo 5) → se pide confirmación.
//  - Nada de push a main/dev ni push --force → se bloquea.
//  - Commit con código sin la bitácora incluida → se bloquea.
import {
  BITACORA,
  cambiosQueSeRegistran,
  directorioProyecto,
  git,
  leerEntrada,
  lineasNoVacias,
  responder,
} from './comun.mjs'

const entrada = leerEntrada()
const comando = entrada?.tool_input?.command ?? ''
const cwd = directorioProyecto(entrada)

const decidir = (decision, motivo) =>
  responder({
    hookSpecificOutput: {
      hookEventName: 'PreToolUse',
      permissionDecision: decision,
      permissionDecisionReason: motivo,
    },
  })

if (/dotnet\s+ef\s+migrations\s+(add|remove)/.test(comando)) {
  decidir(
    'ask',
    'Regla del grupo (riesgo 5): solo Jurgen genera o quita migraciones para no romper el ModelSnapshot. ' +
      'Si usted no es Jurgen, cancele y anote "requiere migración" en la bitácora y en el PR.',
  )
}

if (/git\s+push\b/.test(comando)) {
  if (/(--force\b|\s-f\b|--force-with-lease)/.test(comando)) {
    decidir('deny', 'No se permite push --force en este repositorio.')
  }
  if (/git\s+push\b.*\s(main|dev)(\s|$)/.test(comando) || /:(main|dev)(\s|$)/.test(comando)) {
    decidir('deny', 'No se hace push directo a main ni a dev: suba la rama del módulo y abra un Pull Request hacia dev.')
  }
}

if (/git\s+commit\b/.test(comando)) {
  const incluyeTodo = /\s(-a|--all|-[a-zA-Z]*a[a-zA-Z]*)\b/.test(comando) && !/--amend/.test(comando)
  const preparados = lineasNoVacias(git('diff --cached --name-only', cwd))
  const rutas = incluyeTodo ? [...preparados, ...lineasNoVacias(git('diff --name-only', cwd))] : preparados
  if (cambiosQueSeRegistran(rutas).length > 0 && !rutas.includes(BITACORA)) {
    decidir(
      'deny',
      `El commit incluye cambios pero no ${BITACORA}. Registre la sesión con la skill registrar-bitacora, ` +
        `agregue la bitácora al commit (git add ${BITACORA}) y vuelva a intentarlo.`,
    )
  }
}

process.exit(0)
