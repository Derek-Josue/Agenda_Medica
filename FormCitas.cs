using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Agenda_Medica.BL;
using Agenda_Medica.Entities;

namespace Agenda_Medica.UI
{
    public partial class FormCitas : Form
    {
        private readonly CitaBL _blCita = new CitaBL();
        private readonly MedicoBL _blMedico = new MedicoBL();
        private readonly PacienteBL _blPaciente = new PacienteBL();

        private Cita _citaActual = null;   // null = cita nueva

        public FormCitas()
        {
            InitializeComponent();
        }

        private void FormCitas_Load(object sender, EventArgs e)
        {
            dgvCitas.ReadOnly = true;
            dgvCitas.AllowUserToAddRows = false;
            dgvCitas.AllowUserToDeleteRows = false;
            dgvCitas.MultiSelect = false;
            dgvCitas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCitas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            cmbMedico.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPaciente.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDuracion.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModalidad.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltroMedico.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbDuracion.Items.AddRange(new object[] { "30", "45", "60" });
            foreach (string modalidad in ModalidadesCita.Todas)
                cmbModalidad.Items.Add(modalidad);

            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpHora.Format = DateTimePickerFormat.Time;
            dtpHora.ShowUpDown = true;
            dtpFiltroFecha.Format = DateTimePickerFormat.Short;
            dtpFiltroFecha.Value = DateTime.Today;
            chkFiltrarFecha.Checked = false;

            CargarCombos();
            LimpiarCampos();
            CargarGrilla();
        }

        // Carga de datos 
        private void CargarCombos()
        {
          
            List<Medico> medicos;
            ResultadoBL r = _blMedico.Listar(out medicos);
            if (!r.Exito)
            {
                Mostrar(r);
                return;
            }

            cmbMedico.DisplayMember = "NombreCompleto";
            cmbMedico.ValueMember = "Id";
            cmbMedico.DataSource = new List<Medico>(medicos);

            List<Medico> paraFiltro = new List<Medico>(medicos);
            paraFiltro.Insert(0, new Medico { Id = 0, Nombre = "(Todos los médicos)", Apellidos = "" });
            cmbFiltroMedico.DisplayMember = "NombreCompleto";
            cmbFiltroMedico.ValueMember = "Id";
            cmbFiltroMedico.DataSource = paraFiltro;

            // Pacientes:"cédula - nombre apellidos"
            try
            {
                DataTable pacientes = _blPaciente.ListarPacientes();
                pacientes.Columns.Add("display", typeof(string),
                    "cedula + ' - ' + nombre + ' ' + apellidos");

                cmbPaciente.DisplayMember = "display";
                cmbPaciente.ValueMember = "id_paciente";
                cmbPaciente.DataSource = pacientes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CargarGrilla()
        {
            DateTime? fecha = chkFiltrarFecha.Checked ? dtpFiltroFecha.Value.Date : (DateTime?)null;
            int idMedico = cmbFiltroMedico.SelectedValue == null ? 0 : Convert.ToInt32(cmbFiltroMedico.SelectedValue);

            List<Cita> lista;
            ResultadoBL r = _blCita.Listar(fecha, idMedico, 0, out lista);
            if (!r.Exito)
            {
                Mostrar(r);
                return;
            }

            dgvCitas.DataSource = null;
            dgvCitas.DataSource = lista;

            OcultarColumna("Id");
            OcultarColumna("IdMedico");
            OcultarColumna("IdPaciente");

            TituloColumna("MedicoNombre", "Médico");
            TituloColumna("PacienteNombre", "Paciente");
            TituloColumna("Inicio", "Inicio");
            TituloColumna("Fin", "Fin");
            TituloColumna("Duracion", "Min");

            if (dgvCitas.Columns["Inicio"] != null)
                dgvCitas.Columns["Inicio"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            if (dgvCitas.Columns["Fin"] != null)
                dgvCitas.Columns["Fin"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

            dgvCitas.ClearSelection();
        }

        private void OcultarColumna(string nombre)
        {
            if (dgvCitas.Columns[nombre] != null)
                dgvCitas.Columns[nombre].Visible = false;
        }

        private void TituloColumna(string nombre, string titulo)
        {
            if (dgvCitas.Columns[nombre] != null)
                dgvCitas.Columns[nombre].HeaderText = titulo;
        }

        // Utilidades de UI 
        private Cita LeerFormulario()
        {
            DateTime fecha = dtpFecha.Value;
            DateTime hora = dtpHora.Value;

            int duracion;
            int.TryParse(Convert.ToString(cmbDuracion.SelectedItem), out duracion);

            return new Cita
            {
                Id = _citaActual == null ? 0 : _citaActual.Id,
                IdMedico = cmbMedico.SelectedValue == null ? 0 : Convert.ToInt32(cmbMedico.SelectedValue),
                IdPaciente = cmbPaciente.SelectedValue == null ? 0 : Convert.ToInt32(cmbPaciente.SelectedValue),
                Inicio = new DateTime(fecha.Year, fecha.Month, fecha.Day, hora.Hour, hora.Minute, 0),
                Duracion = duracion,
                Modalidad = cmbModalidad.SelectedItem == null ? "" : cmbModalidad.SelectedItem.ToString(),
                Estado = _citaActual == null ? EstadosCita.Programada : _citaActual.Estado   // la BL decide el estado final
            };
        }

        private void LimpiarCampos()
        {
            _citaActual = null;
            cmbMedico.SelectedIndex = -1;
            cmbPaciente.SelectedIndex = -1;
            dtpFecha.Value = DateTime.Today;
            dtpHora.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 8, 0, 0);
            cmbDuracion.SelectedIndex = 0;
            cmbModalidad.SelectedIndex = 0;
            dgvCitas.ClearSelection();
        }

        private void Mostrar(ResultadoBL r)
        {
            MessageBox.Show(r.Mensaje,
                r.Exito ? "Éxito" : "Aviso",
                MessageBoxButtons.OK,
                r.Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        private void CambiarEstado(string nuevoEstado)
        {
            if (_citaActual == null)
            {
                MessageBox.Show("Seleccione una cita de la lista primero", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmar = MessageBox.Show(
                "¿Cambiar el estado de la cita a \"" + nuevoEstado + "\"?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar != DialogResult.Yes) return;

            ResultadoBL r = _blCita.CambiarEstado(_citaActual.Id, nuevoEstado);
            Mostrar(r);
            if (r.Exito)
            {
                LimpiarCampos();
                CargarGrilla();
            }
        }

        // Eventos 
        // Al hacer clic en una fila, sus datos pasan al formulario
        private void dgvCitas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            Cita c = dgvCitas.Rows[e.RowIndex].DataBoundItem as Cita;
            if (c == null) return;

            _citaActual = c;
            cmbMedico.SelectedValue = c.IdMedico;
            cmbPaciente.SelectedValue = c.IdPaciente;
            dtpFecha.Value = c.Inicio.Date;
            dtpHora.Value = c.Inicio;
            cmbDuracion.SelectedIndex = cmbDuracion.FindStringExact(c.Duracion.ToString());
            cmbModalidad.SelectedIndex = cmbModalidad.FindStringExact(c.Modalidad);
        }
        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            ResultadoBL r = _blCita.Guardar(LeerFormulario());
            Mostrar(r);
            if (r.Exito)
            {
                LimpiarCampos();
                CargarGrilla();
            }
        }
        private void btnConfirmar_Click_1(object sender, EventArgs e)
        {
            CambiarEstado(EstadosCita.Confirmada);
        }
        private void btnAtendida_Click_1(object sender, EventArgs e)
        {
            CambiarEstado(EstadosCita.Atendida);
        }
        private void btnNoAsistio_Click_1(object sender, EventArgs e)
        {
            CambiarEstado(EstadosCita.NoAsistio);
        }

        private void btnReprogramar_Click(object sender, EventArgs e)
        {

            if (_citaActual == null)
            {
                MessageBox.Show("Seleccione una cita de la lista primero", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmar = MessageBox.Show(
                "¿Reprogramar la cita con la nueva fecha, hora y duración seleccionadas?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar != DialogResult.Yes) return;

            Cita citaReprogramada = LeerFormulario();
            citaReprogramada.Id = _citaActual.Id;

            // Si tu CitaBL tiene un método específico (ej. Reprogramar), úsalo aquí;
            // de lo contrario, Guardar con el Id existente actualiza la cita.
            ResultadoBL r = _blCita.Guardar(citaReprogramada);
            Mostrar(r);

            if (r.Exito)
            {
                LimpiarCampos();
                CargarGrilla();
            }
        }
        private void btnCancelarCita_Click_1(object sender, EventArgs e)
        {
            CambiarEstado(EstadosCita.Cancelada);
        }

        private void btnFiltrar_Click_1(object sender, EventArgs e)
        {
            CargarGrilla();
        }


        private void btnQuitarFiltros_Click_1(object sender, EventArgs e)
        {
            chkFiltrarFecha.Checked = false;
            cmbFiltroMedico.SelectedIndex = 0;
            CargarGrilla();
        }


        private void btnLimpiar_Click_1(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnVolver_Click_1(object sender, EventArgs e)
        {
        this.Close();
        }

       
    }
    
}