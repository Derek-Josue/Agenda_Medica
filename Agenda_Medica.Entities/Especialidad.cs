namespace Agenda_Medica.Entities
{
    public class Especialidad
    {
        public int Id { get; set; }              // id_especialidad (0 = todavía no guardada)
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        // Útil para mostrar la especialidad directamente en un ComboBox (módulo Médicos)
        public override string ToString()
        {
            return Nombre;
        }
    }
}