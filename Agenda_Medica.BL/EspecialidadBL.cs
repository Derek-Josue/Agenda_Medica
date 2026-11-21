using System;
using System.Collections.Generic;
using Agenda_Medica.DAL;
using Agenda_Medica.Entities;

namespace Agenda_Medica.BL
{
    public class EspecialidadBL
    {
        private readonly EspecialidadDAL _dal = new EspecialidadDAL();

        public ResultadoBL Listar(out List<Especialidad> lista)
        {
            lista = new List<Especialidad>();
            try
            {
                lista = _dal.Listar();
                return ResultadoBL.Ok("Especialidades cargadas.");
            }
            catch (Exception ex)
            {
                return ResultadoBL.Error(ex.Message);
            }
        }

        public ResultadoBL Validar(Especialidad e)
        {
            if (e == null) return ResultadoBL.Error("Los datos de la especialidad no pueden estar vacíos.");

            string error = Validaciones.Obligatorio(e.Nombre, "nombre", 100);
            if (error != null) return ResultadoBL.Error(error);

            // La descripción es opcional, pero con límite de largo
            if (!string.IsNullOrWhiteSpace(e.Descripcion) && e.Descripcion.Trim().Length > 255)
                return ResultadoBL.Error("La descripción no puede superar 255 caracteres.");

            return ResultadoBL.Ok("Datos de la especialidad válidos.");
        }

        // Inserta si Id == 0, actualiza si Id > 0
        public ResultadoBL Guardar(Especialidad e)
        {
            ResultadoBL validacion = Validar(e);
            if (!validacion.Exito) return validacion;

            e.Nombre = e.Nombre.Trim();
            e.Descripcion = (e.Descripcion ?? "").Trim();

            try
            {
                if (e.Id > 0)
                {
                    _dal.Actualizar(e);
                    return ResultadoBL.Ok("Especialidad actualizada correctamente.");
                }

                _dal.Insertar(e);
                return ResultadoBL.Ok("Especialidad guardada correctamente.");
            }
            catch (Exception ex)
            {
                return ResultadoBL.Error(ex.Message);
            }
        }

        public ResultadoBL Eliminar(int idEspecialidad)
        {
            if (idEspecialidad <= 0)
                return ResultadoBL.Error("Seleccione una especialidad de la lista primero.");

            try
            {
                _dal.Eliminar(idEspecialidad);
                return ResultadoBL.Ok("Especialidad eliminada correctamente.");
            }
            catch (Exception ex)
            {
                return ResultadoBL.Error(ex.Message);
            }
        }
    }
}