# Propuesta aprobada: lo que el grupo se comprometió a construir

Resumen del documento entregado al profesor (`Grupo4_SC250_Avance1.pdf`, en esta misma
carpeta). **Es el contrato del proyecto**: el código debe coincidir con lo que dice aquí,
porque en la defensa se evalúa la relación entre lo documentado y lo desarrollado.
Si algo del código se aparta de este documento, hay que registrarlo como desvío en
`docs/BITACORA.md` y avisar al grupo para corregirlo en el Avance 2.

## Proyecto

**Norte: Sistema de Gestión de Finanzas Personales.** Grupo 4, III Cuatrimestre 2026.
Prof. Manuel de Jesús Montoya.
Integrantes: Jurgen Mata Vargas, Carlos Huete Carballo, Mauricio Prendas Roldán,
Jessica Marín Céspedes.

**Problema:** las personas no tienen control claro de su dinero: gastos sin rastrear, deudas y
pagos olvidados, ausencia de presupuesto y dificultad para ahorrar. Falta una herramienta
sencilla y centralizada para registrar, organizar, visualizar y planificar sus finanzas.

**Objetivo general:** aplicación web de finanzas personales con ASP.NET Core (C#) en el
back-end, React en el front-end y PostgreSQL, en una arquitectura en capas que integre cinco
paradigmas.

## Alcance

Incluye: historial de movimientos, clasificación por categorías, estado de deudas y pagos,
límites mensuales de gasto, reportes y gráficas, metas de ahorro, diseño responsivo
(computadora y celular).

**Fuera de alcance:** integración con bancos, tarjetas u otras instituciones financieras, y
ejecución de pagos o transferencias reales. No agregar nada de eso.

## Los cinco módulos

| Módulo | Qué debe hacer (según el documento) |
|--------|-------------------------------------|
| **Movimientos** | Registrar ingresos y gastos con monto, fecha, categoría, tipo y descripción. Consultar el historial y filtrar por fecha, categoría o tipo. |
| **Deudas y pagos recurrentes** | Registrar tarjetas, préstamos, mensualidades, suscripciones y otros pagos recurrentes. Ver monto total, monto pendiente, fecha de vencimiento y estado: **al día, pendiente o atrasado**. |
| **Presupuestos** | Definir un límite mensual por categoría, comparar contra lo gastado y **alertar cuando el usuario se acerque o supere** el límite. |
| **Dashboard y reportes** | Resumen con gráficas e indicadores: gastos por categoría, ingresos vs. gastos y tendencias por período. |
| **Metas de ahorro** | Objetivo con monto deseado y fecha límite, registro de aportes y progreso. |

## Paradigmas comprometidos y dónde van

| Paradigma | Capa | Compromiso del documento |
|-----------|------|--------------------------|
| Orientado a objetos | Back-end (dominio) | Entidades `Transaccion`, `Deuda`, `Presupuesto`, `MetaAhorro`, `Usuario` con **propiedades y operaciones propias**; encapsulamiento e interfaces. |
| Funcional | Back-end (lógica de negocio) | LINQ y lambdas (`Where`, `Select`, `GroupBy`, `Aggregate`) en **funciones puras** para totales, gasto por categoría y tendencias, sin modificar los datos originales. |
| Concurrente | Back-end (acceso a datos y servicios) | `async`/`await` con los métodos asíncronos de EF Core; **`BackgroundService`** para pagos recurrentes; **`Task.WhenAll`** para procesos independientes; `IDbContextFactory` con un contexto por tarea. |
| Orientado a eventos | Front-end | Manejadores de eventos de React (enviar formulario, cambiar filtro, registrar movimiento) que validan y envían al back-end. |
| Reactivo | Front-end | **Flujos de datos y observables con RxJS** (el documento dice "Rx.NET o RxJS"; el grupo eligió RxJS en el front, con Rx.NET como trabajo a futuro). Cuando se registra un gasto, el presupuesto, el dashboard y las metas se actualizan solos. Debe distinguirse del orientado a eventos. |

Riesgo 1 del documento: si el concurrente y el reactivo quedan "implícitos" (un `useState` o un
`async` aislado), el profesor puede considerar que no hay evidencia. Hay que hacerlos
**visibles y explicables**.

## Stack y versiones

| Pieza | Versión / detalle |
|-------|-------------------|
| Plataforma | .NET 10 LTS (soporte hasta noviembre de 2028), C#, Visual Studio 2026 |
| API | ASP.NET Core 10 Web API (plantilla con controladores), OpenAPI + Swagger UI solo en desarrollo |
| Datos | EF Core 10 + `Npgsql.EntityFrameworkCore.PostgreSQL`, `Microsoft.EntityFrameworkCore.Tools` para migraciones |
| Base de datos | PostgreSQL (Docker Compose en desarrollo), montos en `numeric(18,2)` |
| Front-end | React 19 + Vite (JavaScript), React Router; Node.js 26 LTS |
| Seguridad | Autenticación con ASP.NET Core Identity (hash de contraseñas) + JWT, HTTPS, filtrado por usuario, secretos en user-secrets |
| Código | Git + GitHub, una rama por integrante o funcionalidad, pull requests revisados |

## Arquitectura

Cliente-servidor por capas. React corre en el navegador; la API REST en ASP.NET Core concentra
la lógica; solo la API accede a PostgreSQL mediante EF Core y Npgsql.

Flujo: 1) el navegador carga el build de React; 2) cada acción envía una petición HTTPS con JSON
a la API; 3) la API aplica las reglas de negocio y consulta o guarda en PostgreSQL de forma
asíncrona; 4) la API responde y React actualiza su estado: dashboard, gráficas y barras se
redibujan solos.

Despliegue previsto: un solo servidor que ejecuta la API y entrega el build de React
(pendiente de definir antes del Avance 2, riesgo 10; respaldo: ejecución local).

## Riesgos y mitigaciones que el código debe respetar

| N.º | Riesgo | Mitigación obligatoria |
|-----|--------|------------------------|
| 1 | Paradigmas ocultos por el framework | Cada paradigma en un módulo concreto y visible; documentar un fragmento por paradigma desde el inicio. |
| 2 | Alcance excesivo | Producto mínimo por módulo: **Movimientos y Presupuestos primero**; dashboard avanzado y metas como segunda prioridad. |
| 3 | Errores con dinero | `decimal` en C#, `numeric(18,2)` en PostgreSQL; cálculos en la lógica de negocio con pruebas de redondeo. |
| 4 | Fechas y zonas horarias | Guardar en UTC, `DateOnly` para vencimientos, convertir a hora local solo en la interfaz. |
| 5 | Versiones y migraciones | Versiones fijadas (`global.json`, `.nvmrc`); **un solo integrante (Jurgen) genera migraciones**. |
| 6 | Condiciones de carrera | Transacciones y concurrencia optimista con `xmin`; probar solicitudes simultáneas. |
| 7 | Seguridad | Identity + JWT, hash de contraseñas, **toda consulta filtrada por usuario**, HTTPS, secretos fuera del repo. |
| 8 | Integración front/back | CORS desde el inicio, endpoints documentados en Swagger, **contratos (DTO) acordados antes** de implementar cada módulo. |
| 9 | Entornos distintos | Versiones estandarizadas, PostgreSQL en Docker Compose, instalación en el README. |
| 10 | Despliegue sin definir | Decidir antes del Avance 2; garantizar ejecución local para la demostración. |
| 11 | Trabajo desigual | Responsabilidades por módulo, ramas por funcionalidad, PR revisados, commits frecuentes. |
| 12 | Ajustes de la propuesta | Margen en el cronograma para cambios pedidos por el profesor. |

## Cronograma comprometido

| Semana | Actividades | Paradigma / evidencia | Hito |
|--------|-------------|-----------------------|------|
| 8 | Avance 1, repositorio, versiones, reparto | Paradigma por módulo | Avance 1 (PDF) |
| 9 | Modelo de datos y migraciones, autenticación, Movimientos (API y pantallas), CORS y Swagger | Orientado a objetos | API conectada con React |
| 10 | Presupuestos y Deudas (básico), reportes simples por categoría y mes, primeras capturas | Eventos; funcional (LINQ) | Versión parcial funcional |
| 11 | Corrección de errores, documento actualizado, ZIP | Al menos dos paradigmas funcionando con fragmentos documentados | **Avance 2 (ZIP)** |
| 12 | Dashboard, metas, pagos recurrentes en segundo plano, concurrencia en saldos | Concurrente (BackgroundService); reactivo (dashboard) | Todos los módulos completos |
| 13 | Pruebas (UTC, redondeo, simultáneas), despliegue o ejecución local, documento final, ensayo | Fragmento por paradigma listo | Proyecto cerrado |
| 14 | Entrega y defensa | Explicación de los cinco paradigmas | Entrega final |

## Correcciones pendientes en el documento (para el Avance 2)

Detectadas al revisar el PDF; no cambian el código, pero conviene corregirlas:
".NET y .NET 9" debe decir ".NET 8 y .NET 9" · "servidor Kernel" es "Kestrel" · citas
"(Microsoft, 2026)" sin letra cuando las referencias usan 2026a–f · "Javanovic" vs "Jovanović" ·
URLs de React y LINQ en otros idiomas (`vi.react.dev`, `eu-es`) · riesgo 1 dice "Rx.NET o RxJS"
(ya se eligió RxJS) · riesgo 8 con impacto Baja (debería ser Media) · varios errores de tipeo.
