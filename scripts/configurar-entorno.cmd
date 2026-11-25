@echo off
rem ============================================================================
rem  Norte - configuracion del entorno de desarrollo (Windows)
rem  Uso: doble clic o, desde la raiz del repo, scripts\configurar-entorno.cmd
rem  Se puede correr varias veces: lo que ya esta listo no se vuelve a hacer.
rem
rem  Todo corre dentro de Docker (PostgreSQL, API, migraciones, pruebas y web).
rem  Asi Smart App Control de Windows 11 no bloquea las DLL recien compiladas y
rem  nadie tiene que desactivarlo. Solo se necesitan Git y Docker Desktop.
rem ============================================================================
setlocal EnableExtensions EnableDelayedExpansion
cd /d "%~dp0.."
set "DA=src/Norte.DA"
set "API=src/Norte.Api"

echo.
echo === Norte: configuracion del entorno (Docker) ===
echo Carpeta: %CD%
echo.

echo [1/6] Verificando herramientas...
call :verificar git "Git para Windows" || goto :error
call :verificar docker "Docker Desktop (https://www.docker.com/products/docker-desktop)" || goto :error
docker info >nul 2>&1 || (echo       Docker Desktop no esta abierto. Abralo, espere a que diga "Running" y vuelva a correr este script. & goto :error)

echo [2/6] Archivo .env (claves de la base y del JWT)...
if not exist .env (
  for /f %%i in ('powershell -NoProfile -Command "[guid]::NewGuid().ToString('N')"') do set "CLAVE_NUEVA=%%i"
  > .env (
    echo POSTGRES_DB=norte
    echo POSTGRES_USER=norte
    echo POSTGRES_PASSWORD=!CLAVE_NUEVA!
  )
  echo       .env creado con una clave nueva ^(no se sube al repositorio^).
) else (
  echo       .env ya existe, se reutiliza.
)
findstr /b /c:"JWT_CLAVE=" .env >nul
if errorlevel 1 (
  for /f %%i in ('powershell -NoProfile -Command "[guid]::NewGuid().ToString('N') + [guid]::NewGuid().ToString('N')"') do set "CLAVE_JWT=%%i"
  >> .env echo JWT_CLAVE=!CLAVE_JWT!
  echo       Clave JWT agregada a .env.
)

echo [3/6] Levantando PostgreSQL...
docker compose up -d --wait postgres || goto :error

echo [4/6] Migraciones (la primera vez descarga el SDK de .NET 10, tarda unos minutos)...
if not exist "%DA%\Migrations" (
  set "INTEGRANTE=sin-nombre"
  for /f "delims=" %%n in ('git config user.name') do set "INTEGRANTE=%%n"
  rem Solo Jurgen genera migraciones: se busca "Jurgen" dentro del nombre de git.
  if /i "!INTEGRANTE:Jurgen=!"=="!INTEGRANTE!" (
    echo       Todavia no existe la migracion inicial. La genera Jurgen ^(riesgo 5^).
    echo       Haga git pull de dev cuando el la suba y vuelva a correr este script.
    goto :pruebas
  )
  echo       Generando la migracion Inicial...
  docker compose run --rm migraciones migrations add Inicial --project %DA% --startup-project %API% || goto :error
)
docker compose run --rm migraciones || goto :error

:pruebas
echo [5/6] Corriendo las pruebas...
docker compose run --rm pruebas || goto :error

echo [6/6] Levantando el API y la web...
docker compose up -d || goto :error

echo.
echo === Listo ===
echo  API:  http://localhost:5147/swagger   (pruebe GET /api/categorias)
echo  Web:  http://localhost:5173
echo.
echo  Despues de cambiar codigo C#:   docker compose restart api
echo  Ver errores del API:            docker compose logs -f api
echo  Apagar todo:                    docker compose down
echo.
goto :fin

:verificar
where %1 >nul 2>&1 && exit /b 0
echo       FALTA: %~2
exit /b 1

:error
echo.
echo *** La configuracion se detuvo en el paso anterior. Revise el mensaje de error. ***
pause
endlocal
exit /b 1

:fin
pause
endlocal
exit /b 0
