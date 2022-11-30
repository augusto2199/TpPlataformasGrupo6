using System;
using System.Collections.Generic;
using System.Text;

namespace InterfazTP.Data
{
    public class Pago
    {
        public int id { get; set; }
        public Usuario usuario { get; set; }
        public int num_usr { get; set; }
        public string nombre { get; set; }
        public float monto { get; set; }
        public bool pagado { get; set; }
        public string metodo { get; set; }
        public Pago()
        {

        }

        public Pago( float monto, string nombre, bool pagado)
        {
            this.nombre = nombre;
            this.monto = monto;
            this.pagado = pagado;
            this.metodo = " ";
        }
        public string[] toArray()
        {
            return new string[] { id.ToString(), nombre, monto.ToString(), pagado.ToString() };
        }
    }
}
