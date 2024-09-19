# Aplicación web csf-web-hce-hospital

## Descripción

Esta es una aplicación web desarrollada en **ASP.NET Core** que proporciona funcionalidades de historia clínica electrónica para hospital. La aplicación sigue una arquitectura de MVC (Modelo-Vista-Controlador) y utiliza una base de datos SQL Server.

## Requisitos Previos

Antes de ejecutar la aplicación, asegúrate de tener instalados los siguientes componentes:

- [.NET SDK 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)

## Instalación

Sigue los siguientes pasos para configurar y ejecutar la aplicación en tu entorno local:

1. **Clonar el repositorio**:
   ```bash
   git clone https://github.com/ClinicaSanFelipe/csf-web-hce-hospital.git
   cd csf-app-base
   ```

2. **Configurar la base de datos**:
   - Asegúrate de tener SQL Server en ejecución.
   - Modifica el archivo `appsettings.json` en la sección `ConnectionStrings` para agregar los detalles de tu servidor de base de datos:

   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=192.168.42.151\\INST01;Database=clinica;User Id=usuario;Password=contraseña;"
   }
   ```

3. **Restaurar paquetes NuGet**:
   Ejecuta el siguiente comando para restaurar los paquetes NuGet necesarios:
   ```bash
   dotnet restore
   ```

4. **Compilar y ejecutar la aplicación**:
   ```bash
   dotnet run
   ```

   La aplicación estará disponible en `https://localhost:5001`.

## Características

- Autenticación de usuarios.
- HCE de pacientes.
- Reportes.

## Uso

1. Accede a la aplicación en `https://localhost:5001`.
2. Navega por las diferentes secciones del sitio web y utiliza las funcionalidades proporcionadas.

## Tecnologías Utilizadas

- **ASP.NET Core 6.0**
- **Entity Framework Core**
- **SQL Server**
- **Bootstrap 5**
- **JavaScript / jQuery**

## Desarrollo

Para crear un nuevo desarrollo al proyecto, sigue estos pasos:

1. Crea una rama con tus cambios desde `develop`:
   ```bash
   git checkout -b feature/REQ-2024-000001
   ```

2. Realiza los cambios y haz commit:
   ```bash
   git commit -m 'REQ-2024-000001 Descripción de la nueva funcionalidad'
   ```

3. Sube la rama al repositorio remoto:
   ```bash
   git push origin feature/REQ-2024-000001
   ```

4. Abre un Pull Request para revisión desde tu rama `feature/REQ-2024-000001` hacia la rama `develop`.
