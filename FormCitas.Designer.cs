namespace Agenda_Medica.UI
{
    partial class FormCitas
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbMedico = new System.Windows.Forms.ComboBox();
            this.cmbPaciente = new System.Windows.Forms.ComboBox();
            this.cmbDuracion = new System.Windows.Forms.ComboBox();
            this.cmbModalidad = new System.Windows.Forms.ComboBox();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.dtpHora = new System.Windows.Forms.DateTimePicker();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.btnAtendida = new System.Windows.Forms.Button();
            this.btnNoAsistio = new System.Windows.Forms.Button();
            this.btnCancelarCita = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnVolver = new System.Windows.Forms.Button();
            this.dgvCitas = new System.Windows.Forms.DataGridView();
            this.chkFiltrarFecha = new System.Windows.Forms.CheckBox();
            this.dtpFiltroFecha = new System.Windows.Forms.DateTimePicker();
            this.cmbFiltroMedico = new System.Windows.Forms.ComboBox();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.btnQuitarFiltros = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCitas)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbMedico
            // 
            this.cmbMedico.FormattingEnabled = true;
            this.cmbMedico.Location = new System.Drawing.Point(155, 88);
            this.cmbMedico.Name = "cmbMedico";
            this.cmbMedico.Size = new System.Drawing.Size(172, 24);
            this.cmbMedico.TabIndex = 0;
            // 
            // cmbPaciente
            // 
            this.cmbPaciente.FormattingEnabled = true;
            this.cmbPaciente.Location = new System.Drawing.Point(155, 120);
            this.cmbPaciente.Name = "cmbPaciente";
            this.cmbPaciente.Size = new System.Drawing.Size(172, 24);
            this.cmbPaciente.TabIndex = 1;
            // 
            // cmbDuracion
            // 
            this.cmbDuracion.FormattingEnabled = true;
            this.cmbDuracion.Location = new System.Drawing.Point(155, 158);
            this.cmbDuracion.Name = "cmbDuracion";
            this.cmbDuracion.Size = new System.Drawing.Size(121, 24);
            this.cmbDuracion.TabIndex = 2;
            // 
            // cmbModalidad
            // 
            this.cmbModalidad.FormattingEnabled = true;
            this.cmbModalidad.Location = new System.Drawing.Point(155, 196);
            this.cmbModalidad.Name = "cmbModalidad";
            this.cmbModalidad.Size = new System.Drawing.Size(121, 24);
            this.cmbModalidad.TabIndex = 3;
            // 
            // dtpFecha
            // 
            this.dtpFecha.Location = new System.Drawing.Point(348, 86);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(200, 22);
            this.dtpFecha.TabIndex = 4;
            // 
            // dtpHora
            // 
            this.dtpHora.Location = new System.Drawing.Point(348, 141);
            this.dtpHora.Name = "dtpHora";
            this.dtpHora.Size = new System.Drawing.Size(200, 22);
            this.dtpHora.TabIndex = 5;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(53, 347);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(94, 23);
            this.btnGuardar.TabIndex = 6;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click_1);
            // 
            // btnConfirmar
            // 
            this.btnConfirmar.Location = new System.Drawing.Point(53, 376);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(94, 23);
            this.btnConfirmar.TabIndex = 7;
            this.btnConfirmar.Text = "Confirmar";
            this.btnConfirmar.UseVisualStyleBackColor = true;
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click_1);
            // 
            // btnAtendida
            // 
            this.btnAtendida.Location = new System.Drawing.Point(176, 347);
            this.btnAtendida.Name = "btnAtendida";
            this.btnAtendida.Size = new System.Drawing.Size(100, 23);
            this.btnAtendida.TabIndex = 8;
            this.btnAtendida.Text = "Atendido";
            this.btnAtendida.UseVisualStyleBackColor = true;
            this.btnAtendida.Click += new System.EventHandler(this.btnAtendida_Click_1);
            // 
            // btnNoAsistio
            // 
            this.btnNoAsistio.Location = new System.Drawing.Point(176, 376);
            this.btnNoAsistio.Name = "btnNoAsistio";
            this.btnNoAsistio.Size = new System.Drawing.Size(100, 23);
            this.btnNoAsistio.TabIndex = 9;
            this.btnNoAsistio.Text = "No Asistio";
            this.btnNoAsistio.UseVisualStyleBackColor = true;
            this.btnNoAsistio.Click += new System.EventHandler(this.btnNoAsistio_Click_1);
            // 
            // btnCancelarCita
            // 
            this.btnCancelarCita.Location = new System.Drawing.Point(286, 347);
            this.btnCancelarCita.Name = "btnCancelarCita";
            this.btnCancelarCita.Size = new System.Drawing.Size(100, 23);
            this.btnCancelarCita.TabIndex = 10;
            this.btnCancelarCita.Text = "Cancelar Cita";
            this.btnCancelarCita.UseVisualStyleBackColor = true;
            this.btnCancelarCita.Click += new System.EventHandler(this.btnCancelarCita_Click_1);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(417, 347);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(100, 23);
            this.btnLimpiar.TabIndex = 11;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click_1);
            // 
            // btnVolver
            // 
            this.btnVolver.Location = new System.Drawing.Point(546, 347);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(81, 23);
            this.btnVolver.TabIndex = 12;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = true;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click_1);
            // 
            // dgvCitas
            // 
            this.dgvCitas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCitas.Location = new System.Drawing.Point(578, 45);
            this.dgvCitas.Name = "dgvCitas";
            this.dgvCitas.RowHeadersWidth = 51;
            this.dgvCitas.RowTemplate.Height = 24;
            this.dgvCitas.Size = new System.Drawing.Size(477, 274);
            this.dgvCitas.TabIndex = 13;
            // 
            // chkFiltrarFecha
            // 
            this.chkFiltrarFecha.AutoSize = true;
            this.chkFiltrarFecha.Location = new System.Drawing.Point(354, 173);
            this.chkFiltrarFecha.Name = "chkFiltrarFecha";
            this.chkFiltrarFecha.Size = new System.Drawing.Size(103, 20);
            this.chkFiltrarFecha.TabIndex = 14;
            this.chkFiltrarFecha.Text = "Filtrar Fecha";
            this.chkFiltrarFecha.UseVisualStyleBackColor = true;
            // 
            // dtpFiltroFecha
            // 
            this.dtpFiltroFecha.Location = new System.Drawing.Point(348, 231);
            this.dtpFiltroFecha.Name = "dtpFiltroFecha";
            this.dtpFiltroFecha.Size = new System.Drawing.Size(200, 22);
            this.dtpFiltroFecha.TabIndex = 15;
            // 
            // cmbFiltroMedico
            // 
            this.cmbFiltroMedico.FormattingEnabled = true;
            this.cmbFiltroMedico.Location = new System.Drawing.Point(155, 226);
            this.cmbFiltroMedico.Name = "cmbFiltroMedico";
            this.cmbFiltroMedico.Size = new System.Drawing.Size(172, 24);
            this.cmbFiltroMedico.TabIndex = 16;
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Location = new System.Drawing.Point(311, 376);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(75, 23);
            this.btnFiltrar.TabIndex = 17;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click_1);
            // 
            // btnQuitarFiltros
            // 
            this.btnQuitarFiltros.Location = new System.Drawing.Point(417, 376);
            this.btnQuitarFiltros.Name = "btnQuitarFiltros";
            this.btnQuitarFiltros.Size = new System.Drawing.Size(100, 23);
            this.btnQuitarFiltros.TabIndex = 18;
            this.btnQuitarFiltros.Text = "Quitar filtros";
            this.btnQuitarFiltros.UseVisualStyleBackColor = true;
            this.btnQuitarFiltros.Click += new System.EventHandler(this.btnQuitarFiltros_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 16);
            this.label1.TabIndex = 19;
            this.label1.Text = "Medico";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 16);
            this.label2.TabIndex = 20;
            this.label2.Text = "Paciente";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 16);
            this.label3.TabIndex = 21;
            this.label3.Text = "Duración";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 196);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 22;
            this.label4.Text = "Modalidad";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 226);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 16);
            this.label5.TabIndex = 23;
            this.label5.Text = "Filtro";
            // 
            // FormCitas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 450);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnQuitarFiltros);
            this.Controls.Add(this.btnFiltrar);
            this.Controls.Add(this.cmbFiltroMedico);
            this.Controls.Add(this.dtpFiltroFecha);
            this.Controls.Add(this.chkFiltrarFecha);
            this.Controls.Add(this.dgvCitas);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnCancelarCita);
            this.Controls.Add(this.btnNoAsistio);
            this.Controls.Add(this.btnAtendida);
            this.Controls.Add(this.btnConfirmar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.dtpHora);
            this.Controls.Add(this.dtpFecha);
            this.Controls.Add(this.cmbModalidad);
            this.Controls.Add(this.cmbDuracion);
            this.Controls.Add(this.cmbPaciente);
            this.Controls.Add(this.cmbMedico);
            this.Name = "FormCitas";
            this.Text = "FormCitas";
            this.Load += new System.EventHandler(this.FormCitas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCitas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbMedico;
        private System.Windows.Forms.ComboBox cmbPaciente;
        private System.Windows.Forms.ComboBox cmbDuracion;
        private System.Windows.Forms.ComboBox cmbModalidad;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.DateTimePicker dtpHora;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.Button btnAtendida;
        private System.Windows.Forms.Button btnNoAsistio;
        private System.Windows.Forms.Button btnCancelarCita;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.DataGridView dgvCitas;
        private System.Windows.Forms.CheckBox chkFiltrarFecha;
        private System.Windows.Forms.DateTimePicker dtpFiltroFecha;
        private System.Windows.Forms.ComboBox cmbFiltroMedico;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Button btnQuitarFiltros;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}