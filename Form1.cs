
using Agenda_Medica.BL;
using System;
using System.Windows.Forms;
namespace Agenda_Medica.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnPacientes_Click_1(object sender, EventArgs e)
        {
            FormPacientes formPacientes = new FormPacientes();
            formPacientes.Show();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMedicos_Click(object sender, EventArgs e)
        {
            new FormMedicos().ShowDialog();
        }

        private void btnEspecialidades_Click(object sender, EventArgs e)
        {
            new FormEspecialidades().ShowDialog();
        }

        private void btnCitas_Click(object sender, EventArgs e)
        {
            new FormCitas().ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Solo el administrador ve Médicos y Especialidades
            bool esAdmin = UsuarioBL.UsuarioActual.Rol == "Administrador";
            btnMedicos.Visible = esAdmin;
            btnEspecialidades.Visible = esAdmin;
        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            new FormHistorial().ShowDialog();
        }
    }
}
   
    

