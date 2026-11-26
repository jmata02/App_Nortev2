# Bitácora de desarrollo

Registro de **cada sesión de trabajo** (de una persona o de Claude Code). Sirve para saber qué
se hizo, quién lo hizo, qué decisiones se tomaron y qué falta. También alimenta el Avance 2
("problemas encontrados" y "ajustes desde el primer avance") y la defensa.

- La entrada más reciente va **arriba**, justo debajo de la línea `<!-- ENTRADAS -->`.

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
