# Módulo de Órdenes para E-commerce — ASP.NET Core Web API

Este proyecto implementa un módulo de gestión de órdenes de compra para una tienda online. Fue desarrollado como parte de un trabajo práctico utilizando ASP.NET Core, Entity Framework Core y una base de datos SQL relacional.

---

## Objetivo

Permitir a los clientes registrar pedidos con uno o más productos, consultar el historial de órdenes, ver detalles y modificar el estado de cada orden a lo largo de su ciclo de vida (Pendiente, Enviado, Entregado, Cancelado).

---

## Cómo ejecutar el proyecto

1. Clonar el repositorio:

   ```bash
   git clone https://github.com/soto222/ecommerce-orders-api.git
   cd ecommerce-orders-api
   ```

2. Configurar la cadena de conexión en `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=ECommerceDB;Trusted_Connection=True;"
   }
   ```

3. Aplicar migraciones y crear la base de datos:

   ```bash
   dotnet ef database update
   ```

4. Ejecutar la API:

   ```bash
   dotnet run
   ```

5. Acceder a la documentación Swagger en:

   ```
   http://localhost:5098/swagger
   ```

---

## Endpoints principales

### Crear una orden

**POST** `/api/Orders`

```json
{
  "customerId": "uuid-del-cliente",
  "shippingAddress": "Dirección de envío",
  "billingAddress": "Dirección de facturación",
  "notes": "Notas opcionales",
  "orderItems": [
    {
      "productId": "uuid-del-producto",
      "quantity": 2
    }
  ]
}
```

Retorna la orden creada con ID y detalles.

---

### Obtener todas las órdenes (paginadas y filtradas)

**GET** `/api/Orders?status=Pending&customerId={id}&pageNumber=1&pageSize=10`

Parámetros disponibles:

- `status`: Pending | Shipped | Delivered | Cancelled  
- `customerId`: UUID del cliente

Devuelve lista de órdenes con productos incluidos.

---

### Obtener una orden por ID

**GET** `/api/Orders/{id}`

Devuelve todos los datos de la orden, incluyendo ítems, productos, totales, direcciones, etc.

---

### Cambiar estado de una orden

**PUT** `/api/Orders/{id}/status`

```json
{
  "newStatus": "Shipped"
}
```

Cambia el estado a: `Pending`, `Shipped`, `Delivered` o `Cancelled`.

---

## Base de datos

- Tabla `Orders`: almacena la orden general  
- Tabla `OrderItems`: ítems por orden  
- Relaciones:
  - `CustomerId` → clave foránea a `Customers`
  - `ProductId` → clave foránea a `Products`

---

## Estado de pruebas

| Funcionalidad       | Estado |
| ------------------- | ------ |
| Crear orden         | OK     |
| Obtener todas       | OK     |
| Obtener por ID      | OK     |
| Cambiar estado      | OK     |
| Filtrado por estado | OK     |

---

## Autores

Soto Juan Ignacio · Navarro Valdiviezo Angel Ezequiel · Molina Santiago Jesus  
Ingeniería en Sistemas de Información — UTN-FRT   
Trabajo Práctico — Desarrollo de Software
Comisión 3KA01 - Grupo N°9
Año: 2025 
