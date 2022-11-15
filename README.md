# **Aspectos técnicos**
## Versiones de software
Para la realización de este proyecto se utilizaron los siguientes software:

- SQL Server 2019 Developer versión 15.0.2000.5
- Microsoft SQL Server Management Studio 18 (SSMS) versión 18.9.1
- Visual Studio Community 2019 versión 16.10.2

## Sistema operativo

El proyecto fue realizado con el siguiente sistema operativo:
- Windows 10 x64 bits

## Patrón de diseño implementado
### View model

La razón por la que se decidió utilizar este patrón de diseño fue debido a que nos permite realizar una separación entre la interfaz gráfica y el desarrollo de la lógica del negocio, con el objetivo de mostrar en la interfaz gráfica unicamente los datos que se consideran más relevantes para los gestores.

Este patrón de diseño se utilizó para mostrar la información de contacto de las cabinas registradas y la informacion relacionada a las citas de primera y segunda dosis de los ciudadanos.

## Paquetes externos

- Microsoft.EntityFrameworkCore versión 5.0.7
- Microsoft.EntityFrameworkCore.SqlServer versión 5.0.7
- Microsoft.EntityFrameworkCore.Design versión 5.0.7
- iText7 versión 7.1.15

# **Instalación del software**
## Requisitos de instalación:

Para la correcta instalación y el buen funcionamiento del programa es necesario descargar e instalar lo siguiente:
 - [SQL Server 2019 Developer](https://www.microsoft.com/es-es/sql-server/sql-server-downloads)
 - [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/es-es/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15)
 - [.NET Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-desktop-5.0.7-windows-x64-installer)
- [Script Banco de datos.sql](https://github.com/UCASV/proyecto-final-grupo31/blob/c9ee513eb9bd88a591e5ff47f6b86995b9f5fa6e/Proyecto%20BASES-POO%202021/Bases%20de%20datos/Banco%20de%20datos.sql)

## Pasos a seguir:

Una vez cumplidos todos los requisitos, procedemos a la instalación del programa.

1. ### Creación de la base de datos.

Para que nuestro programa funcione correctamente es necesario realizar la creación de la base de datos antes de la instalación. Primero se debe abrir el archivo llamado “Banco de datos.sql”, el cual está contenido en “Proyecto BASES-POO 2021\Bases de datos” del repositorio.

<img src="https://raw.githubusercontent.com/Rockevinche/Images/main/POO/1.png" width = "250">

Se abrirá el archivo en SQL Server Management Studio (SSMS), luego se debe de conectar con el motor de la base de datos y con el servidor de la base de datos.

<img src="https://raw.githubusercontent.com/Rockevinche/Images/main/POO/2.gif">

Finalmente debemos ejecutar lo contenido en el script para la creación de la base de datos de la siguiente manera: 

<img src="https://raw.githubusercontent.com/Rockevinche/Images/main/POO/3.gif">

Si la creación de la base de datos se realizó correctamente, en la carpeta “Databases” se encontrará la nueva base de datos llamada “VaccinationDB”.

<img src="https://raw.githubusercontent.com/Rockevinche/Images/main/POO/4.png" width = "400">

En caso de no realizar este proceso, el programa no funcionará correctamente, por lo cual es estrictamente necesario realizar el proceso anteriormente descrito.

2. ### Instalación del programa.

Primero se debe abrir el instalador llamado “Setup.msi”, el cual está contenido en “Proyecto BASES-POO 2021\Programación orientada a objetos” del repositorio.

<img src="https://raw.githubusercontent.com/Rockevinche/Images/main/POO/5.png" width = 240>

Se abrirá el inicio del asistente para la instalación del programa. Para seguir con el proceso de instalación damos clic en “Siguiente”.

<img src="https://raw.githubusercontent.com/Rockevinche/Images/main/POO/6.png">

Posteriormente debemos escoger la ruta de instalación del programa, también debemos indicar si queremos que el programa se instale para todos los usuarios o únicamente para el usuario en el que está iniciada la sesión de Windows al momento de la instalación, luego damos clic en “Siguiente”.

<img src="https://raw.githubusercontent.com/Rockevinche/Images/main/POO/7.png">

Después confirmamos la instalación dando clic en siguiente.

<img src="https://raw.githubusercontent.com/Rockevinche/Images/main/POO/8.png">

Se nos mostrará un mensaje en donde se solicita la confirmación para permitir la instalación del programa, debemos dar clic en “Sí”.

<img src="https://raw.githubusercontent.com/Rockevinche/Images/main/POO/9.PNG" width = 525>

Posteriormente comenzará la instalación del programa. Al finalizar este proceso se nos mostrará la confirmación que el programa se ha instalado correctamente, terminaremos dando clic en "Cerrar".

<img src="https://raw.githubusercontent.com/Rockevinche/Images/main/POO/10.png">

Buscamos el acceso directo que se nos ha creado en el escritorio y damos doble clic para ejecutar el programa.

<img src="https://raw.githubusercontent.com/Rockevinche/Images/main/POO/11.png" width = 200>

De esta manera habremos instalado correctamente el programa y ya está listo para ser utilizado. 

<img src="https://raw.githubusercontent.com/Rockevinche/Images/main/POO/12.png" width = "510">

# **Manual de usuario**

Este documento tiene como objetivo brindarle al usuario las instrucciones necesarias para que pueda utilizar correctamente el programa, detallando todas las funciones que este ofrece.

Se puede acceder a través del siguiente [enlace](https://github.com/UCASV/proyecto-final-grupo31/blob/dea08973feaa1f2795b4196c66091c5236e87565/Proyecto%20BASES-POO%202021/Programaci%C3%B3n%20orientada%20a%20objetos/Manual%20de%20usuario.pdf).
