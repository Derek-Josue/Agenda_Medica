using System;
using System.Data;
using Agenda_Medica.DAL;
using Agenda_Medica.Entities;

namespace Agenda_Medica.BL
{
    public class PacienteBL
    {
        private readonly PacienteDAL _dal = new PacienteDAL();

        public DataTable ListarPacientes()
        {
            return _dal.ListarPacientes();
        }

        // Revisa todos los datos del paciente antes de que la DAL los toque
        public ResultadoBL Validar(Paciente p)
        {
            if (p == null) return ResultadoBL.Error("Los datos del paciente no pueden estar vacíos.");

            string error =
                Validaciones.Obligatorio(p.Cedula, "cédula", 20) ??
                Validaciones.Obligatorio(p.Nombre, "nombre", 80) ??
                Validaciones.Obligatorio(p.Apellidos, "apellidos", 100) ??
                Validaciones.Telefono(p.Telefono) ??
                Validaciones.Obligatorio(p.Genero, "género", 20) ??
                Validaciones.Obligatorio(p.TipoSangre, "tipo de sangre", 5);

            if (error != null) return ResultadoBL.Error(error);

            if (p.FechaNacimiento.Date > DateTime.Today)
                return ResultadoBL.Error("La fecha de nacimiento no puede ser futura.");

            if (!string.IsNullOrWhiteSpace(p.Direccion) && p.Direccion.Trim().Length > 255)
                return ResultadoBL.Error("La dirección no puede superar 255 caracteres.");

            return ResultadoBL.Ok("Datos del paciente válidos.");
        }

        // Inserta si Id == 0, actualiza si Id > 0
        public ResultadoBL Guardar(Paciente p)
        {
            ResultadoBL validacion = Validar(p);
            if (!validacion.Exito) return validacion;

            // Normalizar espacios
            p.Cedula = p.Cedula.Trim();
            p.Nombre = p.Nombre.Trim();
            p.Apellidos = p.Apellidos.Trim();
            p.Telefono = p.Telefono.Trim();
            p.Direccion = (p.Direccion ?? "").Trim();

            try
            {
                if (p.Id > 0)
                {
                    _dal.Actualizar(p);
                    return ResultadoBL.Ok("Paciente actualizado correctamente.");
                }

                _dal.Insertar(p);
                return ResultadoBL.Ok("Paciente guardado correctamente.");
            }
            catch (Exception ex)
            {
                return ResultadoBL.Error(ex.Message);
            }
        }

        // Si lo encuentra, lo deja en 'paciente'; si no, Exito = false
        public ResultadoBL BuscarPorCedula(string cedula, out Paciente paciente)
        {
            paciente = null;

            if (string.IsNullOrWhiteSpace(cedula))
                return ResultadoBL.Error("Escriba una cédula para buscar.");

            try
            {
                paciente = _dal.BuscarPorCedula(cedula.Trim());
                if (paciente == null) return ResultadoBL.Error("Paciente no encontrado.");
                return ResultadoBL.Ok("Paciente encontrado.");
            }
            catch (Exception ex)
            {
                return ResultadoBL.Error(ex.Message);
            }
        }

        public ResultadoBL Eliminar(int idPaciente)
        {
            if (idPaciente <= 0) return ResultadoBL.Error("Busque un paciente primero.");

            try
            {
                _dal.Eliminar(idPaciente);
                return ResultadoBL.Ok("Paciente eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return ResultadoBL.Error(ex.Message);
            }
        }
    }
}