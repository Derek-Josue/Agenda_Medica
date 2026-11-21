using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Agenda_Medica.Entities;

namespace Agenda_Medica.DAL
{
    public class MedicoDAL
    {
       

        // Devuelve los médicos con sus especialidades ya cargadas
        public List<Medico> Listar()
        {
            List<Medico> lista = new List<Medico>();
            Dictionary<int, Medico> porId = new Dictionary<int, Medico>();

            try
            {
                using (MySqlConnection conexion = Conexion.ObtenerConexion())
                {
                    // 1) Los médicos
                    const string sqlMedicos =
                        @"SELECT id_medico, cedula, nombre, apellidos, telefono, correo, horario_atencion
                            FROM medico
                           ORDER BY apellidos, nombre;";

                    using (MySqlCommand cmd = new MySqlCommand(sqlMedicos, conexion))
                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            Medico m = new Medico
                            {
                                Id = rd.GetInt32("id_medico"),
                                Cedula = rd.GetString("cedula"),
                                Nombre = rd.GetString("nombre"),
                                Apellidos = rd.GetString("apellidos"),
                                Telefono = Texto(rd, "telefono"),
                                Correo = Texto(rd, "correo"),
                                HorarioAtencion = Texto(rd, "horario_atencion")
                            };
                            lista.Add(m);
                            porId[m.Id] = m;
                        }
                    }

                    // 2) Sus especialidades (una sola consulta para todos)
                    const string sqlEsp =
                        @"SELECT me.id_medico, e.id_especialidad, e.nombre_especialidad AS nombre, e.descripcion
                            FROM medico_especialidad me
                      INNER JOIN especialidad e ON e.id_especialidad = me.id_especialidad
                           ORDER BY e.nombre_especialidad;";

                    using (MySqlCommand cmd = new MySqlCommand(sqlEsp, conexion))
                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            Medico m;
                            if (porId.TryGetValue(rd.GetInt32("id_medico"), out m))
                            {
                                m.Especialidades.Add(new Especialidad
                                {
                                    Id = rd.GetInt32("id_especialidad"),
                                    Nombre = rd.GetString("nombre"),
                                    Descripcion = Texto(rd, "descripcion")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los médicos: " + ex.Message);
            }

            return lista;
        }

        // Guarda el médico y sus especialidades en una sola transacción
        public void Insertar(Medico m)
        {
            const string sql =
                @"INSERT INTO medico (cedula, nombre, apellidos, telefono, correo, horario_atencion)
                  VALUES (@cedula, @nombre, @apellidos, @telefono, @correo, @horario);";

            try
            {
                using (MySqlConnection conexion = Conexion.ObtenerConexion())
                using (MySqlTransaction tx = conexion.BeginTransaction())
                {
                    try
                    {
                        long idNuevo;
                        using (MySqlCommand cmd = new MySqlCommand(sql, conexion, tx))
                        {
                            AgregarParametros(cmd, m);
                            cmd.ExecuteNonQuery();
                            idNuevo = cmd.LastInsertedId;
                        }

                        GuardarEspecialidades(conexion, tx, (int)idNuevo, m.Especialidades);
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062)   // UNIQUE: cédula repetida
            {
                throw new Exception("Ya existe un médico con esa cédula.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el médico: " + ex.Message);
            }
        }

        public void Actualizar(Medico m)
        {
            const string sql =
                @"UPDATE medico
                     SET cedula = @cedula, nombre = @nombre, apellidos = @apellidos,
                         telefono = @telefono, correo = @correo, horario_atencion = @horario
                   WHERE id_medico = @id;";

            try
            {
                using (MySqlConnection conexion = Conexion.ObtenerConexion())
                using (MySqlTransaction tx = conexion.BeginTransaction())
                {
                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand(sql, conexion, tx))
                        {
                            AgregarParametros(cmd, m);
                            cmd.Parameters.AddWithValue("@id", m.Id);
                            cmd.ExecuteNonQuery();
                        }

                        // Se reemplazan las especialidades: se borran las anteriores y se insertan las nuevas
                        using (MySqlCommand cmd = new MySqlCommand(
                            "DELETE FROM medico_especialidad WHERE id_medico = @id;", conexion, tx))
                        {
                            cmd.Parameters.AddWithValue("@id", m.Id);
                            cmd.ExecuteNonQuery();
                        }

                        GuardarEspecialidades(conexion, tx, m.Id, m.Especialidades);
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                throw new Exception("Ya existe un médico con esa cédula.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el médico: " + ex.Message);
            }
        }

        public void Eliminar(int idMedico)
        {
            try
            {
                using (MySqlConnection conexion = Conexion.ObtenerConexion())
                using (MySqlTransaction tx = conexion.BeginTransaction())
                {
                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand(
                            "DELETE FROM medico_especialidad WHERE id_medico = @id;", conexion, tx))
                        {
                            cmd.Parameters.AddWithValue("@id", idMedico);
                            cmd.ExecuteNonQuery();
                        }

                        using (MySqlCommand cmd = new MySqlCommand(
                            "DELETE FROM medico WHERE id_medico = @id;", conexion, tx))
                        {
                            cmd.Parameters.AddWithValue("@id", idMedico);
                            cmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();   // si falla el borrado, las especialidades vuelven a quedar como estaban
                        throw;
                    }
                }
            }
            catch (MySqlException ex) when (ex.Number == 1451)   // llave foránea: tiene citas
            {
                throw new Exception("No se puede eliminar: el médico tiene citas asociadas.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el médico: " + ex.Message);
            }
        }

        
        private static void AgregarParametros(MySqlCommand cmd, Medico m)
        {
            cmd.Parameters.AddWithValue("@cedula", m.Cedula);
            cmd.Parameters.AddWithValue("@nombre", m.Nombre);
            cmd.Parameters.AddWithValue("@apellidos", m.Apellidos);
            cmd.Parameters.AddWithValue("@telefono", m.Telefono);
            cmd.Parameters.AddWithValue("@correo", string.IsNullOrWhiteSpace(m.Correo) ? (object)DBNull.Value : m.Correo);
            cmd.Parameters.AddWithValue("@horario", string.IsNullOrWhiteSpace(m.HorarioAtencion) ? (object)DBNull.Value : m.HorarioAtencion);
        }

        private static void GuardarEspecialidades(MySqlConnection conexion, MySqlTransaction tx,
                                                  int idMedico, List<Especialidad> especialidades)
        {
            foreach (Especialidad esp in especialidades)
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO medico_especialidad (id_medico, id_especialidad) VALUES (@medico, @esp);",
                    conexion, tx))
                {
                    cmd.Parameters.AddWithValue("@medico", idMedico);
                    cmd.Parameters.AddWithValue("@esp", esp.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static string Texto(MySqlDataReader rd, string columna)
        {
            int i = rd.GetOrdinal(columna);
            return rd.IsDBNull(i) ? "" : rd.GetString(i);
        }
    }
}