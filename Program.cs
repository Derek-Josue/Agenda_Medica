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

            FormLogin login = new FormLogin();
            if (login.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new Form1());
            }
           
        }
    }
}