using Agenda_Medica;
using Agenda_Medica.UI;
using System;
using System.Windows.Forms;

namespace Menu_Principall
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            //Application.Run(new FormPacientes());
            //Application.Run(new FormEspecialidades());
            //Application.Run(new FormMedicos());
            Application.Run(new FormCitas());
            //Application.Run(new FormPrueba()); // Cambia a FormPrueba para probar la conexión
        }
    }
}