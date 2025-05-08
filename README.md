# Proyecto API .NET 8 con Entity Framework - Arquitectura en 3 Capas

Este proyecto es una API construida con ASP.NET Core 8 y Entity Framework Core, estructurada en tres capas: API, Data y Models.

---

## Estructura del Proyecto

- **API**: Contiene los controladores y configuración principal (archivo `Program.cs`).
- **Data**: Contiene servicios, interfaces y el `DbContext` (solo actúa como contexto de base de datos).
- **Models**: Contiene entidades (Entities) y DTOs (Data Transfer Objects).

---

## Configuración

La configuración básica está en el archivo `appsettings.json`, incluyendo la cadena de conexión:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=TU_BD;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

---

## Paquetes NuGet Utilizados

Instalados principalmente en la capa `Data`:

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add package Swashbuckle.AspNetCore
```

---

## Cómo Ejecutar el Proyecto

Desde la terminal:

```bash
dotnet run --project Api
```

Para especificar una URL personalizada:

```bash
dotnet run --project Api --urls="http://localhost:5000"
```

Para pasar parámetros al archivo `Program.cs`:

```bash
dotnet run --project Api -- parametro1=valor1 parametro2=valor2
```

Dentro de `Program.cs`, puedes acceder a estos parámetros mediante:

```csharp
var builder = WebApplication.CreateBuilder(args);
```

---

## Despliegue del Proyecto

### Publicar el Proyecto

```bash
dotnet publish Api -c Release -o ./publicacion
```

Este comando genera los archivos en la carpeta `./publicacion` listos para subir al servidor.

### Ejecutar en Producción

```bash
cd publicacion
dotnet Api.dll
```

También puedes usar un servicio como IIS, Nginx o configurar un servicio en Linux con `systemd`.

---

## Migraciones y Base de Datos

Crear una migración:

```bash
dotnet ef migrations add NombreMigracion --project Data --startup-project Api
```

Aplicar migración:

```bash
dotnet ef database update --project Data --startup-project Api
```

---

## Swagger y Pruebas

Si está habilitado `Swashbuckle`, accede a la documentación en:

```
http://localhost:5000/swagger
```

---

## Requisitos

- .NET 7 SDK
- SQL Server
- Visual Studio 2022 o Visual Studio Code
- EF Core CLI: `dotnet tool install --global dotnet-ef`

---

## Notas Finales

- La clase `DbContext` en `Data` no contiene lógica personalizada, solo define el acceso a las entidades.
- Se recomienda inyectar servicios en `Program.cs` mediante `builder.Services.AddScoped<>()`.
- Puedes extender este proyecto hacia una arquitectura más limpia si lo necesitas.
