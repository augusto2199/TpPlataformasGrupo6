using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfazTP.Data;

namespace InterfazTP
{
    public partial class Main : Form
    {
        public object[] argumentos;
        List<List<string>> datos;
        public string usuario;
        public Banco banco;
        public TransDelegadoModificarUsuario TransFModificarUsuario;
        public TransDelegadoModificarTitularesCajaAhorro TransFModificarTitulares;
        public TransDelegadoCerrarSesion TransFCerrarSesion;
        private int selectedCaja;
        private int selectedPLazoFijo;
        private int selectedPago;
        private int selectedTarjeta;
        private int selectedUsuario;

        public Main(string usuario, Banco banco)
        {
            InitializeComponent();
            this.usuario = usuario;
            this.banco = banco;
            refreshDataCbu();
            refreshDataPlazoFijo();
            refreshUsuarios();
            refreshDataPagos();
            refreshDataTarjetasDeCredito();
            //label9.Text = PlazoFijo.tasa.ToString();
        }

        public Main(object[] args)
        {
            InitializeComponent();
            banco = (Banco)args[1];
            argumentos = args;
            //label2.Text = (string)args[0];
            datos = new List<List<string>>();
            refreshDataCajaDeAhorro();
            refreshDataCbu();
            refreshDataPlazoFijo();
            refreshUsuarios();
            refreshDataPagos();
            refreshDataTarjetasDeCredito();
            //label9.Text = PlazoFijo.tasa.ToString();


        }

        // Modificar Usuario
        private void button12_Click(object sender, EventArgs e)
        {
            bool modificar = true;
            this.TransFModificarUsuario(modificar);
        }

        public delegate void TransDelegadoModificarTitularesCajaAhorro(bool modificar);
        public delegate void TransDelegadoCerrarSesion(bool modificar);
        public delegate void TransDelegadoModificarUsuario(bool modificar);

        // CAJA DE AHORRO ////////////////////////////////////////////////////////////

        // Refresh Caja de Ahorro
        private void refreshDataCajaDeAhorro()
        {
            dataGridView1.Rows.Clear();

            if (banco.usuarioActual.administrador)
            {
                foreach (CajaDeAhorro c in banco.MostrarTodasLasCajas())
                {
                    dataGridView1.Rows.Add(c.toArray());
                }
            }
            else
            {
                foreach (CajaDeAhorro c in banco.usuarioActual.obtenerCajas())
                {
                    dataGridView1.Rows.Add(c.toArray());
                }
            }
        }

        // Refresh de cajas al agregar o eliminar Titulares
        public void RefreshModificacionesTitulares()
        {
            refreshDataCajaDeAhorro();
        }

        private void refreshDataCbu()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            comboBox5.Items.Clear();

            if (banco.usuarioActual.administrador)
            {
                foreach (CajaDeAhorro c in banco.MostrarTodasLasCajas())
                {
                    comboBox1.Items.Add(c.cbu.ToString());
                    comboBox3.Items.Add(c.cbu.ToString());
                    comboBox4.Items.Add(c.cbu.ToString());
                    comboBox5.Items.Add(c.cbu.ToString());
                }
                foreach (TarjetaDeCredito t in banco.MostrarTarjetas())
                {
                    comboBox4.Items.Add(t.numero.ToString());
                }
            }
            else
            {

                foreach (TarjetaDeCredito t in banco.usuarioActual.obtenerTarjetas())
                {
                    comboBox4.Items.Add(t.numero.ToString());
                }

                foreach (CajaDeAhorro c in banco.usuarioActual.obtenerCajas())
                {
                    comboBox1.Items.Add(c.cbu.ToString());
                    comboBox3.Items.Add(c.cbu.ToString());
                    comboBox4.Items.Add(c.cbu.ToString());
                    comboBox5.Items.Add(c.cbu.ToString());
                }
            }

            foreach (CajaDeAhorro c in banco.MostrarTodasLasCajas())
            {
                comboBox2.Items.Add(c.cbu.ToString());
            }
        }

        private void refreshDataCajaMovimientos()
        {
            dataGridView5.Rows.Clear();

            foreach (Movimiento m in   banco.MostrarMovimientos(this.selectedCaja))
            {
                dataGridView5.Rows.Add(m.toArray());
            }
        }

        // Seleccionador de caja
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedCaja = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value);
            if (selectedCaja != null && selectedCaja != 0)
            {
                refreshDataCajaMovimientos();
            }
        }

        // Agregar Caja Ahorro
        private void button1_Click(object sender, EventArgs e)
        {
            banco.AltaCajaAhorro();
            MessageBox.Show("Se agrego una nueva Caja de Ahorro.");
            refreshDataCajaDeAhorro();
            refreshDataCbu();
        }


        //Eliminar Caja de Ahorro
        private void button3_Click(object sender, EventArgs e)
        {
            if (banco.BajaCajaAhorro(selectedCaja))
            {
                MessageBox.Show(selectedCaja.ToString());
                MessageBox.Show("Se elimino correctamente");
                refreshDataCbu();
                refreshDataCajaDeAhorro();
            }
            else
            {
                MessageBox.Show("No se pudo eliminar la cuenta");
            }
            

        }

        //Retirar
        private void button15_Click(object sender, EventArgs e)
        {
            if(banco.Retirar(selectedCaja, Convert.ToInt32(textBox1.Text)))
            {
                MessageBox.Show("Se retiro correctamente: " + textBox1.Text);
                refreshDataCajaMovimientos();
                refreshDataCajaDeAhorro();
            }
            else
            {
                MessageBox.Show("No se pudo retirar");
            }

        }
        //Depositar
        private void button14_Click(object sender, EventArgs e)
        {
            if(banco.Depositar(selectedCaja, Convert.ToInt32(textBox1.Text)))
            {
                MessageBox.Show("Se deposito correctamente: " + textBox1.Text);
                refreshDataCajaMovimientos();
                refreshDataCajaDeAhorro();
            }
            else
            {
                MessageBox.Show("No se pudoi depositar");
            }

        }
        //Transferir
        private void button13_Click(object sender, EventArgs e)
        {
            if(banco.Transferir(Convert.ToInt32(comboBox1.Text), Convert.ToInt32(comboBox2.Text), Convert.ToInt32(textBox2.Text)))
            {
                MessageBox.Show("Hola linea de main 521");

                MessageBox.Show("Se transfirio correctamente $" + textBox2.Text + "A la cuenta: " + comboBox2.Text);
                refreshDataCajaMovimientos();
                refreshDataCajaDeAhorro();
            }
            else
            {
                MessageBox.Show("No se pudo tranferir");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Boton para Modificar Titulares de Caja de Ahorro
            bool resultado = true;
            this.TransFModificarTitulares(resultado);
        }

        // PLAZOS FIJOS ////////////////////////////////////////////////////////////
        private void refreshDataPlazoFijo()
        {
            dataGridView2.Rows.Clear();

            if (banco.usuarioActual.administrador)
            {
                foreach (PlazoFijo p in banco.MostrarPlazoFijos())
                {
                    dataGridView2.Rows.Add(p.toArray());
                }
            }
            else
            {
                foreach (PlazoFijo p in banco.usuarioActual.pf)
                {
                    dataGridView2.Rows.Add(p.toArray());
                }
            }
        }

        // Seleccionador de Plazo Fijo
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedPLazoFijo = Convert.ToInt32(dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[0].Value);
        }

        //Agregar Plazo Fijo
        private void button4_Click(object sender, EventArgs e)
        {
            if (banco.AltaPlazoFijo(banco.usuarioActual, float.Parse(textBox3.Text), Convert.ToInt32(comboBox3.Text)))
            {
                MessageBox.Show("Se creo tu plazo fijo");
                refreshDataPlazoFijo();
                refreshDataCajaDeAhorro();
            }
            else
            {
                MessageBox.Show("Hubo un problema al crear el plazo fijo");
            }
        }

        //Eliminar Plazo Fijo
        private void button6_Click(object sender, EventArgs e)
        {
            if (banco.BajaPlazoFijo(selectedPLazoFijo))
            {
                MessageBox.Show("Se elimino correctamente");
                refreshDataPlazoFijo();
            }
            else
            {
                MessageBox.Show("No se pudo eliminar");
            }
        }


        // PAGOS ////////////////////////////////////////////////////////////
        private void refreshDataPagos()
        {
            dataGridView3.Rows.Clear();
            dataGridView6.Rows.Clear();

            if (banco.usuarioActual.administrador)
            {
                foreach (Pago p in banco.MostrarPagos())
                {
                    if (p.pagado == true)
                    {
                        dataGridView3.Rows.Add(p.toArray());
                    }
                    else
                    {
                        dataGridView6.Rows.Add(p.toArray());
                    }
                }
            }
            else
            {
                foreach (Pago p in banco.usuarioActual.pagos)
                {
                    if (p.pagado == true)
                    {
                        dataGridView3.Rows.Add(p.toArray());
                    }
                    else
                    {
                        dataGridView6.Rows.Add(p.toArray());
                    }
                }
            }
        }

        //Seleccionar pago
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedPago= Convert.ToInt32(dataGridView3.Rows[dataGridView3.CurrentRow.Index].Cells[0].Value);
        }

        private void dataGridView6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedPago = Convert.ToInt32(dataGridView6.Rows[dataGridView6.CurrentRow.Index].Cells[0].Value);
        }

        //Agregar Pago
        private void button5_Click(object sender, EventArgs e)
        {
            banco.AltaPago(banco.usuarioActual, textBox4.Text, float.Parse(textBox5.Text));
            refreshDataPagos();
        }

        //Pagar pago
        private void button7_Click(object sender, EventArgs e)
        {
            if(banco.ModificarPago(selectedPago, Convert.ToInt32(comboBox4.Text)))
            {
                MessageBox.Show("El pago se realizo correctamente");
                refreshDataPagos();
                refreshDataCajaDeAhorro();
                refreshDataTarjetasDeCredito();
            }
            else
            {
                MessageBox.Show("No se pudo realizar el pago");
            }
        }

        //Eliminar Pago
        private void button8_Click(object sender, EventArgs e)
        {
            if (banco.BajaPago(selectedPago))
            {
                MessageBox.Show("Se elimno el pago correctamente");
                refreshDataPagos();
            }
            else{
                MessageBox.Show("No se pudo eliminar el pago");
            }
        }

        // TARJETAS DE CREDITO ////////////////////////////////////////////////////////////
        private void refreshDataTarjetasDeCredito()
        {
            dataGridView4.Rows.Clear();

            if (banco.usuarioActual.administrador)
            {
                foreach (TarjetaDeCredito t in banco.MostrarTarjetas())
                {
                    dataGridView4.Rows.Add(t.toArray());
                }
            }
            else
            {
                foreach (TarjetaDeCredito t in banco.usuarioActual.tarjetas)
                {
                    dataGridView4.Rows.Add(t.toArray());
                }
            }
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedTarjeta = Convert.ToInt32(dataGridView4.Rows[dataGridView4.CurrentRow.Index].Cells[0].Value);
        }

        //Alta tarjeta de credito
        private void button9_Click(object sender, EventArgs e)
        {
            banco.AltaTarjetaCredito();
            MessageBox.Show("Se creo la tarjeta de credito correctamente");
            refreshDataTarjetasDeCredito();
            refreshDataCbu();
        }

        //Pagar tarjeta
        private void button10_Click(object sender, EventArgs e)
        {
            if (banco.pagarTarjeta(selectedTarjeta, Convert.ToInt32(comboBox5.Text)))
            {
                MessageBox.Show("Se efectuo el pago correctamente");
                refreshDataTarjetasDeCredito();
                refreshDataCajaDeAhorro();
            }
            else
            {
                MessageBox.Show("No se pudo realizar el pago");
            }
        }

        //Elimnar tarjeta
        private void button11_Click(object sender, EventArgs e)
        {
            if (banco.BajaTarjetaCredito(selectedTarjeta))
            {
                MessageBox.Show("Se elimino correctamente");
                refreshDataTarjetasDeCredito();
                refreshDataCbu();
            }
            else
            {
                MessageBox.Show("No se pudo eliminar");
            }
        }



        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }




        private void label6_Click(object sender, EventArgs e)
        {

        }



        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {


        }

        private void label9_Click(object sender, EventArgs e)
        {
            
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        //Cerrar sesion
        private void button16_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Queres cerrar sesion?", "Advertencia", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                bool confirmacion = true;
                this.TransFCerrarSesion(confirmacion);
                banco.CerrarSesion();
            }

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

		private void button17_Click(object sender, EventArgs e)
		{

        }

        private void dataGridView7_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedUsuario = Convert.ToInt32(dataGridView7.Rows[dataGridView7.CurrentRow.Index].Cells[0].Value);

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
        private void refreshUsuarios()
        {
            if (banco.usuarioActual.administrador)
            {
                tabPage5.Show();
                dataGridView7.Rows.Clear();
                foreach (Usuario u in banco.MostrarUsuarios())
                {
                    dataGridView7.Rows.Add(u.toArray());
                }
            }
        }

        // Boton Desbloquear Usuarios
        private void mostrarUusuarios_Click(object sender, EventArgs e)
        {
            banco.desbloquearUsuario(selectedUsuario);
            refreshUsuarios();
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button17_Click_1(object sender, EventArgs e)
        {

        }
    }
}
