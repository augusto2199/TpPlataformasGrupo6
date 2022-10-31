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
        public string usuarioLogin { get; set; }
        public int id { get; set; }
        public static int ultimoId = 0;
        public string dni { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int intentosFallidos { get; set; }
        public bool bloqueado { get; set; }

        public bool administrador { get; set; }
        public List<CajaDeAhorro> cajas { get; set; }
        public List<PlazoFijo> pf { get; set; }
        public List<TarjetaDeCredito> tarjetas { get; set; }
        public List<Pago> pagos { get; set; }

        public Usuario()
        {
            id = generarId();

            bloqueado = false;

            pagos = new List<Pago>();

            tarjetas = new List<TarjetaDeCredito>();

            pf = new List<PlazoFijo>();

            cajas = new List<CajaDeAhorro>(10);
            
            administrador = false;

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
    }
}
