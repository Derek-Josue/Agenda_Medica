// Proyecto: Agenda_Medica.BL
// Reglas de negocio de las citas. No usan base de datos: solo reciben valores y devuelven true/false.
using System;

namespace Agenda_Medica.BL
{
    public static class ReglasCita
    {
        // Regla: la duración solo puede ser 30, 45 o 60 minutos
        public static bool DuracionValida(int minutos)
        {
            return minutos == 30 || minutos == 45 || minutos == 60;
        }

        public static DateTime CalcularFin(DateTime inicio, int minutos)
        {
            return inicio.AddMinutes(minutos);
        }

        // Regla: dos citas chocan si una empieza antes de que termine la otra, y viceversa.
        // Sirve tanto para el médico como para el paciente.
        public static bool Solapan(DateTime inicioA, DateTime finA, DateTime inicioB, DateTime finB)
        {
            return inicioA < finB && inicioB < finA;
        }

        // Regla: la cita debe caber completa dentro del bloque de horario del médico
        public static bool DentroDeHorario(TimeSpan horaInicio, TimeSpan horaFin,
                                           TimeSpan bloqueInicio, TimeSpan bloqueFin)
        {
            return horaInicio >= bloqueInicio && horaFin <= bloqueFin;
        }
    }
}
