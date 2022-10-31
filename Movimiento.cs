using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfazTP
{
    public class Movimiento
    {
        public int id { get; set; }
        public static int ultimoId = 0;
        public CajaDeAhorro caja { get; set; }
        public string detalle { get; set; }
        public float monto { get; set; }
        public DateTime fecha { get; set; }

        public Movimiento(CajaDeAhorro caja, string detalle, float monto)
        {
            id = generarId();
            this.caja = caja;
            this.detalle = detalle;
            this.monto = monto;
            fecha = DateTime.Now;
        }

        private int generarId()
        {
            ultimoId++;
            return ultimoId;
        }

        public string[] toArray()
        {
            return new string[] { id.ToString(), caja.cbu.ToString(), detalle, monto.ToString(), fecha.ToString() };
        }

    }
}
