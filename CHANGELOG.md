# Changelog

Registro de cambios por módulo. Agreguen su entrada arriba al cerrar cada Pull Request.

## [2026-11-26] — PR #1: Entorno Docker, Autenticación y tema "Norte neón" (Jurgen)

### Agregado
- **Entorno en Docker**: servicios `api`, `migraciones`, `pruebas` y `web` en `docker-compose.yml`
  (Smart App Control de Windows bloqueaba las DLL recién compiladas). `Directory.Build.props`
  separa los `bin/obj` del contenedor y `.config/dotnet-tools.json` fija `dotnet-ef`.
- **Autenticación**: registro e inicio de sesión con JWT (claim `sub` = Id del usuario),
  `PasswordHasher` de ASP.NET Core Identity, `ReglaContrasenaSegura` y `ReglaCorreoNormalizado`
  (funcionales), `Usuario.Crear` con encapsulamiento y 23 pruebas nuevas.
- **Web**: tema "Norte neón" (tokens, estilos y componentes compartidos), pantalla de acceso
  conectada al API, rutas protegidas y botón Salir. Dashboard con datos de ejemplo.

### Modificado
- `ExcepcionDominio.cs` + `ExcepcionesFiltro.cs`: `CredencialesInvalidasException` → 401.
- `clienteHttp.js`: manejo de sesión; un 401 con token cierra la sesión.
- Páginas de Movimientos, Metas, Deudas y Presupuestos: diseño nuevo con datos de ejemplo
  marcados y `TODO` para cada responsable.
- `scripts/configurar-entorno.cmd` y README: puesta en marcha con Docker.

### Endpoints expuestos
- `POST /api/autenticacion/registro` — crea la cuenta y devuelve la sesión (201, 400, 409).
- `POST /api/autenticacion/inicio-sesion` — devuelve la sesión (200, 400, 401).

### Desvío del documento
- Se usa solo el `PasswordHasher` de Identity, sin sus tablas (registrado en la bitácora).

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
