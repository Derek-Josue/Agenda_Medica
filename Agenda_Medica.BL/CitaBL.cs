using System;
using System.Collections.Generic;
using System.Linq;
using Agenda_Medica.DAL;
using Agenda_Medica.Entities;

namespace Agenda_Medica.BL
{
    public class CitaBL
    {
        private readonly CitaDAL _dal = new CitaDAL();

        public ResultadoBL Listar(DateTime? fecha, int idMedico, int idPaciente, out List<Cita> lista)
        {
            lista = new List<Cita>();
            try
            {
                lista = _dal.Listar(fecha, idMedico, idPaciente);
                return ResultadoBL.Ok("Citas cargadas.");
            }
            catch (Exception ex)
            {
                return ResultadoBL.Error(ex.Message);
            }
        }
        // Historial de citas de un paciente (más recientes primero)
        public ResultadoBL HistorialPaciente(string identificacion, out List<Cita> lista)
        {
            lista = new List<Cita>();
            if (string.IsNullOrWhiteSpace(identificacion))
                return ResultadoBL.Error("Debe indicar el ID del paciente.");
            try
            {
                lista = _dal.ListarPorIdentificacion(identificacion.Trim());
                return ResultadoBL.Ok("Historial cargado.");
            }
            catch (Exception ex)
            {
                return ResultadoBL.Error(ex.Message);
            }
        }

        // Validaciones simples de los datos (sin tocar la base de datos)
        public ResultadoBL Validar(Cita c)
        {
            if (c == null) return ResultadoBL.Error("Los datos de la cita no pueden estar vacíos.");
            if (c.IdMedico <= 0) return ResultadoBL.Error("Debe seleccionar un médico.");
            if (c.IdPaciente <= 0) return ResultadoBL.Error("Debe seleccionar un paciente.");

            if (!ReglasCita.DuracionValida(c.Duracion))
                return ResultadoBL.Error("La duración debe ser de 30, 45 o 60 minutos.");

            if (Array.IndexOf(ModalidadesCita.Todas, c.Modalidad) < 0)
                return ResultadoBL.Error("Seleccione una modalidad válida (Presencial, Virtual o Urgencia).");

            return ResultadoBL.Ok("Datos de la cita válidos.");
        }

        // Agenda una cita nueva (Id == 0) o modifica/reprograma una existente (Id > 0)
        public ResultadoBL Guardar(Cita c)
        {
            ResultadoBL validacion = Validar(c);
            if (!validacion.Exito) return validacion;

            c.Fin = ReglasCita.CalcularFin(c.Inicio, c.Duracion);

            try
            {
                Cita original = null;
                if (c.Id > 0)
                {
                    original = _dal.ObtenerPorId(c.Id);
                    if (original == null) return ResultadoBL.Error("La cita ya no existe.");
                    if (EsFinal(original.Estado))
                        return ResultadoBL.Error("No se puede modificar una cita con estado " + original.Estado + ".");
                }

                // ¿Cambió algo que afecta la agenda? (en una cita nueva siempre es true)
                bool cambioAgenda = original == null
                    || original.Inicio != c.Inicio
                    || original.IdMedico != c.IdMedico
                    || original.Duracion != c.Duracion;

                if (cambioAgenda)
                {
                    // Regla: no se agenda en el pasado
                    if (c.Inicio < DateTime.Now)
                        return ResultadoBL.Error("No se puede agendar una cita en una fecha y hora que ya pasó.");

                    // Regla: respetar el horario de atención del médico
                    string horarioTexto = _dal.ObtenerHorarioMedico(c.IdMedico);
                    List<HorarioMedico.Bloque> bloques;
                    string errorHorario;
                    if (!HorarioMedico.TryParse(horarioTexto, out bloques, out errorHorario))
                        return ResultadoBL.Error("El horario de atención del médico no se puede interpretar. " +
                                                 "Corríjalo en el módulo de Médicos. " + errorHorario);

                    if (!HorarioMedico.Permite(bloques, c.Inicio, c.Fin))
                        return ResultadoBL.Error("La cita debe quedar completa dentro del horario de atención del médico (" +
                                                 horarioTexto.Trim() + ").");
                }

                // Regla: el médico no puede tener dos citas que se crucen
                foreach (Cita otra in _dal.CitasActivasDeMedico(c.IdMedico, c.Inicio, c.Id))
                {
                    if (ReglasCita.Solapan(c.Inicio, c.Fin, otra.Inicio, otra.Fin))
                        return ResultadoBL.Error("El médico ya tiene una cita de " + otra.Inicio.ToString("HH:mm") +
                                                 " a " + otra.Fin.ToString("HH:mm") + " que choca con ese horario.");
                }

                // Regla: el paciente no puede tener dos citas simultáneas (aunque sea con otro médico)
                foreach (Cita otra in _dal.CitasActivasDePaciente(c.IdPaciente, c.Inicio, c.Id))
                {
                    if (ReglasCita.Solapan(c.Inicio, c.Fin, otra.Inicio, otra.Fin))
                        return ResultadoBL.Error("El paciente ya tiene una cita de " + otra.Inicio.ToString("HH:mm") +
                                                 " a " + otra.Fin.ToString("HH:mm") + " con " + otra.MedicoNombre + ".");
                }

                // Estado: lo decide la BL, no la pantalla
                if (original == null)
                {
                    c.Estado = EstadosCita.Programada;
                    _dal.Insertar(c);
                    return ResultadoBL.Ok("Cita agendada correctamente.");
                }

                if (cambioAgenda)
                {
                    c.Estado = EstadosCita.Reprogramada;
                    _dal.Actualizar(c);
                    return ResultadoBL.Ok("Cita reprogramada correctamente.");
                }

                c.Estado = original.Estado;
                _dal.Actualizar(c);
                return ResultadoBL.Ok("Cita actualizada correctamente.");
            }
            catch (Exception ex)
            {
                return ResultadoBL.Error(ex.Message);
            }
        }

        // Confirmar, marcar Atendida, Cancelar o marcar No asistió.
        // Programada y Reprogramada no se eligen a mano: las asigna Guardar().
        public ResultadoBL CambiarEstado(int idCita, string nuevoEstado)
        {
            if (idCita <= 0) return ResultadoBL.Error("Seleccione una cita de la lista primero.");

            if (nuevoEstado != EstadosCita.Confirmada && nuevoEstado != EstadosCita.Atendida &&
                nuevoEstado != EstadosCita.Cancelada && nuevoEstado != EstadosCita.NoAsistio)
                return ResultadoBL.Error("Estado no permitido.");

            try
            {
                Cita cita = _dal.ObtenerPorId(idCita);
                if (cita == null) return ResultadoBL.Error("La cita ya no existe.");

                if (EsFinal(cita.Estado))
                    return ResultadoBL.Error("La cita ya está " + cita.Estado + " y no se puede cambiar.");

                if (nuevoEstado == EstadosCita.Confirmada && cita.Inicio < DateTime.Now)
                    return ResultadoBL.Error("No se puede confirmar una cita que ya pasó.");

                if ((nuevoEstado == EstadosCita.Atendida || nuevoEstado == EstadosCita.NoAsistio) &&
                    cita.Inicio > DateTime.Now)
                    return ResultadoBL.Error("Esa cita todavía no ha ocurrido.");

                _dal.CambiarEstado(idCita, nuevoEstado);
                return ResultadoBL.Ok("Estado actualizado a " + nuevoEstado + ".");
            }
            catch (Exception ex)
            {
                return ResultadoBL.Error(ex.Message);
            }
        }

        // Estados que cierran la cita: ya no se modifican
        private static bool EsFinal(string estado)
        {
            return estado == EstadosCita.Atendida
                || estado == EstadosCita.Cancelada
                || estado == EstadosCita.NoAsistio;
        }
    }
}