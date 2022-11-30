using System;
using System.Collections.Generic;
using System.Text;

namespace InterfazTP.Data
{
    public class CajaDeAhorro
    {

        public int id { get; set; }

        public int cbu { get; set; }

        public float saldo { get; set; }

        //public Usuario Titular { get; set; }

        public ICollection<Usuario> Titulares { get; } = new List<Usuario>();

        public List<UsuarioCajaDeAhorro> UsuarioCaja { get; set; }
        public List<Movimiento> movimientos { get; } = new List<Movimiento> { };

        public CajaDeAhorro()
        {

        }

        public CajaDeAhorro(int cbu, float saldo)
        {
            this.saldo = saldo;
            this.cbu = cbu;
        }

        public void agregarMovimiento(Movimiento mov)
        {
            this.movimientos.Add(mov);
        }

        public List<Movimiento> obetenerMovimientos()
        {
            return this.movimientos.ToList();
        }

        public string[] toArray()
        {
            return new string[] { id.ToString(), cbu.ToString(), saldo.ToString() };
        }

    }
}
