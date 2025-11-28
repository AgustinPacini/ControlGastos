# ControlGastos 

Backend API para **ControlGastos**, una aplicación de control financiero personal/multiusuario que permite registrar ingresos, gastos, presupuestos, metas de ahorro, cuentas, gastos fijos y generar reportes y exportaciones.

Construido con **ASP.NET Core 8**, **Entity Framework Core** y siguiendo un enfoque de **Clean Architecture + CQRS**.

---

## 🚀 Características principales

- Autenticación y autorización con **JWT** (multiusuario).
- Gestión de:
  - ✅ Usuarios (registro, login, refresh token).
  - ✅ Categorías.
  - ✅ Gastos.
  - ✅ Ingresos.
  - ✅ Cuentas.
  - ✅ Gastos fijos.
  - ✅ Presupuestos mensuales.
  - ✅ Metas de ahorro.
- Reportes:
  - Balance mensual.
  - Balance histórico anual.
  - Top categorías de gasto.
  - Tendencias mensuales.
- Exportaciones:
  - Exportar gastos a CSV.
  - Exportar ingresos a CSV.
- Manejo centralizado de errores con middleware.
- Estructura orientada a escalabilidad y mantenibilidad.

---

## 🧱 Arquitectura

El backend sigue un enfoque de **Clean Architecture**, organizado en capas:

```text
ControlGastos.Domain        -> Entidades de dominio e interfaces
ControlGastos.Application   -> Casos de uso (CQRS, DTOs, validaciones)
ControlGastos.Infrastructure-> EF Core, DbContext, repositorios, JwtTokenService
ControlGastos.Web           -> API Web (controllers, DI, middlewares)
```

### Estructura general

- **ControlGastos.Domain**
  - `Entity/`
    - `Usuario`, `Gasto`, `Ingresos`, `Categoria`, `Presupuesto`,
      `MetaAhorro`, `Cuenta`, `GastoFijo`, `RefreshToken`, etc.
  - `Interfaces/`
    - `IBaseRepository<T>`, `IGastoRepository`, `IIngresoRepository`,  
      `ICategoriaRepository`, `IPresupuestoRepository`, `IUsuarioRepository`,  
      `IRefreshTokenRepository`, `IJwtTokenService`, etc.

- **ControlGastos.Application**
  - `Gasto_CQRS/`
  - `Ingreso_CQRS/`
  - `Categoria_CQRS/`
  - `Presupuesto_CQRS/`
  - `MetasAhorro_CQRS/`
  - `Reporte_CQRS/`
  - `Export_CQRS/`
  - `Login/`, `Usuario/`
  - Uso de **MediatR** y **FluentValidation**.

- **ControlGastos.Infrastructure**
  - `Data/ControlGastosDbContext.cs`
  - `Data/Migrations/`
  - `Repositories/`
    - `BaseRepository<T>`, `GastoRepository`, `IngresoRepository`, etc.
  - `Services/JwtTokenService.cs`

- **ControlGastos.Web**
  - `Controllers/`
  - `Middlewares/ExceptionHandlingMiddleware.cs`
  - `Extensions/ClaimsPrincipalExtensions.cs`
  - `Program.cs`

---

## 🧰 Stack tecnológico

- **Lenguaje:** C# / .NET 8
- **Web API:** ASP.NET Core
- **ORM:** Entity Framework Core
- **Base de datos:** SQL Server
- **Arquitectura:** Clean Architecture + CQRS + MediatR
- **Autenticación:** JWT Bearer
- **Validación:** FluentValidation
- **Documentación API:** Swagger / OpenAPI
- **Formato de datos:** JSON

---

## ✅ Requisitos previos

Antes de correr el proyecto, asegurate de tener:

- [.NET SDK 8.x](https://dotnet.microsoft.com/)
- SQL Server (local o remoto)
- Herramientas de EF Core (opcional pero recomendado):

```bash
dotnet tool install --global dotnet-ef
```

- Visual Studio 2022 / Rider / VS Code (a gusto).

---

## ⚙️ Configuración

### 1. Clonar el repositorio

```bash
git clone <url-del-repo>
cd <carpeta-del-repo>
```

### 2. Configurar `appsettings.json`

En el proyecto **ControlGastos.Web**, edita `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=ControlGastosDB;User Id=USUARIO;Password=CONTRASEÑA;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "clave-super-secreta-larga",
    "Issuer": "ControlGastos",
    "Audience": "ControlGastosFrontend",
    "AccessTokenMinutes": 120
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

> ⚠️ **Importante:**  
> - Usar una `Key` larga y segura.
> - No subir claves reales a repos públicos.

---

## 🗄️ Migraciones y base de datos

Desde la raíz de la solución (o apuntando al proyecto correcto):

1. Setear proyectos:

- **Startup project:** `ControlGastos.Web`
- **Default project para EF:** `ControlGastos.Infrastructure` (o el que tenga el DbContext).

2. Crear una migración (si hace falta):

```bash
dotnet ef migrations add InitialCreate -p ControlGastos.Infrastructure -s ControlGastos.Web
```

3. Actualizar la base de datos:

```bash
dotnet ef database update -p ControlGastos.Infrastructure -s ControlGastos.Web
```

---

## ▶️ Ejecutar la API

### Opción 1: Visual Studio

- Seleccionar proyecto de inicio: **ControlGastos.Web**
- Ejecutar con **F5** o **Ctrl+F5**.
- Swagger debería estar disponible en algo como:  
  `https://localhost:5001/swagger`

### Opción 2: CLI

```bash
cd ControlGastos.Web
dotnet run
```

---

## 🔐 Autenticación y multiusuario

La API usa **JWT Bearer**:

1. **Registro de usuario**

   `POST /api/usuarios/register`  
   Body ejemplo:

   ```json
   {
     "nombreUsuario": "agustin",
     "email": "agustin@example.com",
     "password": "MiPasswordSegura123"
   }
   ```

2. **Login**

   `POST /api/usuarios/login`

   Respuesta con:

   ```json
   {
     "accessToken": "jwt...",
     "refreshToken": "token-largo..."
   }
   ```

3. Usar el `accessToken` en las llamadas protegidas:

   ```http
   Authorization: Bearer <accessToken>
   ```

4. **Refresh token**

   Endpoint `POST /api/usuarios/refresh`  
   Permite obtener un nuevo access token usando el refresh token.

Todas las entidades (gastos, ingresos, presupuestos, metas, etc.) están asociadas a un `UsuarioId`.  
Los controllers suelen hacer:

```csharp
var usuarioId = User.GetUsuarioId();
```

para filtrar los datos por usuario.

---

## 📦 Módulos principales (resumen)

### Usuarios

- `POST /api/usuarios/register` – Registro.
- `POST /api/usuarios/login` – Login.
- `POST /api/usuarios/refresh` – Refresh token.

### Categorías

- `GET /api/categorias`
- `POST /api/categorias`
- `PUT /api/categorias/{id}`
- `DELETE /api/categorias/{id}`

### Gastos

- `GET /api/gasto`
- `GET /api/gasto/{id}`
- `POST /api/gasto`
- `PUT /api/gasto/{id}`
- `DELETE /api/gasto/{id}`

### Ingresos

- `GET /api/ingreso`
- `GET /api/ingreso/{id}`
- `POST /api/ingreso`
- `PUT /api/ingreso/{id}`
- `DELETE /api/ingreso/{id}`

### Cuentas y gastos fijos

- `api/cuentas/...`
- `api/gastosfijos/...`

### Presupuestos

- `GET /api/presupuestos`
- `GET /api/presupuestos/mensuales?anio=YYYY&mes=MM`
- `POST /api/presupuestos`

### Metas de ahorro

- `GET /api/metas-ahorro`
- `GET /api/metas-ahorro/{id}` (detalle y progreso)
- `POST /api/metas-ahorro`
- `PUT /api/metas-ahorro/{id}`
- `DELETE /api/metas-ahorro/{id}`

### Reportes

- `GET /api/reportes/balance-mensual?mes=MM&anio=YYYY`
- `GET /api/reportes/balance-historico?anio=YYYY`
- `GET /api/reportes/top-categorias?...`
- `GET /api/reportes/tendencias-mensuales?...`
- (Opcional) `GET /api/reportes/resumen-dashboard?...` si está implementado.

### Exportaciones

- `GET /api/exportaciones/gastos`  → CSV
- `GET /api/exportaciones/ingresos` → CSV

---

## ❗ Manejo de errores

Se utiliza un **middleware global de manejo de excepciones** (`ExceptionHandlingMiddleware`) que devuelve siempre un formato estándar:

```json
{
  "statusCode": 400,
  "messages": [
    "Mensaje de error amigable para el usuario."
  ],
  "traceId": "..."
}
```

Tipos de errores manejados:

- `ValidationException` (FluentValidation) → 400 (BadRequest)
- `DbUpdateException` (errores de base de datos, FK, UNIQUE, etc.)
- `KeyNotFoundException` → 404
- `UnauthorizedAccessException` → 401
- Otros errores no controlados → 500 (InternalServerError)

---

## 📌 Mejoras futuras

- Endpoint consolidado de **resumen de dashboard** (balance + top categorías + tendencias).
- Plan de ahorro más avanzado por meta (aporte mensual/semanal/diario).
- Integración con notificaciones/alertas (email, push, etc.).
- Tests automatizados (unitarios e integrados).

---

## 📄 Licencia

```text
Copyright (c) 2025 Agustín Pacini.
Todos los derechos reservados.
```
