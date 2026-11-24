# Agenda_Medica
Agenda Médica
 
Sistema de escritorio para la administración de citas médicas, pacientes y especialidades.
 
Requisitos previos que debes tener instalados en tu computadora
 
1. Visual Studio (cualquier versión compatible con .NET y Windows Forms)
2. MySQL Server (el motor de la base de datos)
3. MySQL Workbench (el programa gráfico para gestionar MySQL)
4. Conexión a internet para descargar el paquete MySqlConnector desde NuGet
 
 Tecnologías utilizadas
 
- Lenguaje de programación: C#
- Interfaz gráfica: Windows Forms
- Gestor de base de datos: MySQL
- Arquitectura del proyecto: 3 capas (UI, BLL, DAL)
 
 Funcionalidades del sistema
 
Inicio de sesión con roles de usuario
Registro, modificación y eliminación de pacientes
Gestión de citas médicas
Administración de especialidades
Visualización del historial de citas
 
Pasos para ejecutar la aplicación 
 
Paso 1: Descargar el proyecto desde GitHub
 
1. Entra al repositorio de GitHub del proyecto
2. Haz clic en el botón verde que dice Code
3. Selecciona la opción Download ZIP
4. Espera a que se descargue el archivo comprimido
5. Ve a la carpeta de descargas de tu computadora
6. Haz clic derecho sobre el archivo  .zip  y selecciona Extraer todo
7. Elige la carpeta de destino y dale en extraer
 
Paso 2: Abrir la solución en Visual Studio
 
1. Abre el programa Visual Studio
2. Ve a la opción Abrir → Proyecto o solución
3. Navega hasta la carpeta donde extrajiste los archivos
4. Selecciona el archivo llamado  Agenda_Medica.sln 
5. Dale clic en Abrir
6. Espera a que se cargue todo el proyecto en el Explorador de Soluciones
 
Paso 3: Verificar que el paquete MySqlConnector esté instalado
 
1. Haz clic derecho sobre el nombre de tu proyecto en el Explorador de Soluciones
2. Selecciona Administrar paquetes NuGet
3. Ve a la pestaña Instalados
4. Busca el paquete llamado MySqlConnector
5. Si no aparece, ve a la pestaña Examinar, búscalo y dale clic en Instalar
 
Paso 4: Preparar la base de datos en MySQL Workbench
 
1. Abre el programa MySQL Workbench
2. Haz clic sobre tu conexión local de MySQL
3. Escribe tu nombre de usuario de MySQL y tu contraseña
4. Dale clic en OK para conectarte al servidor
5. Abre la pestaña que dice SQL
6. Copia todo el código SQL que viene incluido en el repositorio
7. Pégalo dentro del recuadro de texto
8. Presiona el botón de ejecutar 
9. Verifica que se creen correctamente la base de datos y todas las tablas
 
Paso 5: Configurar la cadena de conexión dentro del código
 
1. Regresa a Visual Studio
2. Busca en tu código la línea que contiene la cadena de conexión a MySQL
3. Reemplaza los valores por tu propio usuario y contraseña de MySQL
4. Guarda todos los cambios que hiciste
 
Paso 6: Ejecutar la aplicación
 
1. Revisa que MySQL Server se encuentre encendido y funcionando
2. Presiona el botón de Iniciar  que está en la barra superior de Visual Studio
3. Espera a que se abra la ventana de inicio de sesión del programa
4. ¡Listo, ya puedes usar el sistema de Agenda Médica!
 
Declaración de uso de inteligencia artificial
 
Se utilizó inteligencia artificial como herramienta de apoyo para la redacción, explicación y corrección del código. El diseño, la lógica principal y la estructura del proyecto fueron desarrollados y comprendidos por mí.
 
Autores
 
Dereck Josué
Alvin Jesús
Eliecer Andrés
