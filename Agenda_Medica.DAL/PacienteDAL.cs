using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Agenda_Medica.DAL
{
    public class PacienteDAL
    {
        public DataTable ListarPacientes()
        {
            DataTable tablaPacientes = new DataTable();

            try
            {
                // Usamos el método de nuestra clase Conexion
                using (MySqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string query = "SELECT * FROM pacientes;"; // Asegúrate de que tu tabla se llame así en MySQL

                    using (MySqlCommand comando = new MySqlCommand(query, conexion))
                    {
                        using (MySqlDataAdapter adaptador = new MySqlDataAdapter(comando))
                        {
                            adaptador.Fill(tablaPacientes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los pacientes: " + ex.Message);
            }

            return tablaPacientes;
        }
    }
}