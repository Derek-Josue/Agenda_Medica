// Convierte el texto de medico.horario_atencion (por ejemplo "Lunes a Viernes 8:00-16:00")
// en bloques que se pueden comparar contra una cita. No usa base de datos.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Agenda_Medica.BL
{
    public static class HorarioMedico
    {
        public class Bloque
        {
            public List<DayOfWeek> Dias { get; set; } = new List<DayOfWeek>();
            public TimeSpan Inicio { get; set; }
            public TimeSpan Fin { get; set; }
        }

        public const string Ejemplo = "Lunes a Viernes 8:00-16:00; Sabado 8:00-12:00";

        private static readonly string[] NombresDias =
            { "lunes", "martes", "miercoles", "jueves", "viernes", "sabado", "domingo" };

        private static readonly DayOfWeek[] DiasSemana =
        {
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
            DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
        };

        // "<Dia>[ a <Dia>] H:mm-H:mm"
        private static readonly Regex Patron = new Regex(
            @"^\s*([^\W\d_]+)(?:\s+a\s+([^\W\d_]+))?\s+(\d{1,2}):(\d{2})\s*-\s*(\d{1,2}):(\d{2})\s*$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Varios bloques se separan con punto y coma:
        //   "Lunes a Viernes 8:00-12:00; Lunes a Viernes 13:00-17:00"
        public static bool TryParse(string texto, out List<Bloque> bloques, out string error)
        {
            bloques = new List<Bloque>();
            error = null;

            if (string.IsNullOrWhiteSpace(texto))
            {
                error = "El horario de atención es obligatorio. Ejemplo: " + Ejemplo;
                return false;
            }

            foreach (string parte in texto.Split(';'))
            {
                if (string.IsNullOrWhiteSpace(parte)) continue;

                Match m = Patron.Match(parte);
                if (!m.Success)
                {
                    error = "El horario \"" + parte.Trim() + "\" no tiene un formato válido. Ejemplo: " + Ejemplo;
                    return false;
                }

                int dia1 = IndiceDia(m.Groups[1].Value);
                int dia2 = m.Groups[2].Success ? IndiceDia(m.Groups[2].Value) : dia1;
                if (dia1 < 0 || dia2 < 0)
                {
                    error = "Día no reconocido en \"" + parte.Trim() +
                            "\". Use: Lunes, Martes, Miercoles, Jueves, Viernes, Sabado o Domingo.";
                    return false;
                }

                int h1 = int.Parse(m.Groups[3].Value), m1 = int.Parse(m.Groups[4].Value);
                int h2 = int.Parse(m.Groups[5].Value), m2 = int.Parse(m.Groups[6].Value);
                if (h1 > 23 || h2 > 23 || m1 > 59 || m2 > 59)
                {
                    error = "Hora inválida en \"" + parte.Trim() + "\".";
                    return false;
                }

                TimeSpan inicio = new TimeSpan(h1, m1, 0);
                TimeSpan fin = new TimeSpan(h2, m2, 0);
                if (inicio >= fin)
                {
                    error = "En \"" + parte.Trim() + "\" la hora de inicio debe ser menor que la de fin.";
                    return false;
                }

                Bloque b = new Bloque { Inicio = inicio, Fin = fin };
                for (int i = dia1; ; i = (i + 1) % 7)       // "Viernes a Lunes" también funciona
                {
                    b.Dias.Add(DiasSemana[i]);
                    if (i == dia2) break;
                }
                bloques.Add(b);
            }

            if (bloques.Count == 0)
            {
                error = "El horario de atención es obligatorio. Ejemplo: " + Ejemplo;
                return false;
            }
            return true;
        }

        // ¿La cita cabe completa en alguno de los bloques del médico ese día?
        public static bool Permite(List<Bloque> bloques, DateTime inicio, DateTime fin)
        {
            if (inicio.Date != fin.Date) return false;      // una cita no puede cruzar la medianoche

            foreach (Bloque b in bloques)
            {
                if (b.Dias.Contains(inicio.DayOfWeek) &&
                    ReglasCita.DentroDeHorario(inicio.TimeOfDay, fin.TimeOfDay, b.Inicio, b.Fin))
                    return true;
            }
            return false;
        }

        private static int IndiceDia(string nombre)
        {
            return Array.IndexOf(NombresDias, SinTildes(nombre).ToLowerInvariant());
        }

        private static string SinTildes(string texto)
        {
            string descompuesto = texto.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            foreach (char c in descompuesto)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
