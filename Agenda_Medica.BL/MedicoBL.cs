// Proyecto: Agenda_Medica.BL  (usa la entidad Medico de Agenda_Medica.Entities)
using Agenda_Medica.Entities;

namespace Agenda_Medica.BL
{
    public class MedicoBL
    {
        // Revisa los datos del médico antes de que la DAL los guarde
        public ResultadoBL Validar(Medico m)
        {
            if (m == null) return ResultadoBL.Error("Los datos del médico no pueden estar vacíos.");

            string error =
                Validaciones.Obligatorio(m.Nombre, "nombre", 50) ??
                Validaciones.Obligatorio(m.Apellidos, "apellidos", 80) ??
                Validaciones.Telefono(m.Telefono) ??
                Validaciones.Correo(m.Correo);

            if (error != null) return ResultadoBL.Error(error);

            if (m.IdEspecialidad <= 0)
                return ResultadoBL.Error("Debe seleccionar una especialidad.");

            return ResultadoBL.Ok("Datos del médico válidos.");
        }
    }
}
