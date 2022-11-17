using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

namespace InterfazTP
{
    public class CajaDeAhorro
    {
        
        public int id { get; set; }
        
        public int cbu { get; set; }
       
        public float saldo { get; set; }
        public List<Usuario> titular { get; set; }
        public List<Movimiento> movimientos { get; set; }
        private BaseDeDatos db;



        public CajaDeAhorro(int id,int cbu,float saldo)
         {
            this.id = id;
            this.saldo = saldo;
            this.cbu = cbu;
            titular = new List<Usuario>();
            movimientos = new List<Movimiento>();
            db = new BaseDeDatos();
            InicializarAtributos(id);
        }
        private void InicializarAtributos(int caja)
        {//Si lo activo se hace un loop jajajajaXD
            this.titular = db.mostrarUsuarioEnCaja(caja);
           this.movimientos = db.mostrarMovimientoEnCaja(caja);
        }

        public CajaDeAhorro( )
        {
            titular = new List<Usuario>();


            movimientos = new List<Movimiento>();
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