---
name: registrar-bitacora
description: Agrega o actualiza la entrada de la sesión actual en docs/BITACORA.md con el formato del proyecto Norte. Úsala al terminar una tarea, antes de cada commit o cuando el usuario pida "registra lo que hicimos".
---

# Registrar en la bitácora

1. Reúne los datos de la sesión:
   - Fecha de hoy, integrante (`git config user.name`; si no es claro, pregunta) y rama actual.
   - Cambios: `git status --short` y `git diff --stat` (incluye lo ya preparado con
     `git diff --cached --stat`).
   - Lo conversado en la sesión: pedido, decisiones, problemas y desvíos.
2. Abre `docs/BITACORA.md`. Si ya hay una entrada de hoy del mismo integrante y módulo,
   **amplíala**; si no, crea una nueva **justo debajo** de la línea `<!-- ENTRADAS -->`.
3. Usa exactamente el formato del archivo:
   `### AAAA-MM-DD · <Integrante> · <Módulo> · `<rama>`` y las secciones Pedido, Hecho,
   Paradigma evidenciado, Decisiones, Desvíos del documento, Problemas encontrados y Pendiente.
4. Sé concreto: nombra archivos, clases, métodos y endpoints. Nada de "se hicieron mejoras".
5. Si algo se aparta de `docs/proyecto/PROPUESTA_AVANCE1.md`, escríbelo en "Desvíos del
   documento". Si el modelo de datos cambió, escribe "requiere migración" en Pendiente.
6. Muestra la entrada al usuario.
