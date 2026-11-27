# Bitácora de desarrollo

Registro de **cada sesión de trabajo** (de una persona o de Claude Code). Sirve para saber qué
se hizo, quién lo hizo, qué decisiones se tomaron y qué falta. También alimenta el Avance 2
("problemas encontrados" y "ajustes desde el primer avance") y la defensa.

- La entrada más reciente va **arriba**, justo debajo de la línea `<!-- ENTRADAS -->`.
- Una entrada por sesión. Si la sesión sigue el mismo día en el mismo módulo, se amplía la misma.
- Claude Code la actualiza antes de terminar y antes de cada commit (los hooks lo verifican).
- `CHANGELOG.md` es otra cosa: resume cada Pull Request.

## Formato

```markdown
### AAAA-MM-DD · <Integrante> · <Módulo> · `<rama>`

**Pedido:** qué se pidió en la sesión, en una o dos líneas.

**Hecho:**
- cambio concreto (archivo o componente y para qué sirve)

**Paradigma evidenciado:** cuál, en qué archivo y método (o "ninguno").

**Decisiones:** decisiones técnicas tomadas y por qué.

**Desvíos del documento:** diferencias con `docs/proyecto/PROPUESTA_AVANCE1.md` (o "ninguno").

**Problemas encontrados:** errores, bloqueos y cómo se resolvieron (o "ninguno").

**Pendiente:** lo que queda para la próxima sesión. Incluir "requiere migración" si aplica.
```

---

<!-- ENTRADAS -->

### 2026-11-27 · Jurgen (con Claude) · Web: tema "Norte tornasol" · `feature/tema-tornasol`

**Pedido:** cambiar los colores de la app porque otro grupo presentó una muy parecida con el
tema oscuro neón. Mantener el estilo de ciudad costera, pero "al revés": que predomine el
blanco, con los colores tornasol (celeste, azul, morado) y el rosado de un control de consola
como referencia, y la palmera del logo en blanco.

**Hecho:**
- `estilos/tokens.css`: paleta nueva clara. Blanco y lavanda de base, tinta índigo para el
  texto, tornasol (celeste → azul → índigo → violeta → morado) y rosa de acento; significados
  más oscuros para leerse sobre blanco (turquesa ingreso, coral gasto, ámbar pendiente).
  Nuevos `--degradado-tornasol`, `--degradado-atardecer`, `--sombra` y `--sombra-suave`.
  Desaparecen `--color-magenta` y `--color-cian`.
- `global.css`, `Layout.css`, `Acceso.css` y `Dashboard.css`: todas las transparencias pasan
  de `rgba()` a `color-mix()` con tokens. Ya no queda ningún color escrito a mano en la web
  (la excepción de `rgba()` que había en `tokens.css` deja de hacer falta).
- Paneles de vidrio blanco con borde tornasol al pasar el mouse; botones, filtros activos,
  menú activo, barras de metas y anillos con el degradado tornasol; tarjeta del saldo tornasol
  con texto blanco (la única tarjeta de color, como el centro del control).
- Pantalla de acceso: escena tornasol animada con un sol de franjas blanco → celeste → rosa →
  violeta y cuadrícula blanca; tarjeta blanca.
- Logo: palmera blanca sobre una insignia tornasol (`Logo.jsx`), también en el favicon.
- Chart.js: paleta tornasol, tooltips claros con texto oscuro y brillo más suave.
- Renombrados `FondoNeon` → `FondoTornasol` y `brilloNeon` → `brilloTornasol`.

**Paradigma evidenciado:** ninguno (diseño visual).

**Decisiones:** se toman los colores y la palmera (motivo genérico); no se usa el logo ni nada
de la marca del videojuego. Tema claro único (`color-scheme: light`).

**Desvíos del documento:** ninguno. Segundo cambio de diseño respecto al prototipo del Avance 1
(mencionarlo en el Avance 2 junto con el anterior).

**Problemas encontrados:** el atributo `stroke` de un SVG no acepta `var(--...)`: la palmera
toma el color por `style`. La rejilla de Chart.js usa la tinta con transparencia en hexadecimal
porque el canvas no entiende `color-mix()`.

**Verificación:** lint y build de la web limpios (Docker). En el navegador: acceso, Dashboard con
datos reales, Presupuestos y Metas se ven con el tema nuevo y sin errores en la consola.

**Ajuste (mismo día): logo y acceso con atardecer rosado.** Jurgen pidió un logo con los
rosados y el degradado de una portada que le gustó (solo como referencia de color y estilo;
no se usa nada de su arte ni de su nombre).
- `tokens.css`: nuevos `--color-rosa-fuerte`, `--color-rosa-chicle`, `--color-rosa-claro`,
  `--color-lavanda`, `--color-pervinca` y `--color-ciruela`. `--degradado-atardecer` pasa a
  rosa fuerte → rosa → lavanda → pervinca (el pervinca enlaza con el tornasol) y se agrega
  `--degradado-sol`.
- Logo estilo sticker: insignia con el atardecer rosado, contorno y sombra ciruela, palmera
  blanca, "NORTE" en Unbounded 900 rosa claro con contorno ciruela (`paint-order`) y
  "finanzas" en turquesa inclinado. Se carga el peso 900 de Unbounded en `index.html`.
- Pantalla de acceso: la escena usa el atardecer rosado, el sol va de blanco a rosa claro y
  celeste, y "bajo control" tiene el mismo estilo del logo. Favicon con la insignia rosada.
- Dentro de la app se mantiene el tornasol (saldo, botones, menú): las dos paletas conviven.

**Ajuste (mismo día): armonía en toda la app.** Jurgen sintió que los botones azules no
combinaban con el logo rosado. Decisión: tres reglas de armonía.
1. **Un solo degradado de marca**: `--degradado-tornasol` pasa a rosa fuerte → rosa chicle →
   violeta → índigo → azul (empieza como el logo y termina como el control). Lo usan botones,
   menú activo, filtros, pestañas, saldo, barras de metas y anillos. `--color-marca` = rosa fuerte.
2. **Estilo sticker repetido**: contorno y sombra sólida ciruela (`--sombra-sticker`) en el botón
   principal, los botones secundarios (blancos con contorno; se llenan con el degradado al pasar
   el mouse), el menú activo, las pestañas, los filtros activos y la tarjeta del saldo. Al
   presionar, el botón baja 2 px ("se hunde").
3. **Todo acompaña**: fondo con un toque rosado (`#faf7fc`) y reflejos rosa, pervinca y lavanda;
   tinta y sombras con tinte violeta; foco y campos en violeta y rosa; cuadrícula del fondo en
   rosa y lavanda; gráficas y categorías en rosa chicle, violeta, pervinca, rosa fuerte y lavanda.
   Los colores con significado no cambian (turquesa, coral, ámbar).

**Ajuste: gastos en frambuesa.** El coral rojo de gastos y alertas no armonizaba con los
rosados. Se cambió `--color-coral` por `--color-frambuesa` (`#d6336c`): un rosado intenso
que combina con la marca, pero distinto del rosa de los botones para que siga leyéndose como
"dinero que sale" o "atención" (además del signo "−" y la flecha hacia abajo). El turquesa de
ingresos no cambia. Los estilos ya no nombran el color, sino su significado (`--color-alerta`).

**Ajuste: sin contorno, con resplandor.** A Jurgen no le gustó el contorno grueso ciruela de los
botones sobre el blanco. Se quitó de los botones, el menú activo, las pestañas, los filtros, la
tarjeta del saldo y la insignia del logo, y se reemplazó por un resplandor suave del color de la
marca (`--brillo-marca`, `--brillo-marca-fuerte` y `--brillo-marca-presionado`, rosa y violeta
difuminados) que se pierde en el blanco. Se eliminaron `--sombra-sticker` y su variante. El
contorno ciruela queda solo en las letras del logo ("NORTE", "finanzas") y en "bajo control".
En el celular se ocultó la barra de desplazamiento gris del menú horizontal. Después, el
resplandor (desplazado 10 px hacia abajo y muy opaco) dejaba una mancha debajo de los botones:
se cambió por un halo centrado, pequeño y tenue (casi sin desplazamiento), también en la
tarjeta del saldo y en los botones secundarios.

**Problemas encontrados (ajuste):** los CSS que git reescribió al cambiar de rama tienen saltos
de línea CRLF; los reemplazos se hicieron con un script que normaliza los saltos de línea.

**Verificación:** lint y build limpios; en el navegador, acceso y Dashboard con la paleta
unificada.

**Pendiente:** PR hacia `dev`. Avisar al grupo del cambio de colores (sus páginas lo heredan solo).

### 2026-11-26 · Jurgen (con Claude) · Agente verificador de integración · `chore/agente-verificador`

**Pedido:** un agente que revise absolutamente todo lo que entra a `main` y a `dev` para no
tener problemas.

**Hecho:**
- `.claude/agents/verificador-integracion.md`: revisa el historial (commits directos, rama al
  día, conflictos con `git merge-tree`, mensajes y autores), archivos prohibidos y secretos,
  bitácora, changelog, migraciones (solo Jurgen), cambios de stack, `Program.cs`, dueños de
  módulos y reglas de código; corre compilación, pruebas, migraciones pendientes, lint y build
  de la web; para `main` agrega lo de una entrega. Da un veredicto LISTO / CON OBSERVACIONES /
  NO LISTO. No modifica nada ni cambia de rama.
- `.claude/skills/verificar-integracion/SKILL.md`: comando `/verificar-integracion [rama|dev|main]`.
- `CLAUDE.md` (tabla de agentes y skills) y `CONTRIBUTING.md` (verificar antes de cada merge).

**Paradigma evidenciado:** ninguno (proceso del repositorio).

**Decisiones:** el agente no cambia de rama: si la rama a verificar no está en la carpeta, solo
hace las revisiones estáticas y lo dice. No se agregó integración continua en GitHub (Actions):
sería otra herramienta fuera del documento; queda como idea a conversar con el grupo.

**Desvíos del documento:** ninguno.

**Problemas encontrados:** al probar el agente sobre `dev`, encontró que el PR #3
(`chore/administracion`) se hizo merge en **`main` en vez de `dev`**: `main` quedó con 2 commits
que `dev` no tiene (CHANGELOG del PR #2 y la sección de la demo). Esta rama sale de
`chore/administracion`, así que su PR hacia `dev` trae ese contenido y las dos ramas quedan
iguales. Se agregó al agente una revisión fija de "`main` adelante de `dev`" y de la base del PR.
El agente también señaló, como ya estaba registrado, que los 5 commits de la configuración
inicial tienen autor "Claude" (incluida la migración `Inicial`, generada en la compu de Jurgen);
no se reescribe la historia de `dev`.

**Pendiente:** PR de esta rama **con base `dev`**. Probar el agente con el primer PR de un
compañero (en una sesión nueva de Claude Code ya aparece como `verificador-integracion`).

### 2026-11-26 · Jurgen (con Claude) · Coordinación · `chore/administracion`

**Pedido:** confirmar que `dev` quedó bien después del PR #2 y avanzar con las tareas de
administración.

**Hecho:**
- Verificación de `dev` (`5351e32`): PR #1 y #2 integrados, `dotnet build` sin errores ni
  advertencias, 58/58 pruebas, sin cambios de modelo pendientes, lint y build de la web limpios,
  API (categorías, inicio de sesión, reportes, 401 sin token) y web funcionando en Docker. Las
  5 ramas de trabajo no tienen commits fuera de `dev`.
- `CHANGELOG.md`: entrada del PR #2 (el commit del changelog del PR #1 entró dentro de ese PR).
- README: sección "Demostración y defensa" con el procedimiento del día anterior y del día de
  la defensa. Tarea de riesgo 10 marcada en `DIVISION_TRABAJO.md`.

**Paradigma evidenciado:** ninguno (coordinación).

**Decisiones:** **riesgo 10 resuelto**: la demostración se hace con ejecución local en Docker en
la compu de Jurgen, con una segunda compu de respaldo; no se usa servidor en internet. Es la
opción de respaldo que ya traía la propuesta; se menciona en el Avance 2 como ajuste.

**Desvíos del documento:** ninguno (la propuesta dejaba el despliegue "pendiente de definir"
con la ejecución local como respaldo).

**Problemas encontrados:** ninguno. No está probado correr la demo sin internet (NuGet con
versiones `10.0.*` y Google Fonts); el README lo advierte.

**Pendiente:** proteger `main` y `dev` en GitHub (lo hace Jurgen desde Settings). Confirmar la
decisión de la demo con el grupo y elegir la compu de respaldo. Borrar las ramas integradas
cuando Jurgen lo decida. Preparar el Avance 2.

### 2026-11-26 · Jurgen (con Claude) · Dashboard y reportes · `feature/modulo-reportes`

**Pedido:** backend de reportes con reglas puras (paradigma funcional) y conectar el Dashboard.

**Hecho:**
- `Modelo/Reportes/`: `PeriodoMensual` (record inmutable que valida año y mes),
  `TotalesFinancieros`, `ResumenFinancieroDto`, `GastoPorCategoriaDto` y `TendenciaMensualDto`.
- `Norte.Reglas/ReglaResumenFinanciero.cs`: `CalcularTotales` (`Where` + `Aggregate`),
  `CalcularGastoPorCategoria` (`Where` + `GroupBy` + `Select`), `CalcularTendencia` y
  `CalcularVariacionPorcentual`, con porcentajes de 2 decimales y redondeo `AwayFromZero`.
- `Norte.DA/ReporteDA.cs` con `IDbContextFactory` (un contexto por consulta) y
  `Norte.Flujo/ReporteFlujo.cs`, que pide las consultas en paralelo con `Task.WhenAll` y recibe
  la fecha con `TimeProvider`.
- `ReportesController` con `[Authorize]`: `GET /api/reportes/resumen`, `/gasto-por-categoria` y
  `/tendencia` (400 si el periodo o los meses no son válidos). `ModuloReportes` registra todo.
- 28 pruebas nuevas (58 en total): totales, redondeo, que la entrada no cambie, gasto por
  categoría, tendencia con meses vacíos y cambio de año, y el Flujo con un reloj fijo.
- Web: `api/reportesApi.js`. El Dashboard trae resumen, gasto por categoría y tendencia del API
  real, con estados de carga, error ("Reintentar") y vacío, y se recarga solo cuando llega un
  cambio por `cambiosFinancieros$`. Metas, alerta y últimos movimientos siguen de ejemplo.
- `docs/EVIDENCIA_PARADIGMAS.md`: sección del paradigma funcional con fragmento y defensa.

**Paradigma evidenciado:** funcional en `Norte.Reglas/ReglaResumenFinanciero.cs`
(`CalcularTotales`, `CalcularGastoPorCategoria`, `CalcularTendencia`). Además, concurrente de
apoyo (`Task.WhenAll` + `IDbContextFactory` en `ReporteFlujo`/`ReporteDA`) y la suscripción
reactiva del Dashboard a `cambiosFinancieros$`.

**Decisiones:** "deudas activas" = `MontoPendiente > 0` y "metas en curso" = `FechaLimite >= hoy`,
contando las tablas sin tocar el código de Carlos y Jessica (confirmar con ellos). Sin año y mes
se usa el mes actual en UTC. La variación es `null` si el balance anterior fue 0.

**Desvíos del documento:** ninguno.

**Problemas encontrados:** se había usado `.Result` después de `Task.WhenAll`; se cambió por
`await` para cumplir CLAUDE.md. No requiere migración.

**Verificación:** `dotnet build` sin errores ni advertencias · 52/52 pruebas · lint y build de la
web limpios · contra la base real con transacciones de prueba locales: resumen 200 (variación
+149,03 %), gasto por categoría ordenado con porcentajes, tendencia con un mes vacío en 0,
periodo inválido 400 y sin token 401 · en el navegador el Dashboard muestra los datos reales y
al publicar en `cambiosFinancieros$` vuelve a pedir los tres reportes.

**Revisión (agente `revisor`) y correcciones:** sin bloqueantes de código. Se agregaron
pruebas de `PeriodoMensual` (año bisiesto, cambio de año, inmutabilidad, rangos), una prueba de
que el Flujo pasa el usuario y el "hoy" del reloj a los conteos, y `CalcularGastoPorCategoria` a
la prueba de que la entrada no cambia (58 pruebas en total). La suscripción del Dashboard usa
`debounceTime(300)` para juntar ráfagas de cambios. Swagger documenta el 401. La etiqueta del
saldo dice "Balance de {mes} (ingresos menos gastos)".

**Pendiente:** commit y PR. No requiere migración. Con Jessica: "metas en curso" hoy también
cuenta metas ya cumplidas; cuando `MetaAhorro` tenga sus aportes calculados, agregar
`suma de aportes < MontoObjetivo`. Con Carlos: confirmar que `MontoPendiente` se actualiza en
cada pago. Con Mauricio: avisar que el Dashboard ya está suscrito a `cambiosFinancieros$`.
Cuando Jessica tenga `movimientosApi`, que publique en `cambiosFinancieros$` al guardar.

### 2026-11-26 · Jurgen (con Claude) · Verificación de `dev` tras el PR #1 · `chore/changelog-pr1`

**Pedido:** verificar que `dev` quedara sin errores después de hacer merge del PR #1.

**Hecho:**
- Verificación de `dev` (`87f5421`): `dotnet build` sin errores ni advertencias, 30/30 pruebas,
  sin cambios de modelo pendientes, migraciones al día, lint y build de la web limpios, API y
  web respondiendo en Docker, sin archivos de `bin/`, `obj/`, `dist/`, `node_modules/` ni `.env`
  en el repositorio.
- `CHANGELOG.md`: entrada del PR #1.
- `docker-compose.yml`: la web pasa `--host 0.0.0.0` también en el comando.

**Paradigma evidenciado:** ninguno (verificación y documentación).

**Decisiones:** ninguna nueva.

**Desvíos del documento:** ninguno.

**Problemas encontrados:** al cambiar de rama, Vite se reinició solo mientras git reescribía los
archivos y quedó escuchando solo dentro del contenedor (la web dejó de abrir en Windows). Se
resolvió reiniciando el contenedor y, para que no se repita, el `--host` va también en el comando.

**Pendiente:** PR de esta rama hacia `dev`. Borrar las ramas ya integradas
(`chore/configuracion-claude-code`, `chore/entorno-docker`, `feature/modulo-autenticacion`).
Proteger `main` y `dev` en GitHub (PR obligatorio). Siguiente módulo de Jurgen: Reportes.

### 2026-11-26 · Jurgen (con Claude) · Web: tema "Norte neón" y acceso · `feature/modulo-autenticacion`

**Pedido:** el profe dijo que el prototipo se veía muy plano. El grupo aprobó un tema nuevo
(nocturno, neón, inspirado en la estética de ciudad costera de los videojuegos, sin usar marcas
ni recursos de ningún juego) y pidió pasarlo al proyecto igual al prototipo aprobado, con la
parte web de Autenticación funcional. A los otros módulos solo se les pone el diseño.

**Hecho:**
- Prototipo visual aprobado por el grupo (artefacto "Norte Neón"), con la palmera como logo.
- `estilos/tokens.css` y `estilos/global.css`: paleta con significado (magenta = marca,
  cian = ingreso / al día, coral = gasto / excedido, ámbar = pendiente), fuentes Unbounded,
  Manrope y JetBrains Mono, fondo de atardecer con grano, paneles de vidrio, chips, barras que se
  llenan, anillo, filtros, campos y la entrada escalonada. Respeta "reducir movimiento".
- Componentes compartidos en `componentes/`: `FondoNeon`, `Logo` (palmera), `Panel`
  (inclinación 3D y luz que sigue al mouse), `CifraAnimada`, `MiniGrafica`,
  `Indicadores` (`Chip`, `BarraProgreso`, `Anillo`), `Icono`, `FilaMovimiento`,
  `AvisoVistaPrevia`, `RutaProtegida`, `graficas.js` (Chart.js con brillo neón) y el
  `Layout` nuevo (menú de vidrio, usuario y botón Salir).
- `api/clienteHttp.js`: `guardarSesion`, `obtenerUsuario`, `cerrarSesion`, `haySesion`
  (revisa el vencimiento). Un 401 con token borra la sesión y manda a `/iniciar-sesion`.
  Muestra los mensajes de validación del API.
- Autenticación (web): `PantallaAcceso` con pestañas para `/iniciar-sesion` y `/registro`,
  conectada a `api/autenticacionApi.js`. Rutas internas protegidas.
- Dashboard (Jurgen) con el diseño completo y datos de ejemplo en `dashboard/datosEjemplo.js`,
  con la forma que tendrán las respuestas de `/api/reportes`.
- Movimientos, Metas (Jessica), Deudas (Carlos) y Presupuestos (Mauricio): **solo diseño**,
  con datos de ejemplo marcados con `AvisoVistaPrevia` y un `TODO` para el responsable. Los
  porcentajes y estados vienen como datos (no se calculan en la web). Categorías con el estilo nuevo.
- `public/favicon.svg` con la palmera. Se borró `EnConstruccion.jsx` (ya no se usa).

**Paradigma evidenciado:** orientado a eventos en `PantallaAcceso.jsx` (`manejarCambio`,
`manejarEnvio`) y `Layout.jsx` (`manejarCerrarSesion`).

**Decisiones:** todo en CSS y JavaScript puro, sin librerías nuevas. Las pantallas de otros
módulos usan datos de ejemplo dentro de su propia página para que cada responsable los cambie
por su API sin tocar el diseño. La lógica de su paradigma (formularios de Jessica, concurrencia de
Carlos, flujo reactivo de Mauricio) queda para cada responsable, con autorización de Jurgen.

**Desvíos del documento:** ninguno en alcance ni stack. Cambia el diseño visual respecto al
prototipo presentado en el Avance 1 (decisión del grupo, para mencionarla en el Avance 2).

**Problemas encontrados:** el lint marcó dos advertencias de React: `new Date()` dentro del
render del Dashboard (se pasó a `useState(() => new Date())`) y un `setState` dentro del efecto
de `CifraAnimada` (ahora, con "reducir movimiento", se muestra el valor directo sin efecto).

**Verificación:** `npm run build` ✔ · `npm run lint` en Docker sin advertencias ✔. En el
navegador contra el API real: `/dashboard` sin sesión redirige a `/iniciar-sesion` ✔ · la
contraseña débil se rechaza antes de enviar ✔ · el registro crea la cuenta y entra al Dashboard ✔
· "Salir" borra la sesión ✔ · contraseña mala muestra "Correo o contraseña incorrectos." ✔ ·
inicio de sesión con el correo en mayúsculas entra ✔ · Dashboard y Presupuestos se ven como el
prototipo en escritorio y en celular, sin errores en la consola ✔. No se probó el 401 con token
vencido porque todavía no hay endpoints con `[Authorize]`.

**Revisión (agente `revisor`) y correcciones:**
- Colores escritos a mano en JS/JSX: ahora salen de `tokens.css` (`var(--color-...)` en estilos
  y `colorToken()` para Chart.js). `datosEjemplo.js` ya no trae colores. Excepción acordada y
  anotada en `tokens.css`: los brillos y sombras de los CSS usan `rgba()` de los mismos colores.
- Un inicio de sesión fallido con un token vencido recargaba la página y ocultaba el error:
  `haySesion()` ahora borra la sesión vencida y registro / inicio de sesión no mandan token
  (`conToken: false`). `guardarToken` dejó de exportarse.
- Las formas de datos de ejemplo de Movimientos, Deudas, Presupuestos y Metas quedan marcadas
  como **propuesta de contrato** a acordar con cada responsable (riesgo 8). Deudas usa montos
  numéricos en vez de texto ya formateado.
- Series del Dashboard movidas a `datosEjemplo.js`; aviso de vista previa unificado.
- `Usuario.Crear` mide el nombre ya recortado (mínimo 2 letras) y tiene su prueba (30/30).
- Las mini gráficas tenían huecos en la línea por la animación del trazo; ahora se revelan con
  `clip-path`.
- No se corrigió (anotado): `Usuario` tiene poco comportamiento propio; la evidencia fuerte de
  OO será `Transaccion`, `Deuda`, `Presupuesto` y `MetaAhorro`. El tiempo de respuesta del inicio
  de sesión puede revelar si un correo existe (fuera del alcance del curso).

**Pendiente:** PR hacia `dev` y entrada en `CHANGELOG.md` al cerrarlo. En el PR, avisar los
archivos compartidos tocados (`ExcepcionDominio.cs`, `ExcepcionesFiltro.cs`, `clienteHttp.js`,
`tokens.css`, `global.css`, `Layout.*`) y las páginas de Jessica, Carlos y Mauricio, que tienen
el diseño con datos de ejemplo.

### 2026-11-24 · Jurgen (con Claude) · Autenticación · `feature/modulo-autenticacion`

**Pedido:** módulo de Autenticación: registro e inicio de sesión con JWT (backend, pruebas y web).

**Hecho (backend y pruebas):**
- `Usuario`: setters privados y fábrica `Crear(...)` que valida nombre, correo, hash y fecha UTC.
- DTOs en `Modelo/Usuarios/`: `RegistroSolicitudDto`, `InicioSesionSolicitudDto` (con
  DataAnnotations), `SesionRespuestaDto` y `TokenGenerado`.
- `CredencialesInvalidasException` (401) en `ExcepcionDominio.cs` y su caso en `ExcepcionesFiltro.cs`.
- Interfaces: `IUsuarioDA`, `IAutenticacionFlujo`, `IHasherContrasena`, `IGeneradorToken`,
  `IReglaContrasenaSegura`, `IReglaCorreoNormalizado`, `IAutenticacionController`.
- `Norte.Reglas`: `ReglaContrasenaSegura` (8+ caracteres, letra, número, sin espacios en los
  bordes) y `ReglaCorreoNormalizado` (trim + minúsculas).
- `UsuarioDA` (convierte la violación del índice único en `ConflictoException`),
  `AutenticacionFlujo`, `HasherContrasena` (PasswordHasher de Identity), `GeneradorTokenJwt`
  (claim `sub` = Id), `AutenticacionController` y `ModuloAutenticacion`.
- Endpoints: `POST /api/autenticacion/registro` (201 con token, 400, 409) y
  `POST /api/autenticacion/inicio-sesion` (200, 400, 401).
- 22 pruebas nuevas: reglas, entidad `Usuario` y `AutenticacionFlujo` con DA, hasher y token falsos.

**Paradigma evidenciado:**
- Funcional: `Norte.Reglas/ReglaContrasenaSegura.cs` → `ObtenerIncumplimientos` (requisitos
  como datos, `Where` + `Select` sin estado).
- Orientado a objetos: `Usuario.Crear` (encapsulamiento con setters privados).

**Decisiones:** credenciales incorrectas → 401 con el mismo mensaje si el correo no existe o la
contraseña es mala (no revela cuentas). El registro devuelve el token (sesión iniciada). Hasher y
generador de token viven en `Norte.Api/Servicios` junto a `UsuarioActual`. En la web, un 401
borrará el token y llevará a `/iniciar-sesion`.

**Desvíos del documento:** menor. La propuesta dice "ASP.NET Core Identity (hash de
contraseñas) + JWT"; se usa solo el `PasswordHasher` de Identity (PBKDF2 con sal) sobre la tabla
`usuarios` existente, sin `AddIdentity`/`UserManager` ni tablas `AspNet*`. Motivo: el documento
pide Identity para el hash, el JWT ya cubre la sesión y el sistema completo agregaría ~7 tablas y
otra migración. Decidido por Jurgen; avisar al grupo y corregirlo en el documento del Avance 2.

**Problemas encontrados:** ninguno. Setters privados en `Usuario` no requieren migración
(`has-pending-model-changes` sin cambios).

**Verificación:** `dotnet build` sin errores · 29/29 pruebas en Docker · en el API real: registro
201, correo repetido 409, contraseña débil 400, correo inválido 400, inicio de sesión 200,
credenciales malas 401; el token trae `sub`, `iss`, `aud` y vence a las 2 h; el hash guardado es
de Identity (`AQAAAAIAAY...`) y no hay tablas `AspNet*`.

**Pendiente:** web (inicio de sesión, registro, rutas protegidas, cerrar sesión, 401 → salir),
revisión con el agente `revisor`, commit y PR. Archivos compartidos tocados (avisar en el PR):
`ExcepcionDominio.cs`, `ExcepcionesFiltro.cs`.

### 2026-11-24 · Jurgen (con Claude) · Entorno de desarrollo en Docker · `chore/entorno-docker`

**Pedido:** que todo el proyecto corra dentro de Docker para que ningún integrante tenga que
desactivar Smart App Control de Windows 11.

**Hecho:**
- `docker-compose.yml`: además de `postgres` (ahora con healthcheck), servicios `api`
  (SDK de .NET 10, `dotnet run` en http://localhost:5147), `migraciones` (`dotnet ef`),
  `pruebas` (`dotnet test`) y `web` (Node 26, Vite en http://localhost:5173). La cadena de
  conexión y la clave JWT llegan por variables de entorno desde `.env`.
- `Directory.Build.props`: dentro de Docker, `bin/` y `obj/` van a un volumen aparte y se
  ignoran los de Windows, para que no choquen con Visual Studio.
- `.config/dotnet-tools.json`: fija `dotnet-ef` 10.0.15 como herramienta local.
- `.env.example`: nueva variable `JWT_CLAVE`.
- `web/norte-web/vite.config.js`: en Docker escucha en `0.0.0.0` y revisa cambios por sondeo.
- `scripts/configurar-entorno.cmd`: ahora solo necesita Git y Docker Desktop; levanta todo,
  aplica migraciones y corre las pruebas en contenedores.
- `.claude/hooks/guardia-bash.mjs`: el aviso de migraciones también reconoce la forma con Docker.
- README: puesta en marcha con Docker.
- **Se aplicó la migración `Inicial`**: 9 tablas creadas y 11 categorías de semilla.

**Paradigma evidenciado:** ninguno (infraestructura).

**Decisiones:** correr API, migraciones, pruebas y web en contenedores en lugar de pedir que
desactiven Smart App Control (al desactivarlo puede que no se pueda volver a activar). Las
variables de entorno reemplazan a user-secrets dentro de Docker; los secretos siguen fuera del
repositorio, en `.env`.

**Desvíos del documento:** ninguno. El documento ya usa Docker Compose para el desarrollo
(riesgo 9); ahora también corre el API y la web. El stack y las versiones no cambian.

**Problemas encontrados:** Smart App Control bloqueaba `Norte.DA.dll` ("An Application Control
policy has blocked this file"), así que no se podían aplicar migraciones ni correr el API, y
también bloqueaba el binario nativo de oxlint (`npm run lint`). En el contenedor:
`dotnet ef` no restaura paquetes por sí mismo (se compila antes) y los `obj/` de Windows se
compilaban dos veces (CS0579), por eso se excluyen.

**Verificación:** `docker compose run --rm migraciones` ✔ · `GET /api/categorias` devuelve las
11 categorías ✔ · Swagger 200 ✔ · `docker compose run --rm pruebas` 7/7 ✔ ·
`npm run lint` y `npm run build` en el contenedor ✔ · `dotnet build` en Windows sigue sin
errores ✔ · `scripts\configurar-entorno.cmd` completo ✔.

**Pendiente:** push de `chore/entorno-docker` y PR hacia `dev` (revisado por otra persona).
Avisar al grupo que corran de nuevo `scripts\configurar-entorno.cmd` y agreguen `JWT_CLAVE`
a su `.env` (el script lo hace solo). Después, módulo de Autenticación.

### 2026-11-24 · Jurgen (con Claude) · Configuración del repositorio · `chore/configuracion-claude-code`

**Pedido:** preparar el repositorio para trabajar con Claude Code apegado al documento del
Avance 1, con agentes y un registro de todo lo que se haga.

**Hecho:**
- `CLAUDE.md` reescrito como contrato: orden de lectura, reglas no negociables, paradigmas,
  responsables, stack, capas, reglas de código, migraciones y uso de agentes.
- `docs/proyecto/`: resumen del enunciado (`REQUISITOS_CURSO.md`), resumen de la propuesta
  (`PROPUESTA_AVANCE1.md`) y el PDF del Avance 1.
- `.claude/agents/`: planificador, backend, frontend, pruebas y revisor.
- `.claude/skills/`: implementar-modulo, registrar-bitacora, evidencia-paradigma y preparar-entrega.
- `.claude/hooks/` + `.claude/settings.json`: contexto al iniciar sesión, guardia de comandos
  (migraciones, push a `main`/`dev`, commits sin bitácora) y verificación de bitácora al terminar.
- `Norte.Api.csproj`: `Microsoft.EntityFrameworkCore.Design` cambiado por
  `Microsoft.EntityFrameworkCore.Tools`, que es el paquete que nombra el documento.

**Paradigma evidenciado:** ninguno (configuración).

**Decisiones:** el paradigma reactivo se implementa con RxJS en la web (el documento decía
"Rx.NET o RxJS"); Rx.NET queda como trabajo a futuro, igual que en el documento.

**Desvíos del documento:** ninguno.

**Problemas encontrados:** el esqueleto no se pudo compilar completo fuera de las máquinas del
grupo (sin acceso a NuGet); hay que validar `dotnet build` en la primera sesión.

**Pendiente:** Jurgen: `dotnet build`, migración `Inicial` y autenticación. Cada integrante:
empezar su módulo con `/implementar-modulo`.

**Actualización (mismo día):** el repositorio oficial pasa a ser `jmata02/App_Nortev2` (el de
Carlos no permitía subir desde Claude). Se agregó `scripts/configurar-entorno.cmd`, que deja el
entorno de Windows listo en un paso, y `.gitattributes` para que los `.cmd` mantengan saltos de
línea CRLF. La configuración entró a `dev` sin Pull Request por pedido de Jurgen (coordinador),
para que todos partan de ella.

**Verificación en la compu de Jurgen (Windows, .NET SDK 10.0.400, Node 24):** el script compiló
la solución por primera vez con NuGet y encontró un error real: `CS8506` en
`ExcepcionesFiltro.cs` (el `switch` no tenía un tipo común); se corrigió declarando
`IActionResult?`. Después compiló sin errores, se generó la migración **`Inicial`**
(`src/Norte.DA/Migrations`, con `numeric(18,2)` y `xmin`) y se instalaron las dependencias de
la web. Se agregó `dotnet test` al script.

**Pendiente:** instalar Docker Desktop en la compu de Jurgen y volver a correr el script para
crear las tablas (`dotnet ef database update`). Node 24 funciona, pero el documento recomienda
Node 26 LTS (`.nvmrc`).

### 2026-11-15 · Jurgen (con Claude) · Esqueleto · `main`

**Pedido:** dividir el trabajo entre los cuatro integrantes y subir el esqueleto a GitHub.

**Hecho:**
- Solución .NET 10 en capas, modelo de los cinco módulos, módulo de referencia Categorías,
  web React + Vite, Docker Compose, README, CONTRIBUTING y `docs/DIVISION_TRABAJO.md`.

**Paradigma evidenciado:** ninguno todavía (solo los lugares preparados para cada uno).

**Decisiones:** se reutilizó la organización en capas del proyecto del cuatrimestre anterior,
cambiando Dapper + SQL Server por EF Core + PostgreSQL para cumplir el documento.

**Desvíos del documento:** ninguno.

**Problemas encontrados:** ninguno.

**Pendiente:** primera migración, autenticación y los cinco módulos.
