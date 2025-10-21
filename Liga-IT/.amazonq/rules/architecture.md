# Reglas de Arquitectura del Proyecto

## Clean Architecture
- Todos los proyectos deben seguir los principios de Clean Architecture
- Separación clara de capas: Domain, Application, Infrastructure
- Las dependencias deben fluir hacia el centro (Domain)
- Domain no debe tener dependencias externas
- Application solo depende de Domain
- Infrastructure depende de Application y Domain

## Principios SOLID
- **S**ingle Responsibility: Cada clase debe tener una única responsabilidad
- **O**pen/Closed: Abierto para extensión, cerrado para modificación
- **L**iskov Substitution: Las clases derivadas deben ser sustituibles por sus clases base
- **I**nterface Segregation: Interfaces específicas en lugar de interfaces generales
- **D**ependency Inversion: Depender de abstracciones, no de implementaciones concretas

## Estructura de Proyectos
Al crear nuevos proyectos, seguir esta estructura:
- **Domain**: Entidades, Value Objects, Interfaces de repositorios, Excepciones de dominio
- **Application**: Casos de uso, DTOs, Interfaces de servicios, Validaciones
- **Infrastructure**: Implementaciones de repositorios, Servicios externos, Configuración de BD
