// Proyecto: Agenda_Medica.BL  (necesita referencia a Agenda_Medica.DAL)
using System.Data;
using Agenda_Medica.DAL;

namespace Agenda_Medica.BL
{
    public class PacienteBL
    {
        private readonly PacienteDAL _dal = new PacienteDAL();

        // Conexión BL -> DAL: la UI llama aquí y nunca a PacienteDAL directamente
        public DataTable ListarPacientes()
        {
            return _dal.ListarPacientes();
        }

        // Revisa los datos del paciente. Usa parámetros porque aún no hay entidad Paciente.
        public ResultadoBL Validar(string nombre, string apellidos, string telefono, string correo)
        {
            string error =
                Validaciones.Obligatorio(nombre, "nombre", 50) ??
                Validaciones.Obligatorio(apellidos, "apellidos", 80) ??
                Validaciones.Telefono(telefono) ??
                Validaciones.Correo(correo);

            if (error != null) return ResultadoBL.Error(error);
            return ResultadoBL.Ok("Datos del paciente válidos.");
        }
    }
}
