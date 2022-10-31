using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfazTP
{
    public class PlazoFijo
    {
        public int id { get; set; }
        public static int ultimoId = 0;
        public Usuario titular { get; set; }
        public float monto { get; set; }
        public DateTime fechaIni { get; set; }
        public DateTime fechaFin { get; set; }
        public static float tasa { get; set; } = 60;
        public bool pagado { get; set; }

        public PlazoFijo(Usuario titular, float monto)
        {
            id = generarId();
            this.titular = titular;
            this.monto = monto;
            this.fechaIni = DateTime.Now;
            this.fechaFin = DateTime.Now.AddYears(1);
            pagado = false;
        }

        public float getTasa()
        {
            return tasa;
        }

        private int generarId()
        {
            ultimoId++;
            return ultimoId;
        }

        public string[] toArray()
        {
            return new string[] { id.ToString(), monto.ToString(), fechaIni.ToString(), fechaFin.ToString(), tasa.ToString(), pagado.ToString() };
        }
    }
}
