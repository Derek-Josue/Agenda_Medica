using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Agenda_Medica
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Agenda_Medica.DAL.PacienteDAL pacienteDal = new Agenda_Medica.DAL.PacienteDAL();
                System.Data.DataTable dt = pacienteDal.ListarPacientes();

                // Asignamos la tabla al DataGridView
                dataGridView1.DataSource = dt;

                MessageBox.Show("¡Conexión exitosa! Los datos se cargaron correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
