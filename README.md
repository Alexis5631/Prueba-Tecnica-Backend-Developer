# Proyectos de Desarrollo

Este repositorio contiene dos proyectos de desarrollo diferentes: uno en **.NET** con arquitectura Clean Architecture para un sistema de autotaller con autenticación JWT y otro en **PHP** para gestión de notas escolares.

## 📁 Estructura del Repositorio

```
├── dotnet-dapper-jwt/          # Proyecto .NET con Clean Architecture
├── php-notas/                  # Proyecto PHP para sistema de notas
└── README.md                   # Este archivo
```

---

## 🔧 Proyecto .NET - Sistema de Autotaller con JWT

### 📋 Descripción
API REST desarrollada en .NET 8 con arquitectura Clean Architecture que implementa un sistema de autotaller con autenticación JWT, gestión de usuarios, roles, productos, órdenes y refresh tokens.

### 🛠️ Tecnologías
- **.NET 8**
- **Entity Framework Core**
- **PostgreSQL** (configurado)
- **JWT Authentication**
- **Dapper** (para consultas optimizadas)
- **AutoMapper**
- **Swagger/OpenAPI**
- **Rate Limiting**

### 📂 Arquitectura Clean Architecture
```
dotnet-dapper-jwt/
├── ApiPrueba/                  # Capa de Presentación (API)
│   ├── Controllers/            # Controladores REST
│   ├── Extensions/             # Configuraciones de servicios
│   ├── Helpers/                # JWT, autorización, manejo de errores
│   └── Program.cs              # Punto de entrada
├── Application/                # Capa de Aplicación
│   └── Interfaces/             # Contratos de repositorios
├── Domain/                     # Capa de Dominio
│   └── Entities/               # Entidades del negocio
├── Infrastructure/             # Capa de Infraestructura
│   ├── Data/                   # DbContext
│   ├── Repositories/           # Implementaciones de repositorios
│   ├── UnitOfWork/             # Patrón Unit of Work
│   └── Configuration/          # Configuraciones de EF
└── db/                         # Scripts de base de datos
    ├── ddl.sql                 # Estructura de tablas
    └── dml.sql                 # Datos de prueba
```

### 🗄️ Base de Datos
- **Motor**: PostgreSQL
- **Tablas principales**:
  - `Users` - Usuarios del sistema
  - `Roles` - Roles de usuario
  - `Products` - Catálogo de productos
  - `Orders` - Órdenes de compra
  - `OrderItems` - Items de las órdenes
  - `RefreshTokens` - Tokens de renovación

### 🔧 Configuración y Ejecución

#### Requisitos Previos
- .NET 8 SDK
- PostgreSQL
- Visual Studio 2022 o VS Code

#### Pasos para Ejecutar

1. **Restaurar dependencias**:
   ```bash
   cd dotnet-dapper-jwt
   dotnet restore
   ```

2. **Configurar base de datos**:
   - Instalar PostgreSQL
   - Crear base de datos
   - Ejecutar scripts en `db/ddl.sql` y `db/dml.sql`

3. **Configurar conexión**:
   Editar `appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=PruebaPI;Username=tu_usuario;Password=tu_contraseña"
     }
   }
   ```

4. **Ejecutar migraciones**:
   ```bash
   dotnet ef database update -p .\\Infrastructure\\ -s .\\ApiPrueba\\
   ```

5. **Ejecutar la aplicación**:
   ```bash
   cd ApiPrueba
   dotnet run
   ```

6. **Acceder a la API**:
   - API: `https://localhost:7001` (HTTPS)
   - Swagger: `https://localhost:7001/swagger`

### 🔐 Autenticación JWT

#### Endpoints de Autenticación
- `POST /auth/login` - Iniciar sesión
- `POST /auth/register` - Registrar usuario
- `POST /auth/refresh-token` - Renovar token
- `POST /auth/logout` - Cerrar sesión

#### Uso de Tokens
```bash
# Login
curl -X POST https://localhost:7001/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email": "usuario@email.com", "password": "contraseña"}'

# Usar token en requests
curl -H "Authorization: Bearer tu_token_aqui" \
  https://localhost:7001/api/products
```

### 📡 Endpoints Principales

#### Autenticación
- `POST /auth/login` - Autenticación de usuario
- `POST /auth/register` - Registro de nuevo usuario
- `POST /auth/refresh-token` - Renovar token de acceso

#### Usuarios
- `GET /api/users` - Listar usuarios (requiere autenticación)
- `GET /api/users/{id}` - Obtener usuario por ID

#### Productos
- `GET /api/products` - Listar productos
- `POST /api/products` - Crear producto (requiere autenticación)
- `PUT /api/products/{id}` - Actualizar producto
- `DELETE /api/products/{id}` - Eliminar producto

#### Órdenes
- `GET /api/orders` - Listar órdenes del usuario
- `POST /api/orders` - Crear nueva orden
- `GET /api/orders/{id}` - Obtener orden por ID

#### Roles
- `GET /api/roles` - Listar roles disponibles

### 🛡️ Características de Seguridad

- **JWT Authentication** - Tokens seguros para autenticación
- **Refresh Tokens** - Renovación automática de tokens
- **Role-based Authorization** - Autorización basada en roles
- **Rate Limiting** - Límite de requests por minuto
- **CORS** configurado para desarrollo
- **HTTPS** habilitado en producción

### 🔧 Configuraciones Avanzadas

#### Rate Limiting
```csharp
// Configurado para 100 requests por minuto
builder.Services.AddCustomRateLimiter();
```

#### CORS
```csharp
// Configurado para permitir requests desde frontend
builder.Services.ConfigureCors();
```

---