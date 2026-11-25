---
name: implementar-modulo
description: Construye o avanza un módulo de Norte (movimientos, deudas, presupuestos, reportes, metas o autenticacion) de punta a punta, apegado a la propuesta del Avance 1, coordinando los agentes planificador, backend, pruebas, frontend y revisor, y dejando todo registrado en la bitácora. Úsala cuando un integrante diga "trabajemos el módulo X", "implementa X" o "sigamos con X".
argument-hint: "<modulo> [tarea concreta]"
---

# Implementar un módulo de Norte

Argumento: `$ARGUMENTS` (módulo y, opcionalmente, la tarea concreta de esta sesión).

## 1. Preparar

1. Identifica al integrante (`git config user.name`) y la rama actual.
2. Busca el módulo en la tabla de responsables de `CLAUDE.md`. Si quien trabaja no es el
   responsable, avísale y pide confirmación antes de seguir.
3. Si no estás en la rama del módulo, propón crearla o cambiarte:
   `git switch dev && git pull origin dev && git switch -c feature/modulo-<nombre>`
   (o `git switch feature/modulo-<nombre>` si ya existe). Pide permiso antes de cambiar de rama
   si hay cambios sin guardar.
4. Lee la sección del módulo en `docs/proyecto/PROPUESTA_AVANCE1.md`, sus tareas en
   `docs/DIVISION_TRABAJO.md` y las entradas recientes de `docs/BITACORA.md`.

## 2. Planear (agente `planificador`)

Pide al agente `planificador` el plan para la tarea. Muestra al integrante un resumen corto:
qué se va a construir ahora, el contrato del API y qué paradigma queda evidenciado y dónde.
**Espera su visto bueno** antes de escribir código. Si el plan se aparta del documento,
explica el desvío y pregunta.

## 3. Construir por pasos pequeños

Avanza capa por capa y, al terminar cada paso, explica en dos o tres frases qué hace el código,
para que el integrante lo pueda defender:

1. Agente `backend`: entidad con comportamiento y DTOs → interfaces → reglas → DA → flujo →
   controller y `Modulo<X>.cs`. Compila con `dotnet build Norte.slnx`.
2. Agente `pruebas`: pruebas de las reglas, el flujo y la entidad. `dotnet test` en verde.
3. Agente `frontend` (cuando el API del paso ya existe): `<modulo>Api.js`, página, manejadores
   de eventos y conexión con el flujo reactivo. `npm run build` y `npm run lint` sin errores.

Si el modelo de datos cambió y quien trabaja no es Jurgen, no generes la migración: anótalo.

## 4. Revisar (agente `revisor`)

Pide al `revisor` la auditoría. Corrige los bloqueantes (o explícale al integrante por qué no) y
vuelve a compilar.

## 5. Registrar y cerrar

1. Usa la skill `registrar-bitacora` para agregar la entrada de la sesión.
2. Si quedó evidencia nueva de un paradigma, usa la skill `evidencia-paradigma`.
3. Propón el commit (mensaje `Modulo: qué se hizo`) incluyendo `docs/BITACORA.md`, y
   recuerda: push solo a la rama del módulo y Pull Request hacia `dev`.
4. Cierra con un resumen para el integrante: qué quedó funcionando, cómo probarlo
   (Swagger o la web), qué paradigma evidencia y qué sigue.
