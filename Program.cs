using Agenda_Medica;
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
            Application.Run(new FormPrueba()); // Cambia a FormPrueba para probar la conexión
        }
    }
}