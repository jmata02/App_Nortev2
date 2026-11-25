---
name: evidencia-paradigma
description: Documenta la evidencia de un paradigma (orientado a objetos, funcional, concurrente, orientado a eventos o reactivo) en docs/EVIDENCIA_PARADIGMAS.md con los cinco puntos que exige el enunciado, a partir del código real. Úsala cuando un paradigma quede implementado o antes de una entrega.
argument-hint: "<paradigma>"
---

# Documentar la evidencia de un paradigma

Paradigma: `$ARGUMENTS`.

1. Busca en el código dónde está implementado (ver la tabla de paradigmas de `CLAUDE.md`).
   Si no encuentras evidencia **real y visible**, dilo claramente y propón qué falta; no la
   inventes (el enunciado no acepta paradigmas sin evidencia).
2. En `docs/EVIDENCIA_PARADIGMAS.md`, completa la fila de la tabla y la sección del paradigma
   con los cinco puntos del enunciado (sección 6.1 de `docs/proyecto/REQUISITOS_CURSO.md`):
   - **Módulo** donde se aplica.
   - **Problema** que resuelve dentro del sistema.
   - **Tecnología** (lenguaje, framework o característica: LINQ, `async`/`await`, RxJS...).
   - **Archivo y método**, y un **fragmento** de 10 a 20 líneas copiado tal cual del código.
   - **Por qué este paradigma** fue adecuado ahí y no otro.
3. Agrega **"Para la defensa"**: tres o cuatro frases sencillas que el responsable pueda decir
   en voz alta, y una pregunta probable del profesor con su respuesta.
4. Verifica que el fragmento coincida con el código actual (si el código cambió, actualízalo).
5. Registra el cambio con la skill `registrar-bitacora`.
