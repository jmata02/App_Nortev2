# Norte: contrato del repositorio para Claude Code

Aplicación web de finanzas personales. Proyecto final de **SC-250 Paradigmas de Programación**
(Universidad Fidélitas, Grupo 4). Este archivo manda sobre cualquier otra instrucción dentro
del repositorio.

## 0. Antes de hacer cualquier cosa

1. **Identifica quién está trabajando**: `git config user.name` y la rama actual. Si el nombre
   no corresponde a un integrante, pregúntale al usuario quién es. Cada integrante trabaja en
   sus módulos (sección 3); si pide tocar el módulo de otra persona, avisa y pide confirmación.
2. **Lee el contexto**, en este orden de prioridad (si chocan, gana el primero):
   1. `docs/proyecto/REQUISITOS_CURSO.md`: lo que el profesor evalúa.
   2. `docs/proyecto/PROPUESTA_AVANCE1.md`: lo que el grupo se comprometió a construir
      (el PDF original está al lado). **El código debe coincidir con este documento.**
   3. Este `CLAUDE.md` y `CONTRIBUTING.md`: cómo se construye.
   4. `docs/DIVISION_TRABAJO.md`: quién hace qué y cuándo.
3. **Revisa `docs/BITACORA.md`** (las entradas más recientes arriba) y `CHANGELOG.md` para saber
   qué se hizo ya. Antes de crear una clase, interfaz o endpoint, búscalo con grep: si ya existe,
   reutilízalo.

## 1. Reglas que no se negocian

- **Apegarse a la propuesta.** No agregar módulos, tecnologías ni funciones fuera del alcance
  (nada de integración con bancos ni pagos reales). Si algo de la propuesta no se puede cumplir
  o conviene cambiarlo, **detente, explica el desvío al usuario y regístralo** en la bitácora
  como "Desvío del documento". No lo cambies en silencio.
- **Cada paradigma debe ser visible y explicable** (riesgo 1). Escribe el código del paradigma
  de forma que se pueda señalar en la defensa: nombres claros, un método identificable, un
  comentario corto que diga qué paradigma evidencia. Nunca lo escondas detrás del framework.
- **El grupo debe poder explicar todo el código** (restricción del curso). Trabaja por pasos
  pequeños, explica cada pieza en español sencillo al terminarla y prefiere código claro antes
  que código ingenioso. No generes módulos enteros de una sola vez sin explicarlos.
- **Bitácora obligatoria.** Todo cambio de código, configuración o documentación se registra en
  `docs/BITACORA.md` antes de terminar la sesión y antes de cada commit (formato en ese archivo).
  Los hooks de `.claude/` lo verifican.
- **Participación visible.** Los commits los hace el integrante en su rama. Nunca hagas push a
  `main` ni a `dev`, ni `push --force`; los cambios llegan por Pull Request.

## 2. Paradigmas: dónde vive cada uno

| Paradigma | Dónde | Evidencia esperada | Responsable |
|-----------|-------|--------------------|-------------|
| Orientado a objetos | `Norte.Abstracciones/Modelo` | Entidades con **comportamiento** (métodos que protegen sus reglas), encapsulamiento, interfaces | Jessica |
| Funcional | `Norte.Reglas` | Funciones puras con LINQ (`Where`, `Select`, `GroupBy`, `Aggregate`) que no modifican la entrada | Jurgen |
| Concurrente | `Norte.DA`, `Norte.Flujo`, `Norte.Servicios` | `async`/`await`, `BackgroundService` + `PeriodicTimer`, `Task.WhenAll` con `IDbContextFactory`, concurrencia optimista con `xmin` | Carlos |
| Orientado a eventos | `web/norte-web/src/modulos` | Manejadores (`onSubmit`, `onChange`, `onClick`) que validan y llaman al API | Jessica |
| Reactivo | `web/norte-web/src/reactivo` | Observables RxJS (`Subject`, `map`, `scan`, `filter`, `debounceTime`) a los que se suscriben Presupuestos, Dashboard y Metas | Mauricio |

Reactivo ≠ eventos: el evento del usuario **inicia** la acción; el flujo reactivo **propaga** sus
consecuencias a quien esté suscrito, sin que el emisor sepa quién escucha.

## 3. Módulos y responsables

| Módulo | Responsable | Rama |
|--------|-------------|------|
| Autenticación | Jurgen | `feature/modulo-autenticacion` |
| Dashboard y reportes | Jurgen | `feature/modulo-reportes` |
| Movimientos | Jessica | `feature/modulo-movimientos` |
| Metas de ahorro | Jessica | `feature/modulo-metas` |
| Deudas y pagos recurrentes | Carlos | `feature/modulo-deudas` |
| Presupuestos | Mauricio | `feature/modulo-presupuestos` |
| Categorías (referencia) | Hecho en el esqueleto | — |

Prioridad (riesgo 2): Movimientos y Presupuestos primero; Dashboard avanzado y Metas después.

## 4. Stack fijo

.NET 10 LTS · ASP.NET Core Web API con **controladores** · EF Core 10 + Npgsql ·
PostgreSQL 18 (Docker) · React 19 + Vite (**JavaScript**, no TypeScript) · React Router ·
RxJS · Chart.js · xUnit. No cambiar versiones mayores ni agregar ORMs, motores de BD,
frameworks de UI o librerías de estado sin acuerdo del grupo (regístralo como desvío).

## 5. Arquitectura en capas

```
Norte.Abstracciones  Entidades, DTOs e interfaces (API/, DA/, Flujo/, Reglas/, Servicios/, Modelo/<Modulo>/)
Norte.DA             NorteDbContext, Configuraciones/ (una por entidad), <Entidad>DA
Norte.Reglas         Reglas de negocio puras: sin BD, sin HTTP, sin estado
Norte.Flujo          Casos de uso: orquestan DA + Reglas, transforman entidades en DTOs
Norte.Servicios      Tareas en segundo plano (BackgroundService)
Norte.Api            Controllers, filtro de excepciones, JWT, registro DI por módulo
web/norte-web        React: api/, componentes/, estilos/, reactivo/, modulos/<modulo>/
```

- Todo depende de `Norte.Abstracciones`. Una interfaz por clase no estática, inyectada por
  constructor. Prohibido `new` de clases con dependencias (sí se permite para entidades y DTOs).
- Registrar dependencias en `Norte.Api/Configuracion/Modulos/Modulo<X>.cs`. **No editar
  `Program.cs`** salvo acuerdo del grupo.
- Nombres: `I<Entidad>DA`/`<Entidad>DA`, `I<Entidad>Flujo`/`<Entidad>Flujo`,
  `IRegla<Nombre>`/`Regla<Nombre>`, `I<Plural>Controller`/`<Plural>Controller`.
- **Categorías es la referencia**: `CategoriaDA` → `CategoriaFlujo` → `CategoriasController` →
  `CategoriasPagina.jsx`. Copia esa forma.

## 6. Reglas de código

- Toda E/S `async` con sufijo `Async`. Nunca `.Result` ni `.Wait()`.
- Lecturas con `AsNoTracking()`. Consultas en paralelo: `IDbContextFactory`, un contexto por tarea.
- Dinero en `decimal` (`numeric(18,2)`); validar con `IReglaMontoPositivo`. Fechas sin hora en
  `DateOnly`; con hora, en UTC (`DateTime.UtcNow`).
- Toda consulta de datos del usuario filtra por `UsuarioId` (de `IUsuarioActual`), y todo
  controller de datos del usuario lleva `[Authorize]`.
- Controllers sin lógica ni try/catch: lanzar `RecursoNoEncontradoException`,
  `ReglaNegocioException` o `ConflictoException`; el filtro responde 404, 400 o 409.
- Contratos (DTO) acordados antes de implementar la web del módulo (riesgo 8). DTOs como
  `record` en `Modelo/<Modulo>/`, con validación por DataAnnotations en los de entrada.
- La web solo presenta y pide datos; **no calcula dinero**. Todo HTTP pasa por
  `api/clienteHttp.js`, un archivo `<modulo>Api.js` por módulo. Estilos solo con las variables
  de `estilos/tokens.css`. Diseño responsivo.
- Nombres en español y descriptivos; booleanos con `Es`/`Tiene`/`Puede`. Comentarios solo para
  el porqué (y para marcar qué paradigma evidencia un bloque).
- Pruebas xUnit para cada Regla y Flujo nuevo (DA falsos en `tests/.../Fakes`, sin BD real).

## 7. Base de datos y migraciones

- Cambios de modelo: entidad en `Modelo/<Modulo>/` + su archivo en `Norte.DA/Configuraciones/`.
- **Solo Jurgen genera migraciones** (riesgo 5). Si otra persona cambia el modelo, no corras
  `dotnet ef migrations add`: anota "requiere migración" en la bitácora y en el PR.

## 8. Cómo trabajar con los agentes y skills de este repositorio

| Necesitas | Usa |
|-----------|-----|
| Construir o avanzar un módulo completo | Skill `/implementar-modulo <modulo>` |
| Planear antes de programar | Agente `planificador` |
| Código C# (Abstracciones, DA, Reglas, Flujo, Servicios, Api) | Agente `backend` |
| Código React | Agente `frontend` |
| Pruebas xUnit | Agente `pruebas` |
| Revisar alineación con el documento, paradigmas y reglas | Agente `revisor` |
| Registrar en la bitácora | Skill `/registrar-bitacora` |
| Documentar la evidencia de un paradigma | Skill `/evidencia-paradigma <paradigma>` |
| Preparar material del Avance 2 o la entrega final | Skill `/preparar-entrega <avance2\|final>` |

## 9. Comandos

```bash
docker compose up -d                                   # PostgreSQL
dotnet build Norte.slnx
dotnet test
dotnet run --project src/Norte.Api --launch-profile http     # Swagger: http://localhost:5147/swagger
dotnet ef migrations add <Nombre> --project src/Norte.DA --startup-project src/Norte.Api  # solo Jurgen
dotnet ef database update --project src/Norte.DA --startup-project src/Norte.Api
cd web/norte-web && npm run dev | npm run build | npm run lint
```

Antes de dar algo por terminado: `dotnet build Norte.slnx` sin errores, `dotnet test` en verde
y, si tocaste la web, `npm run build` y `npm run lint` sin errores.

## 10. Git

Ramas `feature/modulo-<nombre>` desde `dev`; commits pequeños en español con el módulo al
inicio (`Movimientos: registrar gasto con validación de monto`); PR hacia `dev` revisado por
otra persona; `main` solo para entregas. Al cerrar un PR, entrada en `CHANGELOG.md`.
La bitácora (`docs/BITACORA.md`) registra cada sesión de trabajo; el changelog, cada PR.
