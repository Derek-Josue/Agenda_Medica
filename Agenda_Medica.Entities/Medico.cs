namespace Agenda_Medica.Entities
{
    public class Medico
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public int IdEspecialidad { get; set; }
        public bool Activo { get; set; }

        public string NombreCompleto
        {
            get { return Nombre + " " + Apellidos; }
        }
    }
}