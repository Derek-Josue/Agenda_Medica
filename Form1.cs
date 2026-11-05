using System;
using System.Windows.Forms;
namespace Menu_Principall
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnPacientes_Click(object sender, EventArgs e)
        {
            FormPacientes formPacientes = new FormPacientes();
            formPacientes.Show();
        }
    }
}
