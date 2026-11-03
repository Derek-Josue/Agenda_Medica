// Proyecto: Agenda_Medica.BL
namespace Agenda_Medica.BL
{
    // Lo que la capa BL le devuelve a la UI: si salió bien y qué mensaje mostrar
    public class ResultadoBL
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }

        public static ResultadoBL Ok(string mensaje)
        {
            return new ResultadoBL { Exito = true, Mensaje = mensaje };
        }

        public static ResultadoBL Error(string mensaje)
        {
            return new ResultadoBL { Exito = false, Mensaje = mensaje };
        }
    }
}