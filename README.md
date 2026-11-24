# Agenda Médica con Turnos y Disponibilidad

Sistema de escritorio desarrollado en C# y Windows Forms para administrar pacientes, médicos, especialidades y citas médicas, utilizando MySQL como sistema de gestión de base de datos.

Descripción del proyecto

El proyecto consiste en desarrollar una aplicación que permita gestionar las actividades de una agenda médica de manera organizada. Su propósito es centralizar la información de pacientes y médicos, facilitar la administración de citas y evitar conflictos relacionados con los horarios y la disponibilidad de los profesionales.

El sistema se desarrolla como parte del curso Programación II de la Universidad Internacional San Isidro Labrador (UISIL).

Tecnologías utilizadas

- Lenguaje de programación: C#
- Interfaz gráfica:Windows Forms
- Base de datos: MySQL
- IDE: Visual Studio
- Control de versiones:Git y GitHub
- Framework: .NET Framework 4.7.2

Arquitectura del sistema

El proyecto utiliza una arquitectura organizada en tres capas principales, además de un proyecto de entidades:

 Proyecto  Responsabilidad 

 Agenda_Medica  Capa de presentación (UI). Contiene los formularios y la interfaz gráfica. 
 Agenda_Medica.BL  Capa de lógica de negocio (BL). Gestiona las validaciones y reglas de negocio. 
 Agenda_Medica.DAL  Capa de acceso a datos (DAL). Gestiona la conexión y las operaciones con MySQL. 
 Agenda_Medica.Entities  Contiene las clases que representan las entidades del sistema. 

Esta organización permite separar las responsabilidades y facilita el mantenimiento y desarrollo de la aplicación.

 Funcionalidades del sistema

El sistema contempla las siguientes funcionalidades:

 Registro, consulta y modificación de pacientes.
 Administración de médicos.
 Administración de especialidades médicas.
 Asociación de médicos con especialidades.
 Registro y administración de citas.
 Consulta de agendas por médico, paciente, fecha y estado.
 Control de disponibilidad de los médicos.
 Prevención de citas simultáneas.
Manejo de usuarios y roles.
 Consulta del historial médico.
 Generación de reportes básicos.

Base de datos

El sistema utiliza MySQL y una base de datos denominada:

`agenda_medica`

El modelo de datos contempla las siguientes tablas:

 ROL
 USUARIO
 MEDICO
 ESPECIALIDAD
 MEDICO_ESPECIALIDAD
 PACIENTE
  CITA
 HISTORIAL_MEDICO

Las relaciones entre las tablas se establecen mediante claves primarias y foráneas para mantener la integridad de la información.

Requisitos para ejecutar el proyecto

Antes de ejecutar la aplicación, es necesario contar con:

1. Windows.
2. Visual Studio con soporte para Windows Forms y .NET Framework 4.7.2.
3. MySQL Server.
4. MySQL Workbench (recomendado para administrar la base de datos).
5. Git.

Instalación y configuración

1. Clonar el repositorio

Abrir una terminal y ejecutar:

Ingresar a la carpeta del proyecto:

```bash
cd Agenda_Medica
```

2. Configurar MySQL

1. Instalar y ejecutar MySQL Server.
2. Abrir MySQL Workbench y establecer una conexión con el servidor local.
3. Crear una base de datos llamada `agenda_medica`.
4. Ejecutar el script SQL de creación de tablas incluido en el repositorio.
5. Configurar las credenciales de acceso según el usuario MySQL creado en cada computadora.

Importante: Cada integrante debe tener su propia instalación y configuración local de MySQL. La base de datos local no se sincroniza automáticamente entre los integrantes del equipo.

3. Configurar la conexión

La aplicación necesita una cadena de conexión válida para comunicarse con MySQL.

Se debe revisar la configuración de conexión del proyecto y establecer los siguientes datos de acuerdo con la instalación local:

- Servidor: `localhost`
- Puerto: `3306`
- Base de datos: `agenda_medica`
- Usuario: el usuario MySQL configurado localmente.
- Contraseña: la contraseña correspondiente a dicho usuario.

No se deben subir contraseñas ni credenciales privadas al repositorio de GitHub.

4. Abrir y ejecutar el proyecto

1. Abrir la carpeta del repositorio.
2. Buscar y abrir el archivo de solución `.sln` con Visual Studio.
3. Esperar a que Visual Studio cargue los proyectos y sus dependencias.
4. Verificar las referencias entre los proyectos.
5. Establecer `Agenda_Medica` como proyecto de inicio.
6. Ejecutar la aplicación.

Reglas de negocio

El sistema contempla reglas para garantizar el correcto funcionamiento de la agenda:

- Un médico no puede tener dos citas simultáneas.
- Un paciente no puede tener dos citas al mismo tiempo.
- Las citas deben respetar los horarios de atención de los médicos.
- Se contemplan diferentes duraciones de citas.
- Las citas pueden ser programadas, confirmadas, atendidas, canceladas o reprogramadas.
- Se deben validar los datos antes de almacenarlos.

Equipo de desarrollo

Desarollado por:

- Alvin Jimenez Vargas
- Eliecer Najera Ramírez
- Derek Josué Orozco Cortes

Universidad Internacional San Isidro Labrador (UISIL)  


