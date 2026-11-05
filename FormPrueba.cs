using System;
using System.Windows.Forms;
using Agenda_Medica.DAL; // Namespace de tu clase Conexion

namespace Agenda_Medica
{
    public partial class FormPrueba : Form
    {
        public FormPrueba()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conexion = Conexion.ObtenerConexion())
                {
                    if (conexion != null && conexion.State == System.Data.ConnectionState.Open)
                    {
                        MessageBox.Show("¡Conexión exitosa a la base de datos agenda_medica!",
                                        "Prueba DAL",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar:\n\n" + ex.Message,
                                "Error de Conexión",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
    }
}