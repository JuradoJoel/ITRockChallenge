# ITRock Challenge API

API REST desarrollada con ASP.NET Core 8 para la gestión de tareas con autenticación JWT e integración con API externa.

---

# Tecnologías utilizadas

- ASP.NET Core 8
- Entity Framework Core InMemory
- JWT Authentication
- Swagger / OpenAPI

---

# Cómo ejecutar el proyecto

## 1. Clonar el repositorio

```bash
git clone https://github.com/JuradoJoel/ITRockChallenge
```

---

## 2. Abrir el proyecto

Abrir la solución en Visual Studio.

---

## 3. Ejecutar la aplicación

```bash
dotnet run
```

La API se abrirá automáticamente en Swagger.

---

# Autenticación

Primero obtener un token JWT usando:

```http
POST /auth/login
```

## Credenciales de prueba

```json
{
  "username": "admin",
  "password": "password123"
}
```

Copiar el token generado y utilizarlo en Swagger (pegar sólo el token, no agregar "bearer ..." al comienzo) o Postman (si se usa el collection, debería usar el token automáticamente una vez generado)

---

# Endpoints

## Auth

### Login

```http
POST /auth/login
```

### Ejemplo curl

```bash
curl -X POST "https://localhost:7209/auth/login" \
-H "Content-Type: application/json" \
-d "{\"username\":\"admin\",\"password\":\"password123\"}"
```

---

# Tasks

## Obtener tareas del usuario autenticado

```http
GET /tasks
```

### Ejemplo curl

```bash
curl -X GET "https://localhost:7209/tasks" \
-H "Authorization: Bearer {token}"
```

---

## Crear tarea

```http
POST /tasks
```

### Body

```json
{
  "title": "Nueva tarea",
  "description": "Descripción de la tarea"
}
```

### Ejemplo curl

```bash
curl -X POST "https://localhost:7209/tasks" \
-H "Authorization: Bearer {token}" \
-H "Content-Type: application/json" \
-d "{\"title\":\"Nueva tarea\",\"description\":\"Descripción\"}"
```

---

## Actualizar tarea

```http
PATCH /tasks/{id}
```

### Body ejemplo

```json
{
  "title": "Título actualizado",
  "description": "Descripción actualizada",
  "completed": true
}
```

### Ejemplo curl

```bash
curl -X PATCH "https://localhost:7209/tasks/{id}" \
-H "Authorization: Bearer {token}" \
-H "Content-Type: application/json" \
-d "{\"completed\":true}"
```

---

## Eliminar tarea

```http
DELETE /tasks/{id}
```

### Ejemplo curl

```bash
curl -X DELETE "https://localhost:7209/tasks/{id}" \
-H "Authorization: Bearer {token}"
```

---

## Importar tareas desde API externa

```http
POST /tasks/import
```

Este endpoint:

- Consume la API externa:

```text
https://jsonplaceholder.typicode.com/todos
```

- Filtra las primeras 5 tareas del usuario `userId = 1`
- Guarda las tareas asociadas al usuario autenticado
- Retorna la cantidad importada y el listado de tareas

### Ejemplo curl

```bash
curl -X POST "https://localhost:7209/tasks/import" \
-H "Authorization: Bearer {token}"
```

---

# Colección Postman

El proyecto incluye una colección de Postman para facilitar las pruebas manuales de los endpoints.

Importar el archivo `postman/ITRockChallenge.postman_collection.json` en Postman.

La colección:

- Obtiene automáticamente el token JWT
- Guarda automáticamente el `taskId`
- Incluye ejemplos para CRUD completo e importación

---

# Decisiones técnicas

## Entity Framework Core InMemory

Se utilizó Entity Framework Core InMemory para simplificar la configuración y mantener el foco del challenge en la lógica de negocio y los endpoints REST, considerando el tiempo estimado de resolución.

## DTOs

Se utilizaron DTOs para separar los modelos de persistencia de los contratos de entrada y salida de la API.

## JWT Authentication

La autenticación se implementó mediante JWT para mantener la API stateless y asociar las tareas al usuario autenticado.

## Service para integración externa

La integración con la API externa fue separada en un servicio dedicado utilizando HttpClient e inyección de dependencias nativa de .NET para mantener una mejor separación de responsabilidades.

## Swagger y Postman

Swagger fue utilizado para documentación y pruebas rápidas de endpoints, y se incluyó una colección Postman para facilitar la validación manual de la API.
