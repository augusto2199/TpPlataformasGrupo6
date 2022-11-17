using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfazTP
{
    public class Pago
    {
        public int id { get; set; }
        public static int ultimoId = 0;
        public Usuario usuario { get; set; }
        public string nombre { get; set; }
        public float monto { get; set; }
        public bool pagado { get; set; }
        public string metodo { get; set; }

        public Pago(Usuario usuario, string nombre, float monto)
        {
            id = generarId();
            this.usuario = usuario;
            this.nombre = nombre;
            this.monto = monto;
            this.pagado = false;
        }
        public Pago(int id, float monto, string nombre,bool pagado,string metodo)
        {
            this.id = id;
            this.nombre = nombre;
            this.monto = monto;
            this.metodo = metodo;
            this.pagado = pagado;
            
        }

        private int generarId()
        {
            ultimoId++;
            return ultimoId;
        }

        public string[] toArray()
        {
            return new string[] { id.ToString(), nombre, monto.ToString(), pagado.ToString() };
        }

    }
}
