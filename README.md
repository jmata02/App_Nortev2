# Norte: Sistema de Gestión de Finanzas Personales

Proyecto final del curso **SC-250 Paradigmas de Programación**, Universidad Fidélitas. Grupo 4.

Norte ayuda a las personas a registrar sus ingresos y gastos, controlar deudas y pagos
recurrentes, definir presupuestos por categoría, ver reportes y cumplir metas de ahorro.

| Integrante | Módulos | Paradigma que defiende |
|------------|---------|------------------------|
| Jurgen Mata Vargas | Autenticación, Dashboard y reportes, coordinación técnica | Funcional |
| Jessica Marín Céspedes | Movimientos, Metas de ahorro | Orientado a objetos y orientado a eventos |
| Carlos Huete Carballo | Deudas y pagos recurrentes | Concurrente |
| Mauricio Prendas Roldán | Presupuestos, capa reactiva de la web | Reactivo |

El detalle de qué hace cada persona está en [docs/DIVISION_TRABAJO.md](docs/DIVISION_TRABAJO.md).

## Stack

| Capa | Tecnología |
|------|------------|
| Backend | ASP.NET Core 10 Web API (controladores), C# |
| Acceso a datos | Entity Framework Core 10 + Npgsql |
| Base de datos | PostgreSQL 18 (Docker) |
| Frontend | React 19 + Vite, JavaScript, React Router, RxJS, Chart.js |
| Pruebas | xUnit |
| Control de versiones | Git + GitHub |

## Estructura

```
App_Norte/
├── Norte.slnx                      Solución (abrir en Visual Studio 2026)
├── src/
│   ├── Norte.Abstracciones/        Entidades, DTOs y TODAS las interfaces
│   │   ├── API/                    Interfaces de los controllers
│   │   ├── DA/                     Interfaces de acceso a datos (I<Entidad>DA)
│   │   ├── Flujo/                  Interfaces de los casos de uso
│   │   ├── Reglas/                 Interfaces de las reglas de negocio
│   │   ├── Servicios/              Interfaces de servicios transversales
│   │   └── Modelo/                 Entidades y DTOs, una carpeta por módulo
│   ├── Norte.DA/                   EF Core: NorteDbContext, Configuraciones/ y un <Entidad>DA por entidad
│   ├── Norte.Reglas/               Reglas de negocio puras (sin BD ni HTTP)
│   ├── Norte.Flujo/                Orquesta DA + Reglas para cada caso de uso
│   ├── Norte.Servicios/            Tareas en segundo plano (pagos recurrentes)
│   └── Norte.Api/                  Controllers, Program.cs, filtro de errores, JWT
│       └── Configuracion/Modulos/  Un archivo de registro de dependencias por módulo
├── tests/Norte.Pruebas.Unitarias/  Pruebas de Reglas y Flujo
├── web/norte-web/                  React + Vite
├── docs/                           División de trabajo y evidencia por paradigma
└── docker-compose.yml              PostgreSQL para desarrollo
```

Las dependencias van siempre hacia `Norte.Abstracciones`: cada capa depende de
interfaces y recibe sus dependencias por constructor.

**Módulo de referencia:** *Categorías* ya está hecho en todas las capas
(`CategoriaDA` → `CategoriaFlujo` → `CategoriasController` → `CategoriasPagina.jsx`).
Cuando empiecen su módulo, copien esa forma.

## Puesta en marcha

Todo corre **dentro de Docker**: PostgreSQL, el API, las migraciones, las pruebas y la web.
En Windows 11, *Smart App Control* bloquea las DLL recién compiladas y algunos binarios de
`node_modules` ("An Application Control policy has blocked this file"). Dentro de los
contenedores el código corre en Linux y ese bloqueo no aplica, así que **nadie tiene que
desactivar protecciones de su equipo** y todos usan exactamente las mismas versiones.

### Requisitos

- Git y **Docker Desktop** (abierto, en "Running").
- Para editar código: Visual Studio 2026 o VS Code. El .NET 10 SDK y Node.js 26 en Windows
  son opcionales (sirven para compilar desde el editor, pero no para ejecutar si Smart App
  Control está activo).

### Opción rápida (Windows)

Desde la raíz del repositorio:

```cmd
scripts\configurar-entorno.cmd
```

El script crea `.env` con claves nuevas (base de datos y JWT), levanta PostgreSQL, aplica las
migraciones, corre las pruebas y deja el API y la web corriendo. Se puede correr varias veces.
La primera vez descarga las imágenes de .NET y Node, así que tarda unos minutos.

### A mano

```bash
cp .env.example .env                      # cambien las claves
docker compose up -d                      # PostgreSQL + API + web
docker compose run --rm migraciones       # crear o actualizar las tablas
```

| Qué | Dónde o cómo |
|-----|--------------|
| Swagger del API | http://localhost:5147/swagger (prueben `GET /api/categorias`) |
| Web | http://localhost:5173 (se recarga sola al guardar) |
| Después de cambiar código C# | `docker compose restart api` |
| Ver errores del API | `docker compose logs -f api` |
| Pruebas | `docker compose run --rm pruebas` |
| Lint y build de la web | `docker compose run --rm web npm run lint` · `docker compose run --rm web npm run build` |
| Nueva migración (solo Jurgen) | `docker compose run --rm migraciones migrations add <Nombre> --project src/Norte.DA --startup-project src/Norte.Api` |
| Apagar todo | `docker compose down` (los datos se conservan) |

Cómo funciona: `docker-compose.yml` monta el repositorio en contenedores con el SDK de .NET 10
y Node 26. Las claves salen de `.env` como variables de entorno. `Directory.Build.props` manda
los `bin/` y `obj/` del contenedor a un volumen aparte para que no choquen con los de Visual
Studio, y `.config/dotnet-tools.json` fija la versión de `dotnet-ef`.

Si alguien no tiene Smart App Control activo, también puede correr todo directo en Windows
(`dotnet run --project src/Norte.Api --launch-profile http`, con la cadena de conexión y
`Jwt:Clave` en user-secrets).

## Trabajar con Claude Code

El repositorio ya trae la configuración para que Claude Code trabaje apegado al documento del
Avance 1:

| Archivo | Para qué sirve |
|---------|----------------|
| `CLAUDE.md` | Contrato: reglas, paradigmas, responsables, capas y comandos. Claude lo lee siempre. |
| `docs/proyecto/` | Resumen del enunciado, resumen de la propuesta y el PDF del Avance 1. |
| `docs/BITACORA.md` | Registro de cada sesión de trabajo (quién, qué, decisiones, desvíos, pendientes). |
| `.claude/agents/` | Agentes: `planificador`, `backend`, `frontend`, `pruebas` y `revisor`. |
| `.claude/skills/` | Comandos: `/implementar-modulo`, `/registrar-bitacora`, `/evidencia-paradigma`, `/preparar-entrega`. |
| `.claude/hooks/` | Revisiones automáticas: contexto al iniciar, bloqueo de push a `main`/`dev`, aviso antes de migraciones y bitácora obligatoria al terminar y al hacer commit. |

Para empezar, cada integrante abre Claude Code en la carpeta del repositorio, en la rama de su
módulo, y escribe por ejemplo:

```
/implementar-modulo movimientos registrar y listar gastos
```

Requisitos: Node.js instalado (los hooks están en Node) y, en Windows, Git para Windows
(Claude Code usa Git Bash). Configuren su nombre en git (`git config --global user.name`)
porque la bitácora y los hooks lo usan para saber quién está trabajando.

## Cómo trabajamos

Ver [CONTRIBUTING.md](CONTRIBUTING.md): ramas por módulo, Pull Requests revisados por otra
persona y una sola persona encargada de las migraciones.
