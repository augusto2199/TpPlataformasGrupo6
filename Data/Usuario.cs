using System;
using System.Collections.Generic;
using System.Text;

namespace InterfazTP.Data
{
    public class Usuario
    {
        public int id_usuario
        {
            get; set;
        }
        public int dni
        {
            get; set;
        }
        public string nombre
        {
            get; set;
        }
        public string apellido
        {
            get; set;
        }
        public string email
        {
            get; set;
        }
        public string usuario
        {
            get; set;
        }
        public string password
        {
            get; set;
        }
        public int intentosFallidos
        {
            get; set;
        }
        public bool bloqueado
        {
            get; set;
        }
        public bool administrador
        {
            get; set;
        }
        public ICollection<CajaDeAhorro> cajas { get; } = new List<CajaDeAhorro>();

        public List<UsuarioCajaDeAhorro> UsuarioCaja { get; set; }

        public List<PlazoFijo> pf { get; } = new List<PlazoFijo>();

        public List<TarjetaDeCredito> tarjetas { get; } = new List<TarjetaDeCredito>();
        public List<Pago> pagos { get; } = new List<Pago>();

        public Usuario()
        {

        }

        public Usuario(int dni, string nombre, string apellido, string mail, string usuario, string contraseña, int intentos, bool bloqueo, bool admin)
        {
            
            this.dni = dni;
            this.nombre = nombre;
            this.apellido = apellido;
            this.email = mail;
            this.usuario = usuario;
            this.password = contraseña;
            this.intentosFallidos = intentos;
            this.administrador = admin;
            this.bloqueado = bloqueo;
        }
        public List<CajaDeAhorro> obtenerCajas()
        {
            return cajas.ToList();
        }

        public List<TarjetaDeCredito> obtenerTarjetas()
        {
            return tarjetas.ToList();
        }
        public string[] toArray()
        {
            return new string[] { id_usuario.ToString(), nombre, bloqueado.ToString(), administrador.ToString() };
        }
    }
}
