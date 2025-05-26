# Liondor

 API de Productos - ASP.NET Core
Esta es una API RESTful desarrollada con **ASP.NET Core** para la gestion de productos, que incluye autenticacion con JWT, uso de **Entity Framework Core**, 
documentacion con **Swagger**, y operaciones **CRUD** completas sobre una entidad de productos.
---
 Caracteristicas
 
- Login con autenticacion JWT solo de prueba usuario:admin password:1234 solo es desmostrativo para generrar un token de seguridad en los endpoints
- Operaciones CRUD sobre productos (`GET`, `POST`, `PUT`, `DELETE`)
- Uso de Entity Framework Core con LINQ
- Swagger integrado para documentacion y pruebas
- Autorizacion mediante [Authorize]

Estructura del proyecto
Productos/
----- Controllers/
	|------ ProductosController.cs
----- Context/
	|------ dbContext.cs
----- Models/
	|------ Producto.cs
	|------	InsertarProducto.cs
----- appsettings.json
----- Program.cs

---

  Requisitos

- [.NET 7 SDK o superior](https://dotnet.microsoft.com/download)
- SQL Server u otro proveedor compatible con EF Core
- Visual Studio o VS Code


Instalacion
1.- Creacion de base de datos Tabla Productos en Sql Server 

CREATE TABLE [dbo].[Productos](
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Nombre] NVARCHAR(100) NOT NULL,
	[Activo] BIT NOT NULL DEFAULT 1, -- Usar BIT para valores booleanos
	[Precio] DECIMAL(10, 2) NOT NULL,
	[Stock] INT NOT NULL,
	[FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE()
);
 --------------------------| Mapear la base de datos en sql server
PREVIAMENTE INSTALANDO :

          dotnet tool install --global dotnet-ef
          dotnet add package Microsoft.EntityFrameworkCore.Tools
          dotnet add package Microsoft.EntityFrameworkCore.Design
          dotnet add package Microsoft.EntityFrameworkCore.SqlServer

          Install-Package BCrypt.Net-Next
          dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.1 (asegurate que tengas la version correcta)
          Install-Package Swashbuckle.AspNetCore
          dotnet add package Swashbuckle.AspNetCore.Annotations


--->Ejecuta en tu consola en menu "Herramientas" -> en el item "Administracion de Paquetes Nuguet" -> "Consola de administrador de paquetes"
     
      Scaffold-DbContext "Server=DANIEL_FUENTES\SQLEXPRESS;Database=GestionProductos;User Id=user;Password=Pekit4s2022;Encrypt=False" 
      Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -forcé




---------------------------------------------------------------------------------------------------------------------------
2. **Configura la cadena de conexión** en `appsettings.json`:
{
    "ConnectionStrings": {
        "ConexionPredeterminada": "Server=DANIEL_FUENTES\\SQLEXPRESS;Database=GestionProductos;User Id=user;Password=Pekit4s2022;Encrypt=False"
    },
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "JWT_Configuracion": {
        "LLaveSecreta": "Q2h6Rn5vYk4!@x0npz9$Jt5HgL1wM8g1YQ3E&bK6k2W#CmXrP6tV7z5eZ1QbUfO"
    }
}
----------------------------------------------------------------------------------------------------------------------------------------------
3. **Agrega la llave secreta del JWT** en `appsettings.json`:
    "JWT_Configuracion": {
      "LLaveSecreta": "Q2h6Rn5vYk4!@x0npz9$Jt5HgL1wM8g1YQ3E&bK6k2W#CmXrP6tV7z5eZ1QbUfO"
    }


4. **Aplica las migraciones de Entity Framework**:

dotnet ef migrations add InitialCreate
dotnet ef database update


> Asegurate de tener instalada la herramienta de EF CLI:
> dotnet tool install --global dotnet-ef

5. **Ejecuta el proyecto**:
dotnet run


Este token se debe incluir en las peticiones protegidas usando el encabezado:
Todos los endpoints (excepto `/login`) requieren autorizacion JWT.

| metodo | Ruta                     |           Descripcion          |
|--------|--------------------------|---------------------------------|
| GET    | /productos               | Listar todos los productos      |
| GET    | /productos/{id}          | Buscar producto por ID          |
| POST   | /productos               | Agregar un nuevo producto       |
| PUT    | /productos/{id}          | Actualizar un producto          |
| DELETE | /productos/{id}          | Eliminar un producto            |
---------------------------------------------------------------------------------------------------------
csharp
public class Producto
{
  public int Id { get; set; }
  public string Nombre { get; set; }
  public decimal Precio { get; set; }
  public bool Activo { get; set; }
  public int Stock { get; set; }
}
-------------------------------------------------------------------------------------------------------

Pruebas
Puedes probar los endpoints directamente desde Swagger UI o usar herramientas como Postman.


* Se utiliza LINQ y EF Core para el acceso a datos.
* El metodo `CreatedAtAction` en POST retorna el recurso creado con su ubicacion.
