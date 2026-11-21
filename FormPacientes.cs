using System;
using System.Windows.Forms;
using Agenda_Medica.BL;
using Agenda_Medica.Entities;

namespace Agenda_Medica.UI
{
    public partial class FormPacientes : Form
    {
        
        private readonly PacienteBL _bl = new PacienteBL();
        private int _idEdicion = 0;   // 0 = paciente nuevo

        public FormPacientes()
        {
            InitializeComponent();
        }

        private void FormPacientes_Load(object sender, EventArgs e)
        {
            if (cmbGenero.Items.Count == 0)
                cmbGenero.Items.AddRange(new object[] { "Masculino", "Femenino", "Otro" });
            if (cmbTipoSangre.Items.Count == 0)
                cmbTipoSangre.Items.AddRange(new object[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" });

            cmbGenero.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoSangre.DropDownStyle = ComboBoxStyle.DropDownList;
            dtpFechaNacimiento.MaxDate = DateTime.Today;
        }

        // Utilidades de UI 
        private Paciente LeerFormulario()
        {
            return new Paciente
            {
                Id = _idEdicion,
                Cedula = txtID.Text,                       // txtID = cédula
                Nombre = txtNombre.Text,
                Apellidos = txtApellido.Text,
                FechaNacimiento = dtpFechaNacimiento.Value,
                Genero = cmbGenero.SelectedItem == null ? "" : cmbGenero.SelectedItem.ToString(),
                Telefono = txtTelefono.Text,
                Direccion = txtDireccion.Text,
                TipoSangre = cmbTipoSangre.SelectedItem == null ? "" : cmbTipoSangre.SelectedItem.ToString()
            };
        }

        private void MostrarPaciente(Paciente p)
        {
            _idEdicion = p.Id;
            txtID.Text = p.Cedula;
            txtNombre.Text = p.Nombre;
            txtApellido.Text = p.Apellidos;
            dtpFechaNacimiento.Value = p.FechaNacimiento > dtpFechaNacimiento.MaxDate
                ? dtpFechaNacimiento.MaxDate : p.FechaNacimiento;
            cmbGenero.SelectedIndex = cmbGenero.FindStringExact(p.Genero);
            txtTelefono.Text = p.Telefono;
            txtDireccion.Text = p.Direccion;
            cmbTipoSangre.SelectedIndex = cmbTipoSangre.FindStringExact(p.TipoSangre);
            txtID.Enabled = false;     // la cédula no se cambia mientras se edita
        }

        private void LimpiarCampos()
        {
            txtID.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            cmbGenero.SelectedIndex = -1;
            cmbTipoSangre.SelectedIndex = -1;
            dtpFechaNacimiento.Value = DateTime.Today;
            _idEdicion = 0;
            txtID.Enabled = true;
            txtID.Focus();
        }

        private void Mostrar(ResultadoBL r)
        {
            MessageBox.Show(r.Mensaje,
                r.Exito ? "Éxito" : "Aviso",
                MessageBoxButtons.OK,
                r.Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        // Eventos de los botones 
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            ResultadoBL r = _bl.Guardar(LeerFormulario());
            Mostrar(r);
            if (r.Exito) LimpiarCampos();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Paciente p;
            ResultadoBL r = _bl.BuscarPorCedula(txtID.Text, out p);
            if (r.Exito) MostrarPaciente(p);
            else Mostrar(r);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_idEdicion <= 0)
            {
                MessageBox.Show("Primero busque un paciente para editar", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show("Modifique los datos y presione GUARDAR", "Editar",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idEdicion <= 0)
            {
                MessageBox.Show("Busque un paciente primero", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmar = MessageBox.Show("¿Eliminar este paciente?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar != DialogResult.Yes) return;

            ResultadoBL r = _bl.Eliminar(_idEdicion);
            Mostrar(r);
            if (r.Exito) LimpiarCampos();
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