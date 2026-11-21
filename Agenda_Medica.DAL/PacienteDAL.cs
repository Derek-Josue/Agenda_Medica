using System;
using System.Data;
using MySql.Data.MySqlClient;
using Agenda_Medica.Entities;

namespace Agenda_Medica.DAL
{
    public class PacienteDAL
    {
        
        public DataTable ListarPacientes()
        {
            DataTable tabla = new DataTable();
            try
            {
                using (MySqlConnection conexion = Conexion.ObtenerConexion())
                using (MySqlCommand comando = new MySqlCommand(
                    "SELECT * FROM paciente ORDER BY apellidos, nombre;", conexion))
                using (MySqlDataAdapter adaptador = new MySqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los pacientes: " + ex.Message);
            }
            return tabla;
        }

        // Devuelve el paciente, o null si no existe esa cédula
        public Paciente BuscarPorCedula(string cedula)
        {
            const string sql = @"SELECT id_paciente, cedula, nombre, apellidos, fecha_nacimiento,
                                        genero, telefono, direccion, tipo_sangre
                                   FROM paciente
                                  WHERE cedula = @cedula
                                  LIMIT 1;";
            try
            {
                using (MySqlConnection conexion = Conexion.ObtenerConexion())
                using (MySqlCommand comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@cedula", cedula);

                    using (MySqlDataReader rd = comando.ExecuteReader())
                    {
                        if (!rd.Read()) return null;

                        return new Paciente
                        {
                            Id = rd.GetInt32("id_paciente"),
                            Cedula = rd.GetString("cedula"),
                            Nombre = rd.GetString("nombre"),
                            Apellidos = rd.GetString("apellidos"),
                            FechaNacimiento = Fecha(rd, "fecha_nacimiento"),
                            Genero = Texto(rd, "genero"),
                            Telefono = Texto(rd, "telefono"),
                            Direccion = Texto(rd, "direccion"),
                            TipoSangre = Texto(rd, "tipo_sangre")
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el paciente: " + ex.Message);
            }
        }

        public void Insertar(Paciente p)
        {
            const string sql = @"INSERT INTO paciente
                (cedula, nombre, apellidos, fecha_nacimiento, genero, telefono, direccion, tipo_sangre)
                VALUES
                (@cedula, @nombre, @apellidos, @fecha, @genero, @telefono, @direccion, @sangre);";
            Ejecutar(sql, p, false);
        }

        public void Actualizar(Paciente p)
        {
            const string sql = @"UPDATE paciente
                SET cedula = @cedula, nombre = @nombre, apellidos = @apellidos,
                    fecha_nacimiento = @fecha, genero = @genero, telefono = @telefono,
                    direccion = @direccion, tipo_sangre = @sangre
                WHERE id_paciente = @id;";
            Ejecutar(sql, p, true);
        }

        public void Eliminar(int idPaciente)
        {
            try
            {
                using (MySqlConnection conexion = Conexion.ObtenerConexion())
                using (MySqlCommand comando = new MySqlCommand(
                    "DELETE FROM paciente WHERE id_paciente = @id;", conexion))
                {
                    comando.Parameters.AddWithValue("@id", idPaciente);
                    comando.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex) when (ex.Number == 1451)   // llave foránea (citas, historial)
            {
                throw new Exception("No se puede eliminar: el paciente tiene citas o historial asociado.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el paciente: " + ex.Message);
            }
        }

      
        private void Ejecutar(string sql, Paciente p, bool incluirId)
        {
            try
            {
                using (MySqlConnection conexion = Conexion.ObtenerConexion())
                using (MySqlCommand comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@cedula", p.Cedula);
                    comando.Parameters.AddWithValue("@nombre", p.Nombre);
                    comando.Parameters.AddWithValue("@apellidos", p.Apellidos);
                    comando.Parameters.AddWithValue("@fecha", p.FechaNacimiento.Date);
                    comando.Parameters.AddWithValue("@genero", p.Genero);
                    comando.Parameters.AddWithValue("@telefono", p.Telefono);
                    comando.Parameters.AddWithValue("@direccion", p.Direccion);
                    comando.Parameters.AddWithValue("@sangre", p.TipoSangre);
                    if (incluirId) comando.Parameters.AddWithValue("@id", p.Id);

                    comando.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062)   // UNIQUE (cédula repetida)
            {
                throw new Exception("Ya existe un paciente con esa cédula.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el paciente: " + ex.Message);
            }
        }

        private static string Texto(MySqlDataReader rd, string columna)
        {
            int i = rd.GetOrdinal(columna);
            return rd.IsDBNull(i) ? "" : rd.GetString(i);
        }

        private static DateTime Fecha(MySqlDataReader rd, string columna)
        {
            int i = rd.GetOrdinal(columna);
            return rd.IsDBNull(i) ? DateTime.Today : rd.GetDateTime(i);
        }
    }
}