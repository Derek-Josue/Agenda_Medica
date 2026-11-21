using System;
using System.Collections.Generic;
using Agenda_Medica.DAL;
using Agenda_Medica.Entities;

namespace Agenda_Medica.BL
{
    public class MedicoBL
    {
        private readonly MedicoDAL _dal = new MedicoDAL();

        public ResultadoBL Listar(out List<Medico> lista)
        {
            lista = new List<Medico>();
            try
            {
                lista = _dal.Listar();
                return ResultadoBL.Ok("Médicos cargados.");
            }
            catch (Exception ex)
            {
                return ResultadoBL.Error(ex.Message);
            }
        }

        // Revisa los datos del médico antes de que la DAL los guarde
        public ResultadoBL Validar(Medico m)
        {
            if (m == null) return ResultadoBL.Error("Los datos del médico no pueden estar vacíos.");

            string error =
                Validaciones.Obligatorio(m.Cedula, "cédula", 20) ??
                Validaciones.Obligatorio(m.Nombre, "nombre", 80) ??
                Validaciones.Obligatorio(m.Apellidos, "apellidos", 100) ??
                Validaciones.Telefono(m.Telefono) ??
                Validaciones.Correo(m.Correo) ??
                Validaciones.Obligatorio(m.HorarioAtencion, "horario de atención", 500);

            if (error != null) return ResultadoBL.Error(error);

            if (!string.IsNullOrWhiteSpace(m.Correo) && m.Correo.Trim().Length > 100)
                return ResultadoBL.Error("El correo no puede superar 100 caracteres.");

            // Regla: el horario debe tener un formato que el sistema pueda interpretar
            // (lo necesita CitaBL para validar que la cita caiga dentro del horario del médico)
            string errorHorario;
            List<HorarioMedico.Bloque> bloques;
            if (!HorarioMedico.TryParse(m.HorarioAtencion, out bloques, out errorHorario))
                return ResultadoBL.Error(errorHorario);

            // Regla: todo médico debe tener al menos una especialidad
            if (m.Especialidades == null || m.Especialidades.Count == 0)
                return ResultadoBL.Error("Debe seleccionar al menos una especialidad.");

            return ResultadoBL.Ok("Datos del médico válidos.");
        }

        // Inserta si Id == 0, actualiza si Id > 0
        public ResultadoBL Guardar(Medico m)
        {
            ResultadoBL validacion = Validar(m);
            if (!validacion.Exito) return validacion;

            m.Cedula = m.Cedula.Trim();
            m.Nombre = m.Nombre.Trim();
            m.Apellidos = m.Apellidos.Trim();
            m.Telefono = m.Telefono.Trim();
            m.Correo = (m.Correo ?? "").Trim();
            m.HorarioAtencion = m.HorarioAtencion.Trim();

            try
            {
                if (m.Id > 0)
                {
                    _dal.Actualizar(m);
                    return ResultadoBL.Ok("Médico actualizado correctamente.");
                }

                _dal.Insertar(m);
                return ResultadoBL.Ok("Médico guardado correctamente.");
            }
            catch (Exception ex)
            {
                return ResultadoBL.Error(ex.Message);
            }
        }

        public ResultadoBL Eliminar(int idMedico)
        {
            if (idMedico <= 0)
                return ResultadoBL.Error("Seleccione un médico de la lista primero.");

            try
            {
                _dal.Eliminar(idMedico);
                return ResultadoBL.Ok("Médico eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return ResultadoBL.Error(ex.Message);
            }
        }
    }
}