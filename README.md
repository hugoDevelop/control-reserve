# Sistema de Gestión de Reservas

Este proyecto es un sistema de gestión de reservas para espacios compartidos. Permite a los usuarios realizar reservas de espacios, garantizando que no existan conflictos de horarios. Los usuarios pueden gestionar sus reservas (crearlas, cancelarlas y visualizarlas) a través de una API RESTful y una interfaz web moderna.

## Características

- **Crear reserva**: Validar que no existan solapamientos con reservas ya existentes en el mismo espacio.
- **Cancelar reserva**: Permitir la eliminación de reservas existentes.
- **Listar reservas**: Mostrar todas las reservas con opciones de filtro por espacio, usuario o rango de fechas.
- **Validaciones**:
  - Un usuario no puede reservar dos espacios al mismo tiempo.
  - Las reservas deben tener un tiempo mínimo y máximo permitido.

## Tecnologías Utilizadas

- **Backend**:

  - .NET 8
  - C#
  - Entity Framework Core
  - PostgreSQL (puede ser reemplazado por SQL Server, MySQL o SQLite)
  - Arquitectura Hexagonal

- **Frontend**:

  - Angular
  - TypeScript

- **Pruebas Unitarias**:
  - xUnit
  - Moq

## Endpoints de la API

### Reservas

- **POST /reservations**: Crear una nueva reserva.
- **DELETE /reservations/{id}**: Cancelar una reserva.
- **GET /reservations**: Listar reservas con filtros opcionales (spaceId, userId, startDate, endDate).

### Espacios

- **POST /spaces**: Crear un nuevo espacio.
- **DELETE /spaces/{id}**: Eliminar un espacio.
- **GET /spaces**: Listar todos los espacios.
- **PUT /spaces**: Actualizar un espacio.

### Usuarios

- **POST /users**: Crear un nuevo usuario.
- **DELETE /users/{id}**: Eliminar un usuario.
- **GET /users**: Listar todos los usuarios.
- **PUT /users**: Actualizar un usuario.

## Instalación y Configuración

### Backend

1. Navegar al repositorio:

```sh
cd control-reserve-back-end
```

2. Configura la cadena de conexión a la base de datos en `appsettings.json`:

```
{
    "ConnectionStrings": 
    { 
        "DefaultConnection": "Host=localhost;Database=control_reserve;Username=tu-usuario;Password=tu-contraseña" 
    }
}
```

3. Aplica las migraciones de la base de datos:

```sh
dotnet ef database update
```

4. Ejecuta el proyecto:

```sh
dotnet run
```

### Frontend

1. Navegar al repositorio:

```sh
cd control-reserve-front-end
```

2. Instala las dependencias:

```sh
npm install
```

3. Ejecuta la aplicación:

```sh
ng serve
```

## Pruebas Unitarias

### Backend

1. Navega al directorio de pruebas:

```sh
cd control-reserve-back-end.test
```

2. Ejecuta las pruebas:

```sh
dotnet test
```

### Frontend

1. Ejecuta las pruebas:

```sh
ng test
```

## Documentación

### Backend

- La documentación de los endpoints de la API está disponible en Swagger. Para acceder a ella, ejecuta el backend y navega a http://localhost:5132/swagger.

### Frontend
- Instrucciones claras sobre cómo levantar y probar la aplicación están incluidas en la sección de Instalación y Configuración.

## Decisiones Arquitectónicas

### Backend

- Se utilizó la Arquitectura Hexagonal para mantener una separación clara entre la lógica de negocio y las dependencias externas.
- Entity Framework Core se utilizó para la persistencia de datos debido a su integración con .NET y su capacidad para trabajar con múltiples bases de datos relacionales.

### Frontend
- Angular se eligió por su capacidad para crear aplicaciones modulares y su fuerte soporte para TypeScript.
- Se implementaron componentes reutilizables para el formulario y la tabla de reservas.

## Contribuciones

Las contribuciones son bienvenidas.