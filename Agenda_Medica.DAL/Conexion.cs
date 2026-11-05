using System;
using MySql.Data.MySqlClient;

namespace Agenda_Medica.DAL
{
    public class Conexion
    {
        private static string cadenaConexion =
            "Server=localhost;" +
            "Port=3306;" +
            "Database=agenda_medica;" +
            "Uid=root;" +
            "Pwd=tNx92mlAw.;";

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