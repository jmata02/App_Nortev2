# Changelog

Registro de cambios por módulo. Agreguen su entrada arriba al cerrar cada Pull Request.

## [2026-11-27] — PR #6: Contraste y accesibilidad del tema (Jurgen)

### Modificado
- Degradado de marca con todas sus paradas ≥ 5:1 con texto blanco; variantes de texto
  `--color-ingreso-texto`, `--color-alerta-texto` y `--color-pendiente-texto`; gris tenue a
  4.9:1; `--color-borde-campo` para que los campos se vean.
- Pantalla de acceso con el atardecer invertido (el lema queda sobre el rosa fuerte).
- Menú del celular con `aria-label`; gráficas con `role="img"`.
- Datos de ejemplo de Deudas con el porcentaje pagado correcto.
- Agente `verificador-integracion`: revisa colores en CSS, tokens borrados y contraste.
- Entradas de los PR #3, #4 y #5 en este CHANGELOG.

## [2026-11-27] — PR #5: Tema visual "Norte tornasol" (Jurgen)

### Modificado
- **Tema claro** que reemplaza al oscuro neón (se parecía al proyecto de otro grupo): predomina
  el blanco, con un solo degradado de marca rosa → violeta → azul en botones, menú, filtros,
  pestañas, saldo, barras de metas y anillos. Los botones flotan con un halo suave.
- **Logo**: palmera blanca en una insignia con atardecer rosado y "NORTE" en rosa claro con
  contorno; favicon nuevo. La pantalla de acceso usa el atardecer rosado.
- **Gastos y alertas** en frambuesa (antes coral); los ingresos siguen en turquesa.
- Todas las transparencias con `color-mix()` de tokens; gráficas de Chart.js en la misma paleta.
- Renombrados `FondoNeon` → `FondoTornasol` y `brilloNeon` → `brilloTornasol`.

### Tokens
- Nuevos: `--color-rosa-fuerte`, `--color-rosa-chicle`, `--color-rosa-claro`, `--color-lavanda`,
  `--color-pervinca`, `--color-ciruela`, `--color-frambuesa`, `--color-marca`,
  `--degradado-tornasol`, `--degradado-atardecer`, `--degradado-sol`, `--brillo-marca` (y
  variantes), `--sombra`, `--sombra-suave`.
- **Eliminados** (si los usan en su rama, cámbienlos): `--color-magenta` → `--color-marca`,
  `--color-cian` → `--color-ok`, `--color-coral` → `--color-alerta`.

## [2026-11-26] — PR #4: Agente verificador de integración (Jurgen)

### Agregado
- Agente `verificador-integracion` y skill `/verificar-integracion [rama|dev|main]`: revisan
  todo lo que entra a `dev` o `main` (historial, secretos, archivos prohibidos, conflictos,
  bitácora, changelog, migraciones, reglas de código) y corren compilación, pruebas y lint.
- `CONTRIBUTING.md`: verificar antes de cada merge.

### Incluye también
- El commit del PR #3, que entró a `main` por error en vez de `dev`; con este PR llegó a `dev`.

## [2026-11-26] — PR #3: Coordinación (Jurgen) — integrado en `main` por error

### Agregado
- Entrada del PR #2 en este CHANGELOG.
- README: sección "Demostración y defensa" (riesgo 10: demo local con Docker y compu de respaldo).
- Se hizo merge en `main` en lugar de `dev`. Su contenido llegó a `dev` con el PR #4; `main`
  solo conserva el commit del merge, sin contenido propio.

## [2026-11-26] — PR #2: Dashboard y reportes (Jurgen)

### Agregado
- **Paradigma funcional**: `ReglaResumenFinanciero` en `Norte.Reglas`, con funciones puras
  (`Where`, `GroupBy`, `Select`, `Aggregate`) para totales del mes, variación del balance,
  gasto por categoría y tendencia. Porcentajes con 2 decimales y redondeo `AwayFromZero`.
- `PeriodoMensual` (record inmutable que se valida solo) y los DTOs del módulo.
- `ReporteDA` con `IDbContextFactory` y `ReporteFlujo` con `Task.WhenAll` y `TimeProvider`.
- 28 pruebas nuevas (58 en total).
- Web: `reportesApi.js`; el Dashboard muestra datos reales y se recarga solo con
  `cambiosFinancieros$` (`debounceTime`).
- Evidencia del paradigma funcional en `docs/EVIDENCIA_PARADIGMAS.md`.

### Endpoints expuestos (todos con `[Authorize]`)
- `GET /api/reportes/resumen?anio=&mes=` — totales, balance, variación, deudas activas y metas en curso.
- `GET /api/reportes/gasto-por-categoria?anio=&mes=` — gasto por categoría con porcentaje.
- `GET /api/reportes/tendencia?meses=6` — ingresos, gastos y balance de los últimos meses (1 a 12).

### Incluye también
- La entrada del PR #1 en este CHANGELOG y `--host` fijo para Vite en Docker.

### Pendiente con otros módulos
- "Deudas activas" (`MontoPendiente > 0`) y "metas en curso" (`FechaLimite >= hoy`): confirmar
  con Carlos y Jessica. Movimientos debe llamar a `publicarCambio(...)` al guardar.

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
