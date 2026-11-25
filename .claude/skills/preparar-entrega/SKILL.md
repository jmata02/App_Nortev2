---
name: preparar-entrega
description: Prepara el material de una entrega de Norte (avance2 o final) comparando el estado del repositorio contra lo que exige el enunciado y lo comprometido en el Avance 1. Úsala cuando el grupo diga "preparemos el avance 2" o "preparemos la entrega final".
argument-hint: "avance2 | final"
---

# Preparar una entrega

Entrega: `$ARGUMENTS`.

1. Lee la lista de contenido mínimo de esa entrega en `docs/proyecto/REQUISITOS_CURSO.md`.
2. Arma una tabla **Requisito | Estado (listo / parcial / falta) | Dónde está | Qué falta**,
   revisando el código, `docs/EVIDENCIA_PARADIGMAS.md`, `docs/BITACORA.md` y `CHANGELOG.md`.
3. Verifica lo que pesa más:
   - **avance2**: al menos **dos paradigmas funcionando** con código ejecutable y fragmentos
     documentados; capturas o video; problemas encontrados y ajustes desde el Avance 1 (sácalos
     de las secciones "Problemas encontrados" y "Desvíos del documento" de la bitácora); plan
     para la entrega final.
   - **final**: los cinco paradigmas con evidencia, pruebas realizadas (ejecuta `dotnet test` y
     resume), manual de instalación (README), arquitectura final y tabla de evidencia.
4. Ejecuta `dotnet build Norte.slnx`, `dotnet test` y `npm run build` y reporta el resultado.
5. Redacta en `docs/entregas/<entrega>.md` los textos que van al documento: explicación técnica
   del avance, problemas encontrados, ajustes realizados y plan de trabajo, con lo que de
   verdad muestra el repositorio. Si algo no está hecho, dilo; no lo maquilles.
6. Lista la participación por integrante (`git shortlog -sn --all`) para confirmar que el
   repositorio muestra el trabajo de todos.
7. Registra la sesión con `registrar-bitacora`.
