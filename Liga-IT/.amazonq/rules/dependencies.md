# Reglas de Dependencias

## Instalación de Paquetes NuGet
- Siempre instalar la versión más reciente de la versión mayor del framework del proyecto
- Para .NET 8: usar `--version 8.0.*` al instalar paquetes
- Para .NET 9: usar `--version 9.0.*` al instalar paquetes
- Ejemplo: `dotnet add package Microsoft.EntityFrameworkCore --version 8.0.*`

## Ubicación de Dependencias según Clean Architecture
- **Domain**: Sin dependencias externas
- **Application**: Solo dependencias de Domain
- **Infrastructure**: 
  - Entity Framework Core
  - Proveedores de base de datos
  - Librerías de servicios externos
- **API**: 
  - Entity Framework Core Tools
  - Swagger/OpenAPI
  - Librerías de presentación
