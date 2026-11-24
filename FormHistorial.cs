using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Agenda_Medica.BL;
using Agenda_Medica.Entities;

namespace Agenda_Medica.UI
{
    public partial class FormHistorial : Form
    {
        private TextBox txtIdPaciente;
        private Button btnBuscar;
        private Button btnCerrar;
        private DataGridView dgvHistorial;

        public FormHistorial()
        {
            ArmarControles();
        }

        private void ArmarControles()
        {
            this.Text = "Historial de citas";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new System.Drawing.Size(900, 450);

            var lbl = new Label { Text = "ID del paciente:", Left = 15, Top = 20, AutoSize = true };
            txtIdPaciente = new TextBox { Left = 130, Top = 17, Width = 120 };
            btnBuscar = new Button { Text = "Buscar", Left = 265, Top = 15, Width = 90 };
            btnCerrar = new Button { Text = "Cerrar", Left = 365, Top = 15, Width = 90 };

            dgvHistorial = new DataGridView
            {
                Left = 15,
                Top = 55,
                Width = 855,
                Height = 340,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnBuscar.Click += btnBuscar_Click;
            btnCerrar.Click += (s, e) => this.Close();

            this.Controls.Add(lbl);
            this.Controls.Add(txtIdPaciente);
            this.Controls.Add(btnBuscar);
            this.Controls.Add(btnCerrar);
            this.Controls.Add(dgvHistorial);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string identificacion = txtIdPaciente.Text.Trim();
            if (identificacion == "")
            {
                MessageBox.Show("Ingrese el ID del paciente");
                return;
            }

            List<Cita> lista;
            var r = new CitaBL().HistorialPaciente(identificacion, out lista);
            if (!r.Exito)
            {
                MessageBox.Show(r.Mensaje);
                return;
            }

            dgvHistorial.DataSource = lista;
            if (lista.Count == 0)
            {
                MessageBox.Show("Este paciente no tiene citas registradas");
                return;
            }

            dgvHistorial.Columns["Id"].HeaderText = "N° Cita";
            dgvHistorial.Columns["IdMedico"].Visible = false;
            dgvHistorial.Columns["IdPaciente"].Visible = false;
            dgvHistorial.Columns["MedicoNombre"].HeaderText = "Médico";
            dgvHistorial.Columns["PacienteNombre"].HeaderText = "Paciente";
            dgvHistorial.Columns["Duracion"].HeaderText = "Duración (min)";
        }
    }
}