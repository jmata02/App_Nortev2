@echo off
rem ============================================================================
rem  Norte - configuracion del entorno de desarrollo (Windows)
rem  Uso: doble clic o, desde la raiz del repo, scripts\configurar-entorno.cmd
rem  Se puede correr varias veces: lo que ya esta listo no se vuelve a hacer.
rem ============================================================================
setlocal EnableExtensions EnableDelayedExpansion
cd /d "%~dp0.."
set "PATH=%PATH%;%USERPROFILE%\.dotnet\tools"
set "API=src\Norte.Api"
set "DA=src\Norte.DA"

echo.
echo === Norte: configuracion del entorno ===
echo Carpeta: %CD%
echo.

echo [1/8] Verificando herramientas...
call :verificar git "Git para Windows" || goto :error
call :verificar dotnet "SDK de .NET 10 (https://dotnet.microsoft.com/download)" || goto :error
call :verificar node "Node.js 26 LTS (https://nodejs.org)" || goto :error
call :verificar docker "Docker Desktop (https://www.docker.com/products/docker-desktop)" || goto :error
for /f "delims=" %%v in ('dotnet --version') do echo       .NET SDK %%v
for /f "delims=" %%v in ('node --version') do echo       Node %%v

echo [2/8] Archivo .env de la base de datos...
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
for /f "usebackq tokens=1,* delims==" %%a in (".env") do (
  if "%%a"=="POSTGRES_DB" set "NOMBRE_BD=%%b"
  if "%%a"=="POSTGRES_USER" set "USUARIO_BD=%%b"
  if "%%a"=="POSTGRES_PASSWORD" set "CLAVE_BD=%%b"
)

echo [3/8] Levantando PostgreSQL con Docker...
docker info >nul 2>&1 || (echo       Docker Desktop no esta abierto. Abralo, espere a que diga "Running" y vuelva a correr este script. & goto :error)
docker compose up -d || goto :error

echo [4/8] Secretos del API (user-secrets)...
dotnet user-secrets set "ConnectionStrings:Norte" "Host=localhost;Port=5432;Database=!NOMBRE_BD!;Username=!USUARIO_BD!;Password=!CLAVE_BD!" --project %API% >nul || goto :error
dotnet user-secrets list --project %API% | findstr /b /c:"Jwt:Clave" >nul
if errorlevel 1 (
  for /f %%i in ('powershell -NoProfile -Command "[guid]::NewGuid().ToString('N') + [guid]::NewGuid().ToString('N')"') do set "CLAVE_JWT=%%i"
  dotnet user-secrets set "Jwt:Clave" "!CLAVE_JWT!" --project %API% >nul || goto :error
  echo       Clave JWT creada.
)
echo       Cadena de conexion y clave JWT listas.

echo [5/8] Herramienta de EF Core (dotnet-ef 10)...
dotnet tool update --global dotnet-ef --version "10.*" >nul || goto :error

echo [6/8] Compilando la solucion...
dotnet build Norte.slnx -nologo -v q || goto :error

echo [7/8] Base de datos...
if not exist "%DA%\Migrations" (
  set "INTEGRANTE=sin-nombre"
  for /f "delims=" %%n in ('git config user.name') do set "INTEGRANTE=%%n"
  rem Solo Jurgen genera migraciones: se busca "Jurgen" dentro del nombre de git.
  if /i "!INTEGRANTE:Jurgen=!"=="!INTEGRANTE!" (
    echo       Todavia no existe la migracion inicial. La genera Jurgen ^(riesgo 5^).
    echo       Haga git pull de dev cuando el la suba y vuelva a correr este script.
    goto :web
  )
  echo       Generando la migracion Inicial...
  dotnet ef migrations add Inicial --project %DA% --startup-project %API% || goto :error
)
echo       Esperando a que PostgreSQL acepte conexiones...
set /a INTENTOS=0
:esperar_bd
docker exec norte-postgres pg_isready -U !USUARIO_BD! >nul 2>&1 && goto :bd_lista
set /a INTENTOS+=1
if !INTENTOS! geq 30 (echo       PostgreSQL no respondio a tiempo. & goto :error)
timeout /t 2 /nobreak >nul
goto :esperar_bd
:bd_lista
dotnet ef database update --project %DA% --startup-project %API% || goto :error

:web
echo [8/8] Dependencias de la web...
pushd web\norte-web
if not exist .env.local copy .env.example .env.local >nul
call npm install --no-fund --no-audit || (popd & goto :error)
popd

echo.
echo === Listo ===
echo  API:  dotnet run --project src\Norte.Api --launch-profile http
echo        Swagger en http://localhost:5147/swagger  (pruebe GET /api/categorias)
echo  Web:  cd web\norte-web  y  npm run dev   (http://localhost:5173)
echo.
goto :fin

:verificar
where %1 >nul 2>&1 && exit /b 0
echo       FALTA: %~2
exit /b 1

:error
echo.
echo *** La configuracion se detuvo en el paso anterior. Revise el mensaje de error. ***
endlocal
exit /b 1

:fin
endlocal
exit /b 0
