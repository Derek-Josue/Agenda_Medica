using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Agenda_Medica.Entities;

namespace Agenda_Medica.DAL
{
    public class EspecialidadDAL
    {
        // Conexion.ObtenerConexion() ya devuelve la conexión abierta: no se llama a Open().

        public List<Especialidad> Listar()
        {
            List<Especialidad> lista = new List<Especialidad>();
            try
            {
                using (MySqlConnection conexion = Conexion.ObtenerConexion())
                using (MySqlCommand comando = new MySqlCommand(
                    "SELECT id_especialidad, nombre_especialidad AS nombre, descripcion FROM especialidad ORDER BY nombre_especialidad;", conexion))
                using (MySqlDataReader rd = comando.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        int iDesc = rd.GetOrdinal("descripcion");
                        lista.Add(new Especialidad
                        {
                            Id = rd.GetInt32("id_especialidad"),
                            Nombre = rd.GetString("nombre"),
                            Descripcion = rd.IsDBNull(iDesc) ? "" : rd.GetString(iDesc)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las especialidades: " + ex.Message);
            }
            return lista;
        }

        public void Insertar(Especialidad e)
        {
            Ejecutar("INSERT INTO especialidad (nombre_especialidad, descripcion) VALUES (@nombre, @descripcion);", e, false);
        }

        public void Actualizar(Especialidad e)
        {
            Ejecutar("UPDATE especialidad SET nombre_especialidad = @nombre, descripcion = @descripcion " +
                     "WHERE id_especialidad = @id;", e, true);
        }

        public void Eliminar(int idEspecialidad)
        {
            try
            {
                using (MySqlConnection conexion = Conexion.ObtenerConexion())
                using (MySqlCommand comando = new MySqlCommand(
                    "DELETE FROM especialidad WHERE id_especialidad = @id;", conexion))
                {
                    comando.Parameters.AddWithValue("@id", idEspecialidad);
                    comando.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex) when (ex.Number == 1451)   // llave foránea: la usa algún médico
            {
                throw new Exception("No se puede eliminar: hay médicos asociados a esta especialidad.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la especialidad: " + ex.Message);
            }
        }

       
        private void Ejecutar(string sql, Especialidad e, bool incluirId)
        {
            try
            {
                using (MySqlConnection conexion = Conexion.ObtenerConexion())
                using (MySqlCommand comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", e.Nombre);
                    comando.Parameters.AddWithValue("@descripcion", e.Descripcion);
                    if (incluirId) comando.Parameters.AddWithValue("@id", e.Id);

                    comando.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062)   // UNIQUE: nombre repetido
            {
                throw new Exception("Ya existe una especialidad con ese nombre.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar la especialidad: " + ex.Message);
            }
        }
    }
}