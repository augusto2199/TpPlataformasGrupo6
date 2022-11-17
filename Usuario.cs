using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace InterfazTP
{
    public class Usuario
    {
        public int id
        {
            get; set;
        }
        public static int ultimoId = 0;
        public int dni
        {
            get; set;
        }
        public string nombre
        {
            get; set;
        }
        public string apellido
        {
            get; set;
        }
        public string email
        {
            get; set;
        }
        public string usuario
        {
            get; set;
        }
        public string password
        {
            get; set;
        }
        public int intentosFallidos
        {
            get; set;
        }
        public bool bloqueado
        {
            get; set;
        }
        public bool administrador
        {
            get; set;
        }
        public List<CajaDeAhorro> cajas
        {
            get; set;
        }
        public List<PlazoFijo> pf
        {
            get; set;
        }
        public List<TarjetaDeCredito> tarjetas
        {
            get; set;
        }
        public List<Pago> pagos
        {
            get; set;
        }
        private BaseDeDatos db;

        public Usuario()
        {
            id = generarId();

            bloqueado = false;

            pagos = new List<Pago>();

            tarjetas = new List<TarjetaDeCredito>();

            pf = new List<PlazoFijo>();

            cajas = new List<CajaDeAhorro>();

            administrador = false;

        }

        public Usuario(int id, int dni, string nombre, string apellido, string mail, string usuario, string contraseña, int intentos, bool bloqueo, bool admin)
        {
            this.id = id;
            this.dni = dni;
            this.nombre = nombre;
            this.apellido = apellido;
            this.email = mail;
            this.usuario = usuario;
            this.password = contraseña;
            this.intentosFallidos = intentos;
            this.administrador = admin;
            this.bloqueado = bloqueo;

            pagos = new List<Pago>();

            tarjetas = new List<TarjetaDeCredito>();

            pf = new List<PlazoFijo>();

            cajas = new List<CajaDeAhorro>();

            db = new BaseDeDatos();
            
        }


        public void InicializarAtributos(int id)
        {
            pagos = db.mostrarPagoEnUsuario(id);
            tarjetas = db.mostrarTarjetaEnUsuario(id);
            pf = db.mostrarPlazoFijoEnUsuario(id);
            cajas = db.mostrarCajaEnUsuario(id);
        }
        private int generarId()
        {
            ultimoId++;
            return ultimoId;
        }

        public List<CajaDeAhorro> obtenerCajas()
        {
            return cajas.ToList();
        }

        public List<TarjetaDeCredito> obtenerTarjetas()
        {
            return tarjetas.ToList();
        }
        public string[] toArray()
        {
            return new string[] { id.ToString(), nombre, bloqueado.ToString(), administrador.ToString() };
        }
    }
}
