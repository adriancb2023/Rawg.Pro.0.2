# Rawg.Pro.0.2

Rawg.Pro.0.2 es una aplicaci�n ERP de escritorio desarrollada en C# y WPF para la gesti�n de una tienda de videojuegos. Permite administrar el cat�logo de juegos, inventario, ventas y distribuidoras, integrando datos de la API de RAWG y gestionando la informaci�n mediante una base de datos local.

## Caracter�sticas principales
- **Gesti�n de usuarios:** Inicio de sesi�n para empleados.
- **Cat�logo de videojuegos:** Consulta, filtrado y visualizaci�n de detalles de juegos obtenidos desde la API de RAWG.
- **Inventario:** Alta, consulta y gesti�n de stock de videojuegos en la tienda.
- **Ventas:** Registro de ventas, c�lculo de importes y control de existencias.
- **Distribuidoras:** Visualizaci�n de los mejores juegos por a�o y gesti�n de proveedores.

## Base de datos
La aplicaci�n utiliza una base de datos local (mediante Entity Framework Core) que incluye las siguientes tablas principales:
- **Empleados:** Usuarios autorizados para acceder al sistema.
- **Juegos:** Informaci�n de los videojuegos gestionados en la tienda.
- **Inventarios:** Relaci�n de stock y precios de venta de los juegos.

La base de datos se crea y gestiona autom�ticamente al ejecutar la aplicaci�n por primera vez. Es necesario tener configurado el string de conexi�n en el archivo de configuraci�n.

## Requisitos
- .NET 8
- Conexi�n a vuestra base de datos SQL Server, la cual puede estar alojada de forma local o en la nube. En caso de que la ubicaci�n o las credenciales de la base de datos cambien, ser� necesario modificar la cadena de conexi�n en la configuraci�n del proyecto.

## Ejecuci�n
1. Clona este repositorio.
2. Restaura los paquetes NuGet y compila el proyecto.
3. Ejecuta la aplicaci�n.

## Notas
- El proyecto est� orientado a la gesti�n interna de una tienda f�sica o digital de videojuegos.
- Para pruebas, puedes modificar o poblar la base de datos con datos de ejemplo.

---