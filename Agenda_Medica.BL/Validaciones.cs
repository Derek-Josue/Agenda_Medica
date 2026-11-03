// Proyecto: Agenda_Medica.BL
// Validaciones reutilizables. Cada método devuelve el mensaje de error, o null si todo está bien.
using System.Text.RegularExpressions;

namespace Agenda_Medica.BL
{
    public static class Validaciones
    {
        public static string Obligatorio(string valor, string campo, int maximo)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return "El campo " + campo + " es obligatorio.";
            if (valor.Trim().Length > maximo)
                return "El campo " + campo + " no puede superar " + maximo + " caracteres.";
            return null;
        }

        // El correo es opcional; si viene, debe tener formato válido
        public static string Correo(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo)) return null;
            if (!Regex.IsMatch(correo.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return "El correo no tiene un formato válido.";
            return null;
        }

        // Teléfono: solo números (se ignoran espacios y guiones), entre 8 y 15 dígitos
        public static string Telefono(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                return "El teléfono es obligatorio.";
            string limpio = telefono.Replace(" ", "").Replace("-", "");
            if (!Regex.IsMatch(limpio, @"^\d{8,15}$"))
                return "El teléfono debe tener entre 8 y 15 dígitos y solo números.";
            return null;
        }
    }
}