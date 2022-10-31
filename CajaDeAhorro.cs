using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace InterfazTP
{
    public class CajaDeAhorro
    {
        public int id { get; set; }
        public static int ultimoId = 0;
        public int cbu { get; set; }

        private static int cbuAnterior = 1000000000;
        public List<Usuario> titular { get; set; }
        public float saldo { get; set; }
        public List<Movimiento> movimientos { get; set; }


        public CajaDeAhorro()
        {
            cbu = generarCbu();
            id = generarId();
            titular = new List<Usuario>();

            movimientos = new List<Movimiento>();

        }

        private int generarId()
        {
            ultimoId++;
            return ultimoId;
        }
        
        private int generarCbu()
        {
            cbuAnterior++;
            return cbuAnterior;
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
            return new string[] {id.ToString(), cbu.ToString(), saldo.ToString()};
        }

    }
}