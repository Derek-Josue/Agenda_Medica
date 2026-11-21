using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Agenda_Medica.BL;
using Agenda_Medica.Entities;

namespace Agenda_Medica.UI
{
    public partial class FormMedicos : Form
    {
        private readonly MedicoBL _blMedico = new MedicoBL();
        private readonly EspecialidadBL _blEspecialidad = new EspecialidadBL();
        private int _idEdicion = 0;   // 0 = médico nuevo

        public FormMedicos()
        {
            InitializeComponent();
        }

        private void FormMedicos_Load(object sender, EventArgs e)
        {
            dgvMedicos.ReadOnly = true;
            dgvMedicos.AllowUserToAddRows = false;
            dgvMedicos.AllowUserToDeleteRows = false;
            dgvMedicos.MultiSelect = false;
            dgvMedicos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMedicos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            clbEspecialidades.CheckOnClick = true;

            CargarEspecialidades();
            CargarGrilla();
        }

        // Carga de datos 
        private void CargarEspecialidades()
        {
            List<Especialidad> lista;
            ResultadoBL r = _blEspecialidad.Listar(out lista);
            if (!r.Exito)
            {
                Mostrar(r);
                return;
            }

            clbEspecialidades.Items.Clear();
            foreach (Especialidad esp in lista)
                clbEspecialidades.Items.Add(esp);     // se muestra con Especialidad.ToString() = nombre
        }

        private void CargarGrilla()
        {
            List<Medico> lista;
            ResultadoBL r = _blMedico.Listar(out lista);
            if (!r.Exito)
            {
                Mostrar(r);
                return;
            }

            dgvMedicos.DataSource = null;
            dgvMedicos.DataSource = lista;

            OcultarColumna("Id");
            OcultarColumna("Nombre");
            OcultarColumna("Apellidos");
            OcultarColumna("Especialidades");     // la lista interna; se muestra EspecialidadesTexto

            TituloColumna("Cedula", "Cédula");
            TituloColumna("NombreCompleto", "Médico");
            TituloColumna("Telefono", "Teléfono");
            TituloColumna("HorarioAtencion", "Horario");
            TituloColumna("EspecialidadesTexto", "Especialidades");

            dgvMedicos.ClearSelection();
        }

        private void OcultarColumna(string nombre)
        {
            if (dgvMedicos.Columns[nombre] != null)
                dgvMedicos.Columns[nombre].Visible = false;
        }

        private void TituloColumna(string nombre, string titulo)
        {
            if (dgvMedicos.Columns[nombre] != null)
                dgvMedicos.Columns[nombre].HeaderText = titulo;
        }

        //  Utilidades de UI 
        private Medico LeerFormulario()
        {
            Medico m = new Medico
            {
                Id = _idEdicion,
                Cedula = txtCedula.Text,
                Nombre = txtNombre.Text,
                Apellidos = txtApellidos.Text,
                Telefono = txtTelefono.Text,
                Correo = txtCorreo.Text,
                HorarioAtencion = txtHorario.Text
            };

            foreach (object item in clbEspecialidades.CheckedItems)
                m.Especialidades.Add((Especialidad)item);

            return m;
        }

        private void LimpiarCampos()
        {
            txtCedula.Clear();
            txtNombre.Clear();
            txtApellidos.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            txtHorario.Clear();

            for (int i = 0; i < clbEspecialidades.Items.Count; i++)
                clbEspecialidades.SetItemChecked(i, false);

            _idEdicion = 0;
            dgvMedicos.ClearSelection();
            txtCedula.Focus();
        }

        private void Mostrar(ResultadoBL r)
        {
            MessageBox.Show(r.Mensaje,
                r.Exito ? "Éxito" : "Aviso",
                MessageBoxButtons.OK,
                r.Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        //  Eventos 
        // Al hacer clic en una fila, sus datos pasan al formulario para editar
        private void dgvMedicos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            Medico m = dgvMedicos.Rows[e.RowIndex].DataBoundItem as Medico;
            if (m == null) return;

            _idEdicion = m.Id;
            txtCedula.Text = m.Cedula;
            txtNombre.Text = m.Nombre;
            txtApellidos.Text = m.Apellidos;
            txtTelefono.Text = m.Telefono;
            txtCorreo.Text = m.Correo;
            txtHorario.Text = m.HorarioAtencion;

            // Marcar solo las especialidades que tiene este médico
            for (int i = 0; i < clbEspecialidades.Items.Count; i++)
            {
                Especialidad esp = (Especialidad)clbEspecialidades.Items[i];
                clbEspecialidades.SetItemChecked(i, m.Especialidades.Any(x => x.Id == esp.Id));
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            ResultadoBL r = _blMedico.Guardar(LeerFormulario());
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
                MessageBox.Show("Seleccione un médico de la lista primero", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmar = MessageBox.Show("¿Eliminar este médico?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar != DialogResult.Yes) return;

            ResultadoBL r = _blMedico.Eliminar(_idEdicion);
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