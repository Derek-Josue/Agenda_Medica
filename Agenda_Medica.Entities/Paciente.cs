using System;

namespace Agenda_Medica.Entities
{
    public class Paciente
    {
        public int Id { get; set; }                  // id_paciente (0 = todavía no guardado)
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string TipoSangre { get; set; }

        public string NombreCompleto
        {
            get { return Nombre + " " + Apellidos; }
        }
    }
}