# Rawg.Pro.0.2

Rawg.Pro.0.2 es una aplicación ERP de escritorio desarrollada en C# y WPF para la gestión de una tienda de videojuegos. Permite administrar el catálogo de juegos, inventario, ventas y distribuidoras, integrando datos de la API de RAWG y gestionando la información mediante una base de datos local.

## Características principales
- **Gestión de usuarios:** Inicio de sesión para empleados.
- **Catálogo de videojuegos:** Consulta, filtrado y visualización de detalles de juegos obtenidos desde la API de RAWG.
- **Inventario:** Alta, consulta y gestión de stock de videojuegos en la tienda.
- **Ventas:** Registro de ventas, cálculo de importes y control de existencias.
- **Distribuidoras:** Visualización de los mejores juegos por año y gestión de proveedores.

## Base de datos
La aplicación utiliza una base de datos local (mediante Entity Framework Core) que incluye las siguientes tablas principales:
- **Empleados:** Usuarios autorizados para acceder al sistema.
- **Juegos:** Información de los videojuegos gestionados en la tienda.
- **Inventarios:** Relación de stock y precios de venta de los juegos.

La base de datos se crea y gestiona automáticamente al ejecutar la aplicación por primera vez. Es necesario tener configurado el string de conexión en el archivo de configuración.

## Requisitos
- .NET 8
- Conexión a vuestra base de datos SQL Server, la cual puede estar alojada de forma local o en la nube. En caso de que la ubicación o las credenciales de la base de datos cambien, será necesario modificar la cadena de conexión en la configuración del proyecto.

## Ejecución
1. Clona este repositorio.
2. Restaura los paquetes NuGet y compila el proyecto.
3. Ejecuta la aplicación.

## Notas
- El proyecto está orientado a la gestión interna de una tienda física o digital de videojuegos.
- Para pruebas, puedes modificar o poblar la base de datos con datos de ejemplo.

---