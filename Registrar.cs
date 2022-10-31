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
    public partial class Registrar : Form
    {
        public bool logued;
        public string usuario;
        public string pass;
        public Banco banco;
        public bool confirmacionRegistro;
        public bool volverAtras;

        public TransfDelegadoRegisterOK TransEventoRegisterOk;

        public Registrar(Banco banco)
        {
            logued = false;
            InitializeComponent();
            this.banco = banco;
        }

        // Boton de Registrar Usuario
        private void button1_Click(object sender, EventArgs e)
        {
            if (banco.AltaUsuario(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text))
            {
                MessageBox.Show("Usuario Registrado!");
                confirmacionRegistro = true;
                this.TransEventoRegisterOk(confirmacionRegistro);
            }
            else
            {
                MessageBox.Show("No se pudo registrar Usuario!");
            }
        }

        public delegate void TransfDelegadoRegisterOK(bool confirmacionRegistro);

        // Boton Volver para atras
        private void button2_Click(object sender, EventArgs e)
        {
            confirmacionRegistro = true;
            this.TransEventoRegisterOk(confirmacionRegistro);
        }
    }
}
