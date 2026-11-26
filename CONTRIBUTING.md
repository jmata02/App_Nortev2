# Cómo colaborar en Norte

El enunciado pide que el repositorio muestre la participación de todos, así que cada
persona trabaja en su rama y sube su propio código con commits frecuentes.

## Ramas

| Rama | Para qué |
|------|----------|
| `main` | Solo versiones entregadas (Avance 2 y entrega final). Nadie hace commit directo. |
| `dev` | Integración. Aquí se juntan los módulos por Pull Request. |
| `feature/modulo-<nombre>` | Trabajo de cada módulo. Sale de `dev` actualizado. |
| `fix/<descripcion>` | Correcciones puntuales. |

Ramas de módulo:

| Rama | Responsable |
|------|-------------|
| `feature/modulo-autenticacion` | Jurgen |
| `feature/modulo-reportes` | Jurgen |
| `feature/modulo-movimientos` | Jessica |
| `feature/modulo-metas` | Jessica |
| `feature/modulo-deudas` | Carlos |
| `feature/modulo-presupuestos` | Mauricio |

```bash
git switch dev
git pull origin dev
git switch -c feature/modulo-movimientos
# ...trabajar, commits pequeños...
git push -u origin feature/modulo-movimientos
```

Antes de abrir el PR, traigan lo último de `dev` (`git pull origin dev`) y resuelvan
conflictos en su rama.

## Bitácora

Cada sesión de trabajo (con o sin Claude Code) se registra en `docs/BITACORA.md` con el formato
de ese archivo: qué se hizo, el paradigma evidenciado, las decisiones, los desvíos del documento
y lo pendiente. Va en el mismo commit que el código. Con Claude Code, la skill
`/registrar-bitacora` la llena y los hooks no dejan hacer commit ni terminar la sesión sin ella.

## Commits

En español, cortos y con el módulo al inicio:

```
Movimientos: registrar gasto con validación de monto
Deudas: tarea en segundo plano de pagos recurrentes
```

## Pull Requests

- Siempre hacia `dev`.
- Lo revisa **otra persona del grupo** antes de hacer merge (así todos conocen el código
  de los demás para la defensa).
- Llenen la plantilla del PR, sobre todo el paradigma que evidencia y dónde está.

## Migraciones de base de datos (riesgo 5)

Solo **Jurgen** genera migraciones, para que el `ModelSnapshot` no entre en conflicto.

1. Si necesitan cambiar una entidad o su archivo en `Norte.DA/Configuraciones`, háganlo
   en su rama y avisen en el PR: "requiere migración".
2. Después del merge, Jurgen ejecuta en `dev`:
   ```bash
   dotnet ef migrations add <NombreDescriptivo> --project src/Norte.DA --startup-project src/Norte.Api
   ```
3. Todos hacen `git pull origin dev` y `dotnet ef database update ...`.

## Reglas de código

- Una interfaz por clase, en `Norte.Abstracciones`; se recibe por constructor y se registra
  en el archivo de su módulo dentro de `Norte.Api/Configuracion/Modulos/`.
  **No editen `Program.cs`** salvo acuerdo del grupo.
- Toda E/S es `async` y los métodos terminan en `Async`. Nunca `.Result` ni `.Wait()`.
- Dinero en `decimal`. Fechas sin hora en `DateOnly`; fechas con hora en UTC.
- Toda consulta de datos del usuario filtra por `UsuarioId` (lo da `IUsuarioActual`).
- Los controllers no tienen lógica ni try/catch: lanzan excepciones de
  `Modelo/Comun/ExcepcionDominio.cs` y el filtro global las traduce a 400, 404 o 409.
- La web no calcula dinero; solo presenta y pide datos al API.
- Nombres en español, descriptivos.
