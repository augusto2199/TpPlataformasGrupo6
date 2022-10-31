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
    public partial class ModificarTitulares : Form
    {
        Banco banco;
        public TransDelegadoVolverAtrasTitulares TransfEventoVolver;
        private int selectCaja;
        private int selectTitularDisponible;


        public ModificarTitulares(Banco banco)
        {
            InitializeComponent();
            this.banco = banco;
            refreshDataCajasAhorro();
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
            
        }

        // Boton Agregar Titular
        private void button2_Click(object sender, EventArgs e)
        {

        }

        // Combobox que muestra las cajas de ahorro y los titulares de cada una
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectCaja =  Convert.ToInt32(comboBox1.Text);
            refreshDataTitulares();
            refreshDataTitularesDisponibles();
        }

        // Refresca las cajas agregadas o eliminadas del usuario y las agrega en el combobox
        private void refreshDataCajasAhorro()
        {
            comboBox1.Items.Clear();

            for(int i = 0; i < banco.MostrarCajasDeAhorro().Count; i++)
            {
                comboBox1.Items.Add(banco.MostrarCajasDeAhorro()[i].cbu);
            }
        }

        // Seleccionador de titular de caja de ahorro
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        // Refresca y muestra los titulares de la cuenta seleccionada
        private void refreshDataTitulares()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(banco.MostrarDatosTitular(selectCaja));
        }

        // Refresca titulares que no estan asignados a la caja seleccionada
        private void refreshDataTitularesDisponibles()
        {
            dataGridView2.Rows.Clear();

            List<Usuario> titulares = banco.MostrarTitularesDisponibles(selectCaja);

            for (int i = 0; i < titulares.Count; i++)
            {
                dataGridView2.Rows.Add(titulares[i].nombre, titulares[i].apellido, titulares[i].dni);
            }
        }

        // Seleccionar titular para agregar
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectTitularDisponible = 1;
        }

        private void ModificarTitulares_Load(object sender, EventArgs e)
        {

        }
    }
}
