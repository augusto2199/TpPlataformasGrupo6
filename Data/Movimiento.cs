using System;
using System.Collections.Generic;
using System.Text;

namespace  InterfazTP.Data
{
    public class Movimiento
    {
        public int id_movimiento { get; set; }
        public CajaDeAhorro caja { get; set; }
        public int num_caja { get; set; }
        public string detalle { get; set; }
        public float monto { get; set; }
        public DateTime fecha { get; set; }

        public Movimiento()
        {

        }

        public Movimiento(string detalle, float monto, DateTime fecha, CajaDeAhorro caja)
        {
            this.detalle = detalle;
            this.monto = monto;
            this.fecha = fecha;
            this.caja = caja;
        }
        public string[] toArray()
        {
            return new string[] { id_movimiento.ToString(), caja.cbu.ToString(), detalle, monto.ToString(), fecha.ToString() };
        }
    }
}
