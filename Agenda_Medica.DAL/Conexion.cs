using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Agenda_Medica.DAL
{
    public class Conexion
    {
        // Cadena de conexión hacia tu base de datos local en MySQL Workbench
        private static string cadenaConexion = "Server=localhost;Database=agenda_medica;Uid=root;Pwd=tNx92mlAw.;";

        public static MySqlConnection ObtenerConexion()
        {
            try
            {
                MySqlConnection conexion = new MySqlConnection(cadenaConexion);
                conexion.Open();
                return conexion;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al conectar a la base de datos: " + ex.Message);
            }
        }
    }
}