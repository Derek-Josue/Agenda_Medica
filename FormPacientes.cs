using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Menu_Principall
{
    public partial class FormPacientes : Form
    {
        private List<Paciente> listaPacientes = new List<Paciente>();
        private int indiceEdicion = -1;

        public FormPacientes()
        {
            InitializeComponent();
        }

        public class Paciente
        {
            public string ID { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Telefono { get; set; }
            public string Direccion { get; set; }
        }

        private void LimpiarCampos()
        {
            txtID.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            indiceEdicion = -1;
            txtID.Enabled = true;
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Escriba el ID del paciente", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("Escriba el apellido", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return false;
            }
            return true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            if (indiceEdicion >= 0)
            {
                listaPacientes[indiceEdicion].ID = txtID.Text;
                listaPacientes[indiceEdicion].Nombre = txtNombre.Text;
                listaPacientes[indiceEdicion].Apellido = txtApellido.Text;
                listaPacientes[indiceEdicion].Telefono = txtTelefono.Text;
                listaPacientes[indiceEdicion].Direccion = txtDireccion.Text;
                MessageBox.Show("Paciente actualizado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Paciente nuevo = new Paciente
                {
                    ID = txtID.Text,
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Telefono = txtTelefono.Text,
                    Direccion = txtDireccion.Text
                };
                listaPacientes.Add(nuevo);
                MessageBox.Show("Paciente guardado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            LimpiarCampos();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Escriba un ID para buscar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (Paciente p in listaPacientes)
            {
                if (p.ID == txtID.Text)
                {
                    txtNombre.Text = p.Nombre;
                    txtApellido.Text = p.Apellido;
                    txtTelefono.Text = p.Telefono;
                    txtDireccion.Text = p.Direccion;
                    indiceEdicion = listaPacientes.IndexOf(p);
                    txtID.Enabled = false;
                    return;
                }
            }
            MessageBox.Show("Paciente no encontrado", "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Primero busque un paciente para editar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show("Modifique los datos y presione GUARDAR", "Editar", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Busque un paciente primero", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (Paciente p in listaPacientes)
            {
                if (p.ID == txtID.Text)
                {
                    DialogResult confirmar = MessageBox.Show("¿Eliminar este paciente?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirmar == DialogResult.Yes)
                    {
                        listaPacientes.Remove(p);
                        MessageBox.Show("Paciente eliminado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                    }
                    return;
                }
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

        private void btnBusacar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Escriba un ID para buscar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (Paciente p in listaPacientes)
            {
                if (p.ID == txtID.Text)
                {
                    txtNombre.Text = p.Nombre;
                    txtApellido.Text = p.Apellido;
                    txtTelefono.Text = p.Telefono;
                    txtDireccion.Text = p.Direccion;
                    indiceEdicion = listaPacientes.IndexOf(p);
                    txtID.Enabled = false;
                    return;
                }
            }
            MessageBox.Show("Paciente no encontrado", "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Primero busque un paciente para editar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show("Modifique los datos y presione GUARDAR", "Editar", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Busque un paciente primero", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (Paciente p in listaPacientes)
            {
                if (p.ID == txtID.Text)
                {
                    DialogResult confirmar = MessageBox.Show("¿Eliminar este paciente?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirmar == DialogResult.Yes)
                    {
                        listaPacientes.Remove(p);
                        MessageBox.Show("Paciente eliminado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                    }
                    return;
                }
            }
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
