// SessionStart: le da a Claude el contexto mínimo para empezar apegado al proyecto.
import { existsSync, readFileSync } from 'node:fs'
import { join } from 'node:path'
import { BITACORA, directorioProyecto, git, leerEntrada, responder } from './comun.mjs'

const entrada = leerEntrada()
const cwd = directorioProyecto(entrada)

const integrante = git('config user.name', cwd) || '(sin configurar)'
const rama = git('rev-parse --abbrev-ref HEAD', cwd) || '(desconocida)'
const pendientes = git('status --short', cwd)

let ultimaEntrada = '(no se encontró la bitácora)'
const rutaBitacora = join(cwd, BITACORA)
if (existsSync(rutaBitacora)) {
  const texto = readFileSync(rutaBitacora, 'utf8')
  const marca = '<!-- ENTRADAS -->'
  const inicio = texto.lastIndexOf(marca)
  const contenido = inicio >= 0 ? texto.slice(inicio + marca.length) : ''
  const primera = contenido.split(/\n(?=### )/).find((bloque) => bloque.trim().startsWith('### '))
  ultimaEntrada = primera ? primera.trim().slice(0, 1500) : '(todavía no hay entradas)'
}

const contexto = `Proyecto Norte (SC-250, Grupo 4).
Integrante según git: ${integrante}. Rama actual: ${rama}.
${pendientes ? `Hay cambios sin commit:\n${pendientes}\n` : 'No hay cambios sin commit.'}
Antes de trabajar: lee CLAUDE.md, docs/proyecto/PROPUESTA_AVANCE1.md y docs/proyecto/REQUISITOS_CURSO.md.
Todo cambio se registra en ${BITACORA} (skill registrar-bitacora) antes de terminar y antes de cada commit.
Última entrada de la bitácora:
${ultimaEntrada}`

responder({ hookSpecificOutput: { hookEventName: 'SessionStart', additionalContext: contexto } })
