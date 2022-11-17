using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfazTP
{
    public partial class Login : Form
    {
        public bool logued;
        public string usuario;
        public string pass;
        public Banco banco;

        public TransfDelegado TransfEvento;
        public TransDelegadoRegister TransfEventoRegister;

        public Login(Banco b)
        {
            logued = false;
            InitializeComponent();
            banco = b;
        }

        // Login
        public delegate void TransfDelegado(string usuario, string pass);

        // Boton Login Usuario
        private void button1_Click(object sender, EventArgs e)
        {
            usuario = textBox1.Text;
            pass = textBox2.Text;
            if (usuario != null && usuario != "")
            {
                this.TransfEvento(usuario, pass);
                textBox1.Text = "";
                textBox2.Text = "";
            }
            else
            {
                MessageBox.Show("Debe ingresar un usuario!");
            }
        }

        public delegate void TransDelegadoRegister(bool logued);

        // Boton registrarse, abre form Registrar
        private void button2_Click(object sender, EventArgs e)
        {
            if(logued == false)
            {
                this.TransfEventoRegister(logued);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
