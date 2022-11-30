using System;
using System.Collections.Generic;
using System.Text;

namespace InterfazTP.Data
{
    public class TarjetaDeCredito
    {
        // Revisar titular o num_usr
        public int id { get; set; }
        public Usuario titular { get; set; }
        public int num_usr { get; set; }
        public int numero { get; set; }
        public int codigoV { get; set; }
        public float limite { get; set; }
        public float consumos { get; set; }

        public TarjetaDeCredito()
        {

        }

        public TarjetaDeCredito(int numero, int codigov, float limi, float consumo)
        {
            this.numero = numero;
            this.codigoV = codigov;
            this.limite = limi;
            this.consumos = consumo;
        }

        public string[] toArray()
        {
            return new string[] { id.ToString(), numero.ToString(), codigoV.ToString(), limite.ToString(), consumos.ToString() };
        }

    }
}
