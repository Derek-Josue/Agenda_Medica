using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Agenda_Medica.Entities;

namespace Agenda_Medica.DAL
{
    public class CitaDAL
    {
        // Conexion.ObtenerConexion() ya devuelve la conexión abierta: no se llama a Open().

        private const string SqlBase =
            @"SELECT c.id_cita, c.id_medico, c.id_paciente, c.fecha_hora_inicio, c.fecha_hora_fin,
                     c.duracion, c.modalidad, c.estado,
                     CONCAT(m.nombre, ' ', m.apellidos) AS medico,
                     CONCAT(p.nombre, ' ', p.apellidos) AS paciente
                FROM cita c
          INNER JOIN medico   m ON m.id_medico   = c.id_medico
          INNER JOIN paciente p ON p.id_paciente = c.id_paciente ";

        // Filtros opcionales: fecha (día completo), médico y paciente (0 = sin filtro)
        public List<Cita> Listar(DateTime? fecha, int idMedico, int idPaciente)
        {
            List<string> condiciones = new List<string>();
            if (fecha.HasValue) condiciones.Add("c.fecha_hora_inicio >= @desde AND c.fecha_hora_inicio < @hasta");
            if (idMedico > 0) condiciones.Add("c.id_medico = @medico");
            if (idPaciente > 0) condiciones.Add("c.id_paciente = @paciente");

            string sql = SqlBase +
                (condiciones.Count > 0 ? " WHERE " + string.Join(" AND ", condiciones) : "") +
                " ORDER BY c.fecha_hora_inicio;";

            try
            {
                using (MySqlConnection conexion = Conexion.ObtenerConexion())
                using (MySqlCommand cmd = new MySqlCommand(sql, conexion))
                {
                    if (fecha.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@desde", fecha.Value.Date);
                        cmd.Parameters.AddWithValue("@hasta", fecha.Value.Date.AddDays(1));
                    }
                    if (idMedico > 0) cmd.Parameters.AddWithValue("@medico", idMedico);
                    if (idPaciente > 0) cmd.Parameters.AddWithValue("@paciente", idPaciente);

                    return LeerCitas(cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las citas: " + ex.Message);
            }
        }

        // Devuelve la cita, o null si no existe
        public Cita ObtenerPorId(int idCita)
        {
            try
            {
                using (MySqlConnection conexion = Conexion.ObtenerConexion())
                using (MySqlCommand cmd = new MySqlCommand(SqlBase + " WHERE c.id_cita = @id;", conexion))
                {
                    cmd.Parameters.AddWithValue("@id", idCita);
                    List<Cita> lista = LeerCitas(cmd);
                    return lista.Count > 0 ? lista[0] : null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar la cita: " + ex.Message);
            }
        }

        // Citas que ocupan el horario del médico ese día (no cuenta las canceladas ni las de No asistió)
        public List<Cita> CitasActivasDeMedico(int idMedico, DateTime dia, int idCitaExcluir)
        {
            return CitasActivas("c.id_medico", idMedico, dia, idCitaExcluir);
        }

        // Citas que ocupan el horario del paciente ese día
        public List<Cita> CitasActivasDePaciente(int idPaciente, DateTime dia, int idCitaExcluir)
        {
            return CitasActivas("c.id_paciente", idPaciente, dia, idCitaExcluir);
        }

        // Texto de horario_atencion del médico ("" si no tiene)
        public string ObtenerHorarioMedico(int idMedico)
        {
            try
            {
                using (MySqlConnection conexion = Conexion.ObtenerConexion())
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT horario_atencion FROM medico WHERE id_medico = @id;", conexion))
                {
                    cmd.Parameters.AddWithValue("@id", idMedico);
                    object r = cmd.ExecuteScalar();
                    return (r == null || r == DBNull.Value) ? "" : Convert.ToString(r);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el horario del médico: " + ex.Message);
            }
        }

        public void Insertar(Cita c)
        {
            const string sql =
                @"INSERT INTO cita (id_medico, id_paciente, fecha_hora_inicio, fecha_hora_fin, duracion, modalidad, estado)
                  VALUES (@medico, @paciente, @inicio, @fin, @duracion, @modalidad, @estado);";
            Ejecutar(sql, c, false);
        }

        public void Actualizar(Cita c)
        {
            const string sql =
                @"UPDATE cita
                     SET id_medico = @medico, id_paciente = @paciente,
                         fecha_hora_inicio = @inicio, fecha_hora_fin = @fin,
                         duracion = @duracion, modalidad = @modalidad, estado = @estado
                   WHERE id_cita = @id;";
            Ejecutar(sql, c, true);
        }

        public void CambiarEstado(int idCita, string nuevoEstado)
        {
            try
            {
                using (MySqlConnection conexion = Conexion.ObtenerConexion())
                using (MySqlCommand cmd = new MySqlCommand(
                    "UPDATE cita SET estado = @estado WHERE id_cita = @id;", conexion))
                {
                    cmd.Parameters.AddWithValue("@estado", nuevoEstado);
                    cmd.Parameters.AddWithValue("@id", idCita);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar el estado de la cita: " + ex.Message);
            }
        }

        // ------------------------------------------------------------
        private List<Cita> CitasActivas(string columna, int id, DateTime dia, int idCitaExcluir)
        {
            // 'columna' es siempre un literal interno (c.id_medico / c.id_paciente), nunca texto del usuario
            string sql = SqlBase +
                @" WHERE " + columna + @" = @id
                     AND c.id_cita <> @excluir
                     AND c.estado NOT IN (@cancelada, @noAsistio)
                     AND c.fecha_hora_inicio >= @desde AND c.fecha_hora_inicio < @hasta
                   ORDER BY c.fecha_hora_inicio;";
            try
            {
                using (MySqlConnection conexion = Conexion.ObtenerConexion())
                using (MySqlCommand cmd = new MySqlCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@excluir", idCitaExcluir);
                    cmd.Parameters.AddWithValue("@cancelada", EstadosCita.Cancelada);
                    cmd.Parameters.AddWithValue("@noAsistio", EstadosCita.NoAsistio);
                    cmd.Parameters.AddWithValue("@desde", dia.Date);
                    cmd.Parameters.AddWithValue("@hasta", dia.Date.AddDays(1));
                    return LeerCitas(cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al revisar la disponibilidad: " + ex.Message);
            }
        }

        private void Ejecutar(string sql, Cita c, bool incluirId)
        {
            try
            {
                using (MySqlConnection conexion = Conexion.ObtenerConexion())
                using (MySqlCommand cmd = new MySqlCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@medico", c.IdMedico);
                    cmd.Parameters.AddWithValue("@paciente", c.IdPaciente);
                    cmd.Parameters.AddWithValue("@inicio", c.Inicio);
                    cmd.Parameters.AddWithValue("@fin", c.Fin);
                    cmd.Parameters.AddWithValue("@duracion", c.Duracion);
                    cmd.Parameters.AddWithValue("@modalidad", c.Modalidad);
                    cmd.Parameters.AddWithValue("@estado", c.Estado);
                    if (incluirId) cmd.Parameters.AddWithValue("@id", c.Id);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar la cita: " + ex.Message);
            }
        }

        private static List<Cita> LeerCitas(MySqlCommand cmd)
        {
            List<Cita> lista = new List<Cita>();
            using (MySqlDataReader rd = cmd.ExecuteReader())
            {
                while (rd.Read())
                {
                    lista.Add(new Cita
                    {
                        Id = rd.GetInt32("id_cita"),
                        IdMedico = rd.GetInt32("id_medico"),
                        IdPaciente = rd.GetInt32("id_paciente"),
                        MedicoNombre = rd.GetString("medico"),
                        PacienteNombre = rd.GetString("paciente"),
                        Inicio = rd.GetDateTime("fecha_hora_inicio"),
                        Fin = rd.GetDateTime("fecha_hora_fin"),
                        Duracion = rd.GetInt32("duracion"),
                        Modalidad = rd.GetString("modalidad"),
                        Estado = rd.GetString("estado")
                    });
                }
            }
            return lista;
        }
    }
}