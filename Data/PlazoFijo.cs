using System;
using System.Collections.Generic;
using System.Text;

namespace InterfazTP.Data
{
    public class PlazoFijo
    {
        public int id { get; set; }
        public Usuario titular { get; set; }
        public int num_usr { get; set; }
        public float monto { get; set; }
        public DateTime fechaIni { get; set; }
        public DateTime fechaFin { get; set; }
        public float tasa { get; set; } = 60;
        public bool pagado { get; set; }


        public PlazoFijo()
        {

        }

        public PlazoFijo( float monto, DateTime fechaIni, DateTime fechaFin, float tasa, bool pagado)
        {
            

            this.monto = monto;
            this.fechaIni = fechaIni;
            this.fechaFin = fechaFin;
            this.pagado = pagado;
            this.tasa = tasa;
        }
        public float getTasa()
        {
            return tasa;
        }

        public string[] toArray()
        {
            return new string[] { id.ToString(), monto.ToString(), fechaIni.ToString(), fechaFin.ToString(), tasa.ToString(), pagado.ToString() };
        }
    }
}
