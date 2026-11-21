using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Agenda_Medica.BL;
using Agenda_Medica.Entities;

namespace Agenda_Medica.UI
{
    public partial class FormEspecialidades : Form
    {
        private readonly EspecialidadBL _bl = new EspecialidadBL();
        private int _idEdicion = 0;   // 0 = especialidad nueva

        public FormEspecialidades()
        {
            InitializeComponent();
        }

        private void FormEspecialidades_Load(object sender, EventArgs e)
        {
            dgvEspecialidades.ReadOnly = true;
            dgvEspecialidades.AllowUserToAddRows = false;
            dgvEspecialidades.AllowUserToDeleteRows = false;
            dgvEspecialidades.MultiSelect = false;
            dgvEspecialidades.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEspecialidades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            CargarGrilla();
        }

        // Utilidades de UI 
        private void CargarGrilla()
        {
            List<Especialidad> lista;
            ResultadoBL r = _bl.Listar(out lista);
            if (!r.Exito)
            {
                Mostrar(r);
                return;
            }

            dgvEspecialidades.DataSource = null;
            dgvEspecialidades.DataSource = lista;

            if (dgvEspecialidades.Columns["Id"] != null)
                dgvEspecialidades.Columns["Id"].Visible = false;
            dgvEspecialidades.ClearSelection();
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
            _idEdicion = 0;
            dgvEspecialidades.ClearSelection();
            txtNombre.Focus();
        }

        private void Mostrar(ResultadoBL r)
        {
            MessageBox.Show(r.Mensaje,
                r.Exito ? "Éxito" : "Aviso",
                MessageBoxButtons.OK,
                r.Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        // Eventos 
        // Al hacer clic en una fila, sus datos pasan a los cuadros de texto para editar
        private void dgvEspecialidades_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            Especialidad esp = dgvEspecialidades.Rows[e.RowIndex].DataBoundItem as Especialidad;
            if (esp == null) return;

            _idEdicion = esp.Id;
            txtNombre.Text = esp.Nombre;
            txtDescripcion.Text = esp.Descripcion;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Especialidad esp = new Especialidad
            {
                Id = _idEdicion,
                Nombre = txtNombre.Text,
                Descripcion = txtDescripcion.Text
            };

            ResultadoBL r = _bl.Guardar(esp);
            Mostrar(r);
            if (r.Exito)
            {
                LimpiarCampos();
                CargarGrilla();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idEdicion <= 0)
            {
                MessageBox.Show("Seleccione una especialidad de la lista primero", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmar = MessageBox.Show("¿Eliminar esta especialidad?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar != DialogResult.Yes) return;

            ResultadoBL r = _bl.Eliminar(_idEdicion);
            Mostrar(r);
            if (r.Exito)
            {
                LimpiarCampos();
                CargarGrilla();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}