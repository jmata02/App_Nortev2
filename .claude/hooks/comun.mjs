// Utilidades compartidas por los hooks de Claude Code del proyecto Norte.
// Están en Node.js porque todo el grupo lo tiene instalado (Windows, macOS o Linux).
import { execSync } from 'node:child_process'
import { readFileSync } from 'node:fs'

export const BITACORA = 'docs/BITACORA.md'

// Cambios que obligan a registrar en la bitácora: código, configuración y documentación.
const RUTAS_QUE_SE_REGISTRAN = /^(src|tests|web|docs|\.claude|\.github)\/|^[^/]+\.(slnx|json|ya?ml|md)$/

export function leerEntrada() {
  try {
    return JSON.parse(readFileSync(0, 'utf8') || '{}')
  } catch {
    return {}
  }
}

export function directorioProyecto(entrada) {
  return process.env.CLAUDE_PROJECT_DIR || entrada.cwd || process.cwd()
}

export function git(comando, cwd) {
  try {
    // trimEnd y no trim: `git status --porcelain` empieza con un espacio significativo.
    return execSync(`git ${comando}`, { cwd, encoding: 'utf8', stdio: ['ignore', 'pipe', 'ignore'] }).trimEnd()
  } catch {
    return ''
  }
}

export function lineasNoVacias(texto) {
  return texto.split('\n').map((l) => l.trim()).filter(Boolean)
}

// Archivos de una lista de rutas que exigen entrada en la bitácora (sin contar la bitácora).
export function cambiosQueSeRegistran(rutas) {
  return rutas.filter((ruta) => ruta !== BITACORA && RUTAS_QUE_SE_REGISTRAN.test(ruta))
}

// Convierte la salida de `git status --porcelain` en rutas (incluye renombrados).
export function rutasDeStatus(porcelana) {
  // No se recortan espacios: las dos primeras columnas son el estado (" M", "??").
  return porcelana.split('\n').filter((linea) => linea.length > 3).map((linea) => linea.slice(3).split(' -> ').pop().replace(/^"|"$/g, ''))
}

export function responder(objeto) {
  process.stdout.write(JSON.stringify(objeto))
  process.exit(0)
}
