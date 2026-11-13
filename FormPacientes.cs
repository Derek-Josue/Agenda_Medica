using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Menu_Principall
{
    public partial class FormPacientes : Form
    {
        private const string ConnectionString =
            "Server=localhost;" +
            "Port=3306;" +
            "Database=agenda_medica;" +
            "Uid=root;" +
            "Pwd=tNx92mlAw.;";

        // id_paciente del registro que se está editando (0 = paciente nuevo)
        private int idPacienteEdicion = 0;

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

        private void LimpiarCampos()
        {
            txtID.Clear();          // txtID = cédula
            txtNombre.Clear();
            txtApellido.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            cmbGenero.SelectedIndex = -1;
            cmbTipoSangre.SelectedIndex = -1;
            dtpFechaNacimiento.Value = DateTime.Today;
            idPacienteEdicion = 0;
            txtID.Enabled = true;
            txtID.Focus();
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Escriba la cédula del paciente", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtID.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Escriba el nombre", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("Escriba los apellidos", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return false;
            }
            if (cmbGenero.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione el género", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbGenero.Focus();
                return false;
            }
            if (cmbTipoSangre.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione el tipo de sangre", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTipoSangre.Focus();
                return false;
            }
            return true;
        }

        private static void SeleccionarItem(ComboBox combo, string valor)
        {
            int i = combo.FindStringExact(valor);
            combo.SelectedIndex = i;
        }

        // ---------------------------------------------------------------
        // GUARDAR (INSERT o UPDATE)
        // ---------------------------------------------------------------

        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            bool esEdicion = idPacienteEdicion > 0;

            // CORRECCIÓN: Se cambió 'pacientes' por 'paciente' (nombre real de la tabla)
            string sql = esEdicion
                ? @"UPDATE paciente
                       SET cedula = @cedula, nombre = @nombre, apellidos = @apellidos,
                           fecha_nacimiento = @fecha, genero = @genero, telefono = @telefono,
                           direccion = @direccion, tipo_sangre = @sangre
                     WHERE id_paciente = @id"
                : @"INSERT INTO paciente
                       (cedula, nombre, apellidos, fecha_nacimiento, genero, telefono, direccion, tipo_sangre)
                    VALUES
                       (@cedula, @nombre, @apellidos, @fecha, @genero, @telefono, @direccion, @sangre)";

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cedula", txtID.Text.Trim());
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@apellidos", txtApellido.Text.Trim());
                    cmd.Parameters.AddWithValue("@fecha", dtpFechaNacimiento.Value.Date);
                    cmd.Parameters.AddWithValue("@genero", cmbGenero.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@telefono", txtTelefono.Text.Trim());
                    cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text.Trim());
                    cmd.Parameters.AddWithValue("@sangre", cmbTipoSangre.SelectedItem.ToString());

                    if (esEdicion)
                        cmd.Parameters.AddWithValue("@id", idPacienteEdicion);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show(
                    esEdicion ? "Paciente actualizado correctamente" : "Paciente guardado correctamente",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                MessageBox.Show("Ya existe un paciente con esa cédula", "Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error de base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBusacar_Click(object sender, EventArgs e)
        {
             // ---------------------------------------------------------------
             // BUSCAR por cédula
             // ---------------------------------------------------------------
        
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Escriba una cédula para buscar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // CORRECCIÓN: Se cambió 'pacientes' por 'paciente'
            const string sql = @"SELECT id_paciente, cedula, nombre, apellidos, fecha_nacimiento,
                                        genero, telefono, direccion, tipo_sangre
                                   FROM paciente
                                  WHERE cedula = @cedula
                                  LIMIT 1";
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cedula", txtID.Text.Trim());
                    conn.Open();

                    using (var rd = cmd.ExecuteReader())
                    {
                        if (!rd.Read())
                        {
                            MessageBox.Show("Paciente no encontrado", "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        idPacienteEdicion = rd.GetInt32("id_paciente");
                        txtID.Text = rd.GetString("cedula");
                        txtNombre.Text = rd.GetString("nombre");
                        txtApellido.Text = rd.GetString("apellidos");
                        dtpFechaNacimiento.Value = rd.IsDBNull(rd.GetOrdinal("fecha_nacimiento"))
                            ? DateTime.Today
                            : rd.GetDateTime("fecha_nacimiento");
                        SeleccionarItem(cmbGenero, rd.IsDBNull(rd.GetOrdinal("genero")) ? "" : rd.GetString("genero"));
                        txtTelefono.Text = rd.IsDBNull(rd.GetOrdinal("telefono")) ? "" : rd.GetString("telefono");
                        txtDireccion.Text = rd.IsDBNull(rd.GetOrdinal("direccion")) ? "" : rd.GetString("direccion");
                        SeleccionarItem(cmbTipoSangre, rd.IsDBNull(rd.GetOrdinal("tipo_sangre")) ? "" : rd.GetString("tipo_sangre"));

                        txtID.Enabled = false;
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error de base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
        }

        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            if (idPacienteEdicion <= 0)
            {
                MessageBox.Show("Primero busque un paciente para editar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show("Modifique los datos y presione GUARDAR", "Editar", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {

            if (idPacienteEdicion <= 0)
            {
                MessageBox.Show("Busque un paciente primero", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmar = MessageBox.Show("¿Eliminar este paciente?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar != DialogResult.Yes) return;

            try
            {
                // CORRECCIÓN: Se cambió 'pacientes' por 'paciente'
                using (var conn = new MySqlConnection(ConnectionString))
                using (var cmd = new MySqlCommand("DELETE FROM paciente WHERE id_paciente = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", idPacienteEdicion);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Paciente eliminado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
            }
            catch (MySqlException ex) when (ex.Number == 1451)
            {
                MessageBox.Show("No se puede eliminar: el paciente tiene registros asociados (por ejemplo, citas).",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error de base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click_1(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}