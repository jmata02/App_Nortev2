# Changelog

Registro de cambios por módulo. Agreguen su entrada arriba al cerrar cada Pull Request.

## [2026-11-24] — Configuración de Claude Code (Jurgen)

### Agregado
- `CLAUDE.md` como contrato del repositorio, apegado al documento del Avance 1.
- `docs/proyecto/`: resumen del enunciado, resumen de la propuesta y PDF del Avance 1.
- `docs/BITACORA.md`: registro obligatorio de cada sesión de trabajo.
- `.claude/agents/` (planificador, backend, frontend, pruebas, revisor), `.claude/skills/`
  (implementar-modulo, registrar-bitacora, evidencia-paradigma, preparar-entrega) y
  `.claude/hooks/` + `.claude/settings.json`.

### Modificado
- `Norte.Api.csproj`: `Microsoft.EntityFrameworkCore.Tools` en lugar de `...Design`, como dice el documento.
- README y CONTRIBUTING: sección de Claude Code y de la bitácora.
- `scripts/configurar-entorno.cmd`: configura el entorno de Windows en un paso.

## [2026-11-15] — Esqueleto inicial (todos)

### Agregado
- Solución .NET 10 en capas: Abstracciones, DA (EF Core + PostgreSQL), Reglas, Flujo, Servicios, Api y pruebas unitarias.
- Modelo de dominio de los cinco módulos y su mapeo en `NorteDbContext`.
- Módulo de referencia **Categorías** completo en todas las capas, con semilla de 11 categorías.
- Regla compartida `ReglaMontoPositivo` con pruebas.
- Web React 19 + Vite con rutas, layout, cliente HTTP y base del flujo reactivo (RxJS).
- `docker-compose.yml` con PostgreSQL 18, `global.json` y `.nvmrc` para fijar versiones.
- Documentación: README, división de trabajo, guía de colaboración y plantilla de evidencia por paradigma.

### Endpoints expuestos
- `GET /api/categorias` — lista el catálogo, con filtro opcional `?tipo=Ingreso|Gasto`.
- `GET /api/categorias/{id}` — detalle de una categoría.
