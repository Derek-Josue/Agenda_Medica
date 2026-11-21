using System;

namespace Agenda_Medica.Entities
{
    // Los textos deben ser IGUALES a los valores del ENUM de la tabla cita
    public static class EstadosCita
    {
        public const string Programada = "Programada";
        public const string Confirmada = "Confirmada";
        public const string Atendida = "Atendida";
        public const string Cancelada = "Cancelada";
        public const string NoAsistio = "No asistio";
        public const string Reprogramada = "Reprogramada";

        public static readonly string[] Todos =
            { Programada, Confirmada, Atendida, Cancelada, NoAsistio, Reprogramada };
    }

    public static class ModalidadesCita
    {
        public const string Presencial = "Presencial";
        public const string Virtual = "Virtual";
        public const string Urgencia = "Urgencia";

        public static readonly string[] Todas = { Presencial, Virtual, Urgencia };
    }

    public class Cita
    {
        public int Id { get; set; }                  // id_cita (0 = todavía no guardada)
        public int IdMedico { get; set; }
        public int IdPaciente { get; set; }

        // Solo para mostrar en la grilla (vienen del JOIN)
        public string MedicoNombre { get; set; }
        public string PacienteNombre { get; set; }

        public DateTime Inicio { get; set; }         // fecha_hora_inicio
        public DateTime Fin { get; set; }            // fecha_hora_fin (la calcula la BL)
        public int Duracion { get; set; }            // minutos: 30, 45 o 60
        public string Modalidad { get; set; }
        public string Estado { get; set; }
    }
}