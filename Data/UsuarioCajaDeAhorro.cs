using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfazTP.Data
{
    public class UsuarioCajaDeAhorro
    {
        public int id_UsuarioCaja
        {
            get; set;
        }

        public Usuario usuario
        {
            get; set; 
        }

        public int fk_usuario
        {
            get; set;
        }

        public CajaDeAhorro cajaAhorro
        {
            get; set; 
        }

        public int fk_cajaAhorro
        {
            get; set;
        }

        public UsuarioCajaDeAhorro()
        {

        }
    }
}
