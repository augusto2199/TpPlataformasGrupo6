using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfazTP.Data;

namespace InterfazTP
{
    public partial class ModificarTitulares : Form
    {
        Banco banco;
        public TransDelegadoVolverAtrasTitulares TransfEventoVolver;
        private int selectCaja;
        private int selectTitular;
        private int selectTitularDisponible;


        public ModificarTitulares(Banco banco)
        {
            InitializeComponent();
            this.banco = banco;
            refreshDataCajasAhorro();
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        // Boton Volver
        private void button3_Click(object sender, EventArgs e)
        {
            bool confirmacion = true;
            this.TransfEventoVolver(confirmacion);
        }

        public delegate void TransDelegadoVolverAtrasTitulares(bool confirmacion);

        // Boton Eliminar Titular
        private void button1_Click(object sender, EventArgs e)
        {
            if (banco.EliminarTitular(selectTitular, selectCaja))
            {
                refreshDataTitulares();
                refreshDataTitularesDisponibles();
                MessageBox.Show("Se elimino titular de caja de ahorro.");
            }
            else
            {
                MessageBox.Show("Las cajas de ahorro deben tener por lo menos 1 usuario.");
            }
        }

        // Boton Agregar Titular
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(selectTitularDisponible.ToString());
            if (banco.AgregarTitular(selectTitularDisponible, selectCaja))
            {
                refreshDataTitulares();
                refreshDataTitularesDisponibles();
                MessageBox.Show("Se agrego titular a caja de ahorro.");
            }
            else
            {
                MessageBox.Show("No se pudo agregar");
            }
        }

        // Combobox que muestra las cajas de ahorro y los titulares de cada una
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectCaja = Convert.ToInt32(comboBox1.Text);
            refreshDataTitulares();
            refreshDataTitularesDisponibles();
        }

        // Refresca las cajas agregadas o eliminadas del usuario y las agrega en el combobox
        private void refreshDataCajasAhorro()
        {
            comboBox1.Items.Clear();

            for (int i = 0; i < banco.MostrarCajasDeAhorro().Count; i++)
            {
                comboBox1.Items.Add(banco.MostrarCajasDeAhorro()[i].cbu);
            }
        }

        

        // Refresca y muestra los titulares de la cuenta seleccionada
        private void refreshDataTitulares()
        {
            dataGridView1.Rows.Clear();

            List<Usuario> titulares = banco.MostrarDatosTitular(selectCaja);

            for (int i = 0; i < titulares.Count; i++)
            {
                if(titulares[i].administrador == false)
                {
                    dataGridView1.Rows.Add(titulares[i].nombre, titulares[i].apellido, titulares[i].dni);
                }
            }
        }

        // Refresca titulares que no estan asignados a la caja seleccionada
        private void refreshDataTitularesDisponibles()
        {
            dataGridView2.Rows.Clear();
            List<Usuario> usuario = banco.MostrarDatosTitular(selectCaja);

            List<Usuario> titulares = banco.MostrarTitularesDisponibles(usuario);

            for (int i = 0; i < titulares.Count; i++)
            {
                if (titulares[i].administrador == false)
                {
                    dataGridView2.Rows.Add(titulares[i].nombre, titulares[i].apellido, titulares[i].dni);
                }

            }
        }


        // Seleccionador de titular de caja de ahorro
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectTitular = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value);
        }

        // Seleccionar titular para agregar
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectTitularDisponible = Convert.ToInt32(dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[2].Value);
            
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ModificarTitulares_Load(object sender, EventArgs e)
        {

        }
    }
}
