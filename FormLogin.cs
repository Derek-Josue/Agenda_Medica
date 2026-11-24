using Agenda_Medica.BL;
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
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void BtnIngresar_Click(object sender, EventArgs e)
        { }
            private void btnIngresar_Click(object sender, EventArgs e)
        {
            UsuarioBL bl = new UsuarioBL();
            if (bl.Login(txtUsuario.Text, txtClave.Text))
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Usuario o clave incorrectos");
                txtClave.Clear();
                txtClave.Focus();
            }
            
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
               

        }
    }
    
}
