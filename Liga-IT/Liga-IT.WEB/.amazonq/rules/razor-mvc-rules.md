# Reglas de Desarrollo - Liga IT

## Stack Tecnológico
- **Framework**: ASP.NET Core MVC con Razor Pages
- **Lenguaje**: C#
- **Frontend**: HTML5, CSS3, Bootstrap 5
- **Iconos**: Font Awesome 6.4.0
- **Validación**: Data Annotations, jQuery Validation

## Estructura del Proyecto
```
Views/
├── Auth/
│   └── Index.cshtml (Login)
├── Home/
├── Shared/
│   ├── _Layout.cshtml
│   └── _Layout.cshtml.css
Controllers/
├── AuthController.cs
├── HomeController.cs
Models/
wwwroot/
├── css/
│   ├── site.css
│   └── login.css
├── js/
└── lib/
```

## Convenciones de Código

### Vistas Razor (.cshtml)
- Usar `@{}` para bloques de código C#
- Importar estilos CSS personalizados con `<link rel="stylesheet" href="~/css/archivo.css">`
- Usar Font Awesome para iconos: `<i class="fas fa-nombre"></i>`
- Aplicar clases Bootstrap para responsive design

### Controllers
- Métodos GET para mostrar vistas
- Métodos POST para procesar formularios
- Usar `ViewData` para pasar datos a vistas
- Retornar `View()` o `RedirectToAction()`

### Modelos
- Usar Data Annotations para validación
- Propiedades con getters/setters
- Nombres en PascalCase

### CSS
- Usar variables CSS para colores consistentes
- Mobile-first approach
- Gradientes azules para tema de fútbol
- Animaciones suaves

## Dependencias Requeridas
- Bootstrap 5 (en wwwroot/lib/)
- jQuery (en wwwroot/lib/)
- Font Awesome 6.4.0 (CDN)
- jQuery Validation (en wwwroot/lib/)

## Patrones de Desarrollo
- Separar lógica de presentación
- Usar partial views para componentes reutilizables
- Validación en cliente y servidor
- Manejo de errores con try-catch
