# División del trabajo

Cada persona es dueña de **módulos completos** (backend + frontend + pruebas) y de **un
paradigma** que explica en la defensa. Así el repositorio muestra la participación de todos
y cada integrante tiene una parte técnica propia, como pide el enunciado.

| | Jurgen | Jessica | Carlos | Mauricio |
|---|---|---|---|---|
| **Módulos** | Autenticación, Dashboard y reportes | Movimientos, Metas de ahorro | Deudas y pagos recurrentes | Presupuestos |
| **Paradigma que defiende** | Funcional | Orientado a objetos y orientado a eventos | Concurrente | Reactivo |
| **Además** | Coordinación técnica, migraciones, repositorio | Diseño de formularios de la web | Pruebas de concurrencia | Capa reactiva compartida de la web |

Ya está hecho en el esqueleto: la estructura de capas, el modelo de datos de los cinco
módulos, el módulo de referencia *Categorías*, la configuración del API (JWT, CORS,
Swagger, manejo de errores) y la base de la web (rutas, menú, estilos, cliente HTTP).

---

## Jurgen Mata Vargas: Autenticación, Dashboard y reportes, coordinación

**Paradigma: funcional.** Los reportes transforman colecciones de movimientos en resúmenes
sin modificar los datos originales.

Backend
- [x] Generar la primera migración (`Inicial`) y verificar que todos puedan crear la BD.
- [x] Autenticación: `IUsuarioDA`, `AutenticacionFlujo` (registro e inicio de sesión), hash de
  contraseña con `PasswordHasher<Usuario>`, `IGeneradorToken` (JWT con el claim `sub` = id
  del usuario) y `AutenticacionController`:
  `POST /api/autenticacion/registro`, `POST /api/autenticacion/inicio-sesion`.
- [x] Reportes: reglas **puras** en `Norte.Reglas` (por ejemplo `ReglaResumenFinanciero`)
  con `Where`, `Select`, `GroupBy` y `Aggregate`: total de ingresos y gastos, gasto por
  categoría y tendencia de los últimos meses.
- [x] `IReporteDA` + `ReportesController`: `GET /api/reportes/resumen`,
  `/gasto-por-categoria` y `/tendencia`. Para cargar varias piezas a la vez usar
  `IDbContextFactory<NorteDbContext>` (un contexto por tarea) con `Task.WhenAll`.

Frontend
- [x] Páginas de inicio de sesión y registro (guardar el token con `guardarToken`).
- [x] Proteger las rutas cuando no hay sesión.
- [x] Dashboard con gráficas (Chart.js) que se actualiza al suscribirse a
  `cambiosFinancieros$` (coordinar con Mauricio).

Coordinación
- [ ] Proteger `main` y `dev` en GitHub (PR obligatorio) y revisar merges a `dev`.
- [x] Decidir dónde se despliega o cómo se corre local para la demostración (riesgo 10): local con Docker, ver README.

Defensa: arquitectura general, flujo de una solicitud por las capas, paradigma funcional.

---

## Jessica Marín Céspedes: Movimientos y Metas de ahorro

**Paradigmas: orientado a objetos y orientado a eventos.** Las entidades encapsulan sus
propias reglas, y la web reacciona a las acciones del usuario con manejadores de eventos.

Backend
- [ ] Darle comportamiento a `Transaccion` (por ejemplo un método de fábrica que valide
  monto, tipo y fecha) en vez de dejarla solo con propiedades.
- [ ] Movimientos: `ITransaccionDA`, `TransaccionFlujo` y `MovimientosController`
  (crear, editar, eliminar, listar con filtros por fecha, categoría y tipo). Validar que la
  categoría coincida con el tipo de movimiento.
- [ ] `MetaAhorro` con comportamiento: `RegistrarAporte(monto)`, `MontoAhorrado`,
  `PorcentajeAvance` y `EstaCompletada`.
- [ ] Metas: `IMetaAhorroDA`, `MetaAhorroFlujo` y `MetasController` (crear, listar, aportar).

Frontend
- [ ] Movimientos: formulario con `onSubmit` y `onChange`, validación antes de enviar,
  lista con filtros. Después de guardar, llamar a `publicarCambio(...)` para que el resto de
  la app se entere.
- [ ] Metas: tarjetas con barra de avance y formulario de aporte.

Defensa: clases, encapsulamiento e interfaces (POO); manejadores de eventos en React.

---

## Carlos Huete Carballo: Deudas y pagos recurrentes

**Paradigma: concurrente.** Tareas en segundo plano, operaciones asíncronas y protección
contra modificaciones simultáneas.

Backend
- [ ] `Deuda` con comportamiento: `RegistrarPago(monto)` y `CalcularEstado(hoy)` que
  devuelva `AlDia`, `Pendiente` o `Atrasado`.
- [ ] `IDeudaDA`, `DeudaFlujo` y `DeudasController` (crear, listar con estado, registrar
  pago). El pago y la actualización del saldo van en una sola transacción.
- [ ] Concurrencia optimista: `Version` ya está mapeada a `xmin`. Si dos pagos llegan a la
  vez, uno recibe 409 (el filtro ya traduce `DbUpdateConcurrencyException`).
- [ ] `GeneradorPagosRecurrentesTarea` (`BackgroundService` + `PeriodicTimer`) y
  `ProcesadorPagosRecurrentes`, en `Norte.Servicios` (ver su README).
- [ ] Prueba que envíe solicitudes simultáneas con `Task.WhenAll` y verifique el 409.

Frontend
- [ ] Página de deudas con el estado de cada una (al día, pendiente, atrasada) y el
  formulario de pago.

Defensa: `async`/`await`, tareas en segundo plano, `Task.WhenAll`, concurrencia optimista.

---

## Mauricio Prendas Roldán: Presupuestos y capa reactiva

**Paradigma: reactivo.** Los cambios en los datos se propagan solos a las partes de la
interfaz que dependen de ellos.

Backend
- [ ] `Presupuesto` con comportamiento: `PorcentajeUsado(gastado)` y `EstaExcedido(gastado)`.
- [ ] `IPresupuestoDA`, `PresupuestoFlujo` y `PresupuestosController` (definir el límite de
  una categoría para un mes, listar los del mes con lo gastado y las alertas).

Frontend
- [ ] Completar `src/reactivo/almacenFinanciero.js` con RxJS: flujos derivados
  (`map`, `scan`, `filter`, `debounceTime`) y un flujo de alertas de presupuesto.
- [ ] Página de presupuestos con barras de progreso que cambian solas cuando Movimientos
  publica un gasto (sin recargar la página).
- [ ] Ayudar a Jurgen y Jessica a suscribir Dashboard y Metas al flujo.

Defensa: paradigma reactivo y su diferencia con el orientado a eventos (el evento inicia
la acción, el flujo reactivo propaga las consecuencias).

---

## Todos

- [ ] Pruebas unitarias de sus reglas y flujos en `tests/Norte.Pruebas.Unitarias`.
- [ ] Llenar su fila en `docs/EVIDENCIA_PARADIGMAS.md` con archivo, método y fragmento.
- [ ] Capturas o video corto de su módulo para el Avance 2.
- [ ] Revisar al menos un PR de otra persona por semana.

## Plan por semana

Basado en el cronograma del Avance 1.

| Semana | Jurgen | Jessica | Carlos | Mauricio |
|--------|--------|---------|--------|----------|
| 9 | Migración inicial, autenticación (backend) | Movimientos (backend) | `Deuda` y su DA | Presupuestos (backend) y prueba de concepto con RxJS |
| 10 | Login en la web, reportes simples | Movimientos (web) | Deudas (backend y web), pagos | Presupuestos (web) conectado al flujo reactivo |
| 11 (**Avance 2**) | Integrar en `dev` y preparar el ZIP | Capturas | Capturas | Capturas |
| 12 | Dashboard con gráficas | Metas de ahorro | Pagos recurrentes en segundo plano, prueba de concurrencia | Alertas reactivas en Dashboard y Metas |
| 13 | Manual de instalación, presentación | Evidencia POO y eventos | Evidencia concurrente | Evidencia reactiva |
| 14 | Defensa | Defensa | Defensa | Defensa |

**Para el Avance 2** se exigen al menos dos paradigmas funcionando con código ejecutable.
Lo más seguro: **orientado a objetos** (Movimientos) y **funcional** (reportes simples), con la
prueba de concepto **reactiva** de Presupuestos como tercero. El reactivo es el paradigma
con más riesgo de quedar "implícito", por eso conviene adelantarlo y no dejarlo para la
semana 12.
