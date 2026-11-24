using System;
using MySql.Data.MySqlClient;
using Agenda_Medica.Entities;

namespace Agenda_Medica.DAL
{
    public class UsuarioDAL
    {
        public Usuario Validar(string usuario, string claveHash)
        {
            // CAMBIA esta línea por la forma en que tu PacienteDAL obtiene la conexión
            using (MySqlConnection conexion = Conexion.ObtenerConexion())
            {
                
                string sql = "SELECT id, usuario, rol FROM usuarios WHERE usuario=@u AND clave_hash=@c";
                MySqlCommand cmd = new MySqlCommand(sql, conexion);
                cmd.Parameters.AddWithValue("@u", usuario);
                cmd.Parameters.AddWithValue("@c", claveHash);

                using (MySqlDataReader r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        return new Usuario
                        {
                            Id = r.GetInt32(0),
                            NombreUsuario = r.GetString(1),
                            Rol = r.GetString(2)
                        };
                    }
                }
            }
            return null;
        }
    }
}