using System.Collections.Generic;

namespace Agenda_Medica.Entities
{
    public class Medico
    {
        public int Id { get; set; }                    // id_medico (0 = todavía no guardado)
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string HorarioAtencion { get; set; }

        // Relación muchos a muchos: un médico puede tener varias especialidades
        public List<Especialidad> Especialidades { get; set; } = new List<Especialidad>();

        public string NombreCompleto
        {
            get { return Nombre + " " + Apellidos; }
        }

        // Para mostrar en la grilla: "Pediatría, Cardiología"
        public string EspecialidadesTexto
        {
            get
            {
                List<string> nombres = new List<string>();
                foreach (Especialidad e in Especialidades) nombres.Add(e.Nombre);
                return string.Join(", ", nombres);
            }
        }
    }
}