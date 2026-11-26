---
name: verificar-integracion
description: Verifica todo lo que va a entrar a dev o a main antes del merge (commits, autores, secretos, archivos prohibidos, conflictos, bitácora, changelog, migraciones, reglas de CLAUDE.md, compilación, pruebas y lint) con el agente verificador-integracion. Úsala antes de aprobar o hacer merge de cualquier Pull Request, o cuando digan "verifica dev", "¿está listo para merge?" o "preparemos main".
argument-hint: "[rama | dev | main]"
---

# Verificar una integración a dev o main

Objetivo: `$ARGUMENTS` (vacío = la rama actual hacia `dev`).

1. Traduce el objetivo al rango que espera el agente:
   - una rama o PR (`feature/modulo-x`) → esa rama contra `origin/dev`;
   - `dev` → auditar lo que ya está en `dev` (`origin/main..origin/dev`);
   - `main` → entrega de `dev` a `main` (`origin/main..origin/dev`, con el punto 7 del agente).
2. Lanza el agente `verificador-integracion` con ese rango y cualquier contexto útil (qué PR es,
   quién lo hizo, qué decisiones ya están registradas en la bitácora).
3. Muestra al integrante el veredicto tal cual y, debajo, un resumen corto: qué bloquea, qué
   conviene arreglar y qué no se pudo verificar.
4. No corrijas nada por tu cuenta. Si el integrante lo pide, arregla los hallazgos **en la rama
   del PR** (nunca en `dev` ni en `main`), registra en la bitácora y vuelve a verificar.
