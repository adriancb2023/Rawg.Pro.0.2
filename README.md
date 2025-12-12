# 🎮 Rawg.Pro - Game Store Manager

<div align="center">

![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![WPF](https://img.shields.io/badge/WPF-512BD4?style=for-the-badge&logo=windows&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-005C84?style=for-the-badge&logo=mysql&logoColor=white)
![RAWG API](https://img.shields.io/badge/RAWG_API-000000?style=for-the-badge&logo=json&logoColor=white)

**Una solución de escritorio moderna y robusta para la gestión integral de tiendas de videojuegos, conectada en tiempo real con la base de datos de RAWG.**

[Características](#-características-principales) •
[Instalación](#-instalación) •
[Uso](#-uso) •
[Tecnologías](#-tecnologías)

</div>

---

## 📖 Descripción

**Rawg.Pro** es una aplicación de escritorio desarrollada en **WPF (Windows Presentation Foundation)** diseñada para optimizar el flujo de trabajo en tiendas de videojuegos. Permite a los empleados gestionar el inventario, realizar ventas y consultar información detallada de miles de juegos gracias a su integración con la **API de RAWG**.

Su interfaz moderna y fluida ofrece una experiencia de usuario premium, facilitando desde la búsqueda de títulos hasta el control de stock y ventas diarias.

## ✨ Características Principales

### 🔌 Integración API RAWG
- **Búsqueda Global**: Acceso a la base de datos masiva de videojuegos de RAWG.
- **Detalles Completos**: Visualiza calificación, fecha de lanzamiento, desarrolladores, género y capturas de pantalla de alta calidad.
- **Novedades**: Sección dedicada a los "Próximos Lanzamientos" y "Lo Mejor del Año".
- **Filtros Avanzados**: Búsqueda por nombre, año o desarrolladora.

### 📦 Gestión de Inventario
- **Catálogo Local**: Añade juegos desde la API directamente a tu base de datos local.
- **Control de Stock**: Gestiona la cantidad de copias disponibles y actualiza precios de venta.
- **Inventario Personalizado**: Vista dedicada para auditar el stock actual de la tienda.

### 💰 Punto de Venta (POS)
- **Sistema de Ventas**: Interfaz para procesar compras de clientes.
- **Cálculo Automático**: Totalización de importes basada en el precio y cantidad seleccionada.
- **Histórico (WIP)**: Registro de transacciones realizadas por cada empleado.

### 🔐 Seguridad y Acceso
- **Autenticación**: Sistema de Login seguro para empleados.
- **Roles**: Distinción entre diferentes usuarios (Vendedor, Manager, etc.).

## 🛠 Instalación y Configuración

### Prerrequisitos
- **Visual Studio 2022** (o compatible) con la carga de trabajo de desarrollo de escritorio .NET.
- **MySQL Server** (u otro servidor compatible como MariaDB).
- **Nombre de base de datos**: `rawg2`.

### Pasos

1.  **Clonar el repositorio**
    ```bash
    git clone https://github.com/tu-usuario/Rawg.Pro.0.2.git
    ```

2.  **Configurar la Base de Datos**
    - Abre tu gestor de base de datos favorito (ej. HeidiSQL, MySQL Workbench).
    - Ejecuta el script `BBDD.sql` incluido en la raíz del proyecto para crear la base de datos `rawg2` y las tablas necesarias.
    - *Nota: El script incluye datos de prueba iniciales.*

3.  **Configurar Conexión**
    - La aplicación está configurada por defecto para usar credenciales locales.
    - Asegúrate de que tu servidor MySQL esté corriendo en el puerto `3306`.
    - Credenciales por defecto en `Rawg2Context.cs`:
        - User: `root`
        - Pass: `root`
        - *Si tus credenciales son diferentes, actualiza la cadena de conexión en `Modelo/Rawg2Context.cs`.*

4.  **API Key de RAWG**
    - El proyecto incluye una clave de demostración en `RawgApiClient.cs`.
    - Para producción, obtén tu propia clave gratuita en [rawg.io/apidocs](https://rawg.io/apidocs) y actualiza la variable `apiKey`.

5.  **Compilar y Ejecutar**
    - Abre la solución `Rawg.Pro.0.2.sln` en Visual Studio.
    - Restaura los paquetes NuGet.
    - Presiona `F5` para iniciar.

## 🚀 Uso

### Credenciales de Acceso (Demo)

Puedes utilizar las siguientes cuentas preconfiguradas para acceder al sistema:

| Usuario | Contraseña | Rol |
| :--- | :--- | :--- |
| `root` | `root` | Super Admin |
| `maria` | `maria123` | Manager |
| `carlos` | `carlos123` | Developer |
| `ana` | `ana123` | HR |

## 💻 Tecnologías

Este proyecto ha sido construido utilizando estándares modernos de desarrollo .NET:

- **Frontend**: WPF (XAML) con un diseño UI limpio y responsivo.
- **Backend**: C# .NET.
- **ORM**: Entity Framework Core con proveedor MySQL (Pomelo).
- **Datos**: JSON (Newtonsoft) para serialización de respuestas API.
- **Base de Datos**: MySQL / MariaDB.

---

<p align="center">
  <sub>Desarrollado con ❤️ para los amantes de los videojuegos.</sub>
</p>