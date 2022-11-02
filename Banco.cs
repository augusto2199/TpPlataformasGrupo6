using Microsoft.VisualBasic.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace InterfazTP
{
    public class Banco
    {
        public List<Usuario> usuario { get; set; }
        private List<CajaDeAhorro> caja { get; set; }
        private List<PlazoFijo> plazosFijos { get; set; }
        private List<TarjetaDeCredito> tarjetas { get; set; }
        private List<Pago> pagos { get; set; }
        private List<Movimiento> movimientos { get; set; }
        public Usuario usuarioActual { get; set; }
        public int cbu = 1000;

        private BaseDeDatos db;

        public Banco()
        {
            pagos = new List<Pago>();
            tarjetas = new List<TarjetaDeCredito>();
            movimientos = new List<Movimiento>();
            plazosFijos = new List<PlazoFijo>();
            caja = new List<CajaDeAhorro>();
            usuario = new List<Usuario>();
            db = new BaseDeDatos();
            InicializarAtributos();
        }

        // Llama a todos los datos de la BD
        private void InicializarAtributos()
        {
            this.usuario = db.inicializarUsuarios();
            this.caja = db.mostrarCaja();
            this.plazosFijos = db.mostrarPlazoFijo();
            this.tarjetas = db.mostrarTarjetaDeCredito();
            this.movimientos = db.mostrarMovimiento();
            this.pagos = db.mostrarPago();
        }

        /* METODOS ABM */
        // Registrar Usuario
        public bool AltaUsuario(string user, string password, string nombre, string apellido, string dni, string email)
        {
            if (password.Length < 8 || user.Length < 8)
            {
                MessageBox.Show("Usuario y Contraseña deben tener minimo 8 caracteres.");
                return false;
            }
            else
            {
                Usuario nuevoUsuario = new Usuario();
                nuevoUsuario.usuarioLogin = user;
                nuevoUsuario.password = password;
                nuevoUsuario.nombre = nombre;
                nuevoUsuario.apellido = apellido;
                nuevoUsuario.dni = dni;
                nuevoUsuario.email = email;
                nuevoUsuario.id = Convert.ToInt32(dni);

                usuario.Add(nuevoUsuario);
                return true;
            }
        }
        // Modificar datos de Usuario
        public bool ModificarUsuario(string user, string password, string nombre, string apellido, string email)
        {
            bool result = false;

            foreach (var usuario in usuario)
            {
                if (usuario.id == usuarioActual.id)
                {
                    usuario.usuarioLogin = user;
                    usuario.password = password;
                    usuario.nombre = nombre;
                    usuario.apellido = apellido;
                    usuario.email = email;

                    result = true;
                }
            }
            return result;
        }
        // Eliminar Usuario
        public void EliminarUsuario(int usuarioId)
        {
            foreach (Usuario u in this.MostrarUsuarios())
            {
                if(u.id == usuarioId)
                {
                    u.cajas.RemoveAll(c => c.id == usuarioId);
                    u.tarjetas.RemoveAll(c => c.id == usuarioId);
                    u.pf.RemoveAll(c => c.id == usuarioId);
                    u.pagos.RemoveAll(c => c.id == usuarioId);                    
                    this.usuario.Remove(u);
                }
            }
        }

        /* METODOS CAJA DE AHORRO*/
        // Alta caja de ahorro
        public void AltaCajaAhorro(Usuario usuarioActual)
        {

            if (usuarioActual != null)
            {
                // Usuario
                CajaDeAhorro nuevaCajaAhorro = new CajaDeAhorro();
                nuevaCajaAhorro.saldo = 0;
                nuevaCajaAhorro.cbu = cbu;
                nuevaCajaAhorro.titular.Add(usuarioActual);
                usuarioActual.cajas.Add(nuevaCajaAhorro);


                // Banco
                caja.Add(nuevaCajaAhorro);
                cbu++;

            }

        }
        // Baja caja de ahorro
        public bool BajaCajaAhorro(int id)
        {
            if (usuarioActual != null)
            {
                foreach (var c in usuarioActual.obtenerCajas())
                {
                    if (c.id == id)
                    {
                        if (c.saldo == 0)
                        {
                            usuarioActual.cajas.Remove(c);
                            caja.Remove(c);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        // Modificar datos de caja de ahorro
        public void ModificarCajaAhorro(int id)
        {

        }

        // Devuelve datos necesarios para mostrar titular en forma de string(INTERFAZ)
        public List<Usuario> MostrarDatosTitular(int numCaja)
        {
            List<Usuario> titulares = new List<Usuario>();

            for (int i = 0; i < this.MostrarUsuarios().Count; i++)
            {
                foreach (CajaDeAhorro c in caja)
                {
                    if (c.cbu == numCaja)
                    {
                        if (this.MostrarUsuarios()[i].cajas.Contains(c))
                        {
                            titulares.Add(this.MostrarUsuarios()[i]);
                        }
                        else
                        {

                        }
                    }
                }
            }
            return titulares.ToList();
        }

        // Devuelve datos necesarios para mostrar titulares disponibles para agregar(INTERFAZ)
        public List<Usuario> MostrarTitularesDisponibles(int numCaja)
        {
            List<Usuario> titulares = new List<Usuario>();

            for (int i = 0; i < this.MostrarUsuarios().Count; i++)
            {
                foreach (CajaDeAhorro c in caja)
                {
                    if (c.cbu == numCaja)
                    {
                        if (this.MostrarUsuarios()[i].cajas.Contains(c))
                        {

                        }
                        else
                        {
                            titulares.Add(this.MostrarUsuarios()[i]);
                        }
                    }
                }
            }
            return titulares.ToList();
        }

        // Agrega titular a caja de ahorro (INTERFAZ)
        public bool AgregarTitular(int idUsuario, int numCaja)
        {
            bool resultado = false;
            foreach (Usuario t in this.MostrarUsuarios())
            {
                if (t.id == idUsuario)
                {
                    foreach (CajaDeAhorro c in this.MostrarTodasLasCajas())
                    {
                        if (c.cbu == numCaja)
                        {
                            t.cajas.Add(c);
                            c.titular.Add(t);
                            resultado = true;
                        }
                    }
                }
            }
            return resultado;
        }

        // Eliminar titular a caja de ahorro
        public bool EliminarTitular(int idUsuario, int numCaja)
        {
            bool resultado = false;
            foreach (Usuario t in this.MostrarUsuarios())
            {
                if (t.id == idUsuario)
                {
                    foreach (CajaDeAhorro c in this.MostrarTodasLasCajas())
                    {
                        if (c.cbu == numCaja)
                        {
                            if (c.titular.Count == 1)
                            {
                                return resultado;
                            }
                            else
                            {
                                t.cajas.Remove(c);
                                c.titular.Remove(t);
                                resultado = true;
                            }
                        }
                    }
                }
            }
            return resultado;
        }

        public void AltaMovimiento(CajaDeAhorro c, string detalle, float monto)
        {
            c.agregarMovimiento(new Movimiento(c, detalle, monto));
        }

        public void AltaPago(Usuario usuario, string nombre, float monto)
        {
            Pago p = new Pago(usuario, nombre, monto);
            pagos.Add(p);
            usuarioActual.pagos.Add(p);
        }

        public bool BajaPago(int id)
        {
            foreach (var p in pagos)
            {
                if (p.id == id && p.pagado == true)
                {
                    pagos.Remove(p);
                    usuarioActual.pagos.Remove(p);
                    return true;
                }
            }
            return false;
        }

        public bool ModificarPago(int id, int identificador)
        {
            foreach (var p in pagos)
            {
                if (p.id == id && p.pagado == false)
                {
                    foreach (CajaDeAhorro c in usuarioActual.cajas)
                    {
                        if (c.cbu == identificador && c.saldo >= p.monto)
                        {
                            p.pagado = true;
                            c.saldo -= p.monto;
                            c.agregarMovimiento(new Movimiento(c, "Pago", p.monto));
                            return true;
                        }
                    }
                    foreach (TarjetaDeCredito t in usuarioActual.tarjetas)
                    {
                        if (t.numero == identificador && (t.limite - t.consumos) >= p.monto)
                        {
                            p.pagado = true;
                            t.consumos += p.monto;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool AltaPlazoFijo(Usuario u, float monto, int cbuDestino)
        {
            foreach (CajaDeAhorro c in caja)
            {
                if (c.cbu == cbuDestino && c.saldo >= monto && monto >= 1000)
                {
                    PlazoFijo pfj = new PlazoFijo(u, monto);
                    usuarioActual.pf.Add(pfj);
                    plazosFijos.Add(pfj);
                    c.saldo -= pfj.monto;
                    return true;
                }
            }
            return false;
        }

        public bool BajaPlazoFijo(int id)
        {
            foreach (var p in plazosFijos)
            {
                if (p.id == id)
                {
                    if (p.pagado == true && (DateTime.Now - p.fechaFin).TotalDays > 30)
                    {
                        usuarioActual.pf.Remove(p);
                        plazosFijos.Remove(p);
                        return true;
                    }
                }
            }
            return false;
        }

        public void cobrarPlazoFijo(int plazoFijoID)
        {
            foreach (PlazoFijo pf in plazosFijos)
            {
                if (pf.id == plazoFijoID)
                {
                    usuarioActual.cajas.First().saldo = pf.monto + (pf.monto * (pf.getTasa() / 365));
                    pf.pagado = true;
                }
            }
        }

        public void AltaTarjetaCredito()
        {
            TarjetaDeCredito t = new TarjetaDeCredito(usuarioActual);
            tarjetas.Add(t);
            usuarioActual.tarjetas.Add(t);
        }

        public bool BajaTarjetaCredito(int numero)
        {
            foreach (TarjetaDeCredito t in tarjetas)
            {
                if (t.id == numero && t.consumos == 0)
                {
                    tarjetas.Remove(t);
                    usuarioActual.tarjetas.Remove(t);
                    return true;
                }
            }
            return false;
        }

        public bool pagarTarjeta(int tarjeta, int cbu)
        {
            foreach (TarjetaDeCredito t in tarjetas)
            {
                if (t.id == tarjeta)
                {
                    foreach (CajaDeAhorro c in caja)
                    {
                        if (t.consumos <= c.saldo)
                        {
                            c.saldo -= t.consumos;
                            t.limite += t.consumos;
                            t.consumos = 0;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // Metodo de Login
        public bool IniciarSesion(string usuario, string contrasenia)
        {
            bool encontrado = false;
            try
            {
                foreach (Usuario usuarioInd in this.usuario)
                {

                    if (usuarioInd.usuarioLogin == usuario)
                    {

                        if (usuarioInd.bloqueado == false)
                        {
                            // agregar verificacion usuario
                            if (usuarioInd.password == contrasenia)
                            {
                                usuarioActual = usuarioInd;
                                usuarioInd.intentosFallidos = 0;
                                encontrado = true;
                            }
                            else
                            {
                                usuarioInd.intentosFallidos++;

                                if (usuarioInd.intentosFallidos == 4)
                                {

                                    usuarioInd.bloqueado = true;
                                }

                            }
                        }
                    }
                }

            }
            catch (Exception i)
            {

                Console.WriteLine("error de " + i);
            }
            return encontrado;
        }

        public void CerrarSesion()
        {
            usuarioActual = null;
        }

        public bool Depositar(int idCaja, float monto)
        {
            if (usuarioActual != null)
            {
                foreach (CajaDeAhorro c in usuarioActual.cajas)
                {

                    if (c.id == idCaja)
                    {
                        c.saldo = monto + c.saldo;
                        AltaMovimiento(c, "Deposito", monto);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Retirar(int CajaID, float monto)
        {
            if (usuarioActual != null)
            {
                foreach (CajaDeAhorro c in usuarioActual.cajas)
                {
                    if (c.saldo >= monto && c.id == CajaID)
                    {
                        c.saldo = c.saldo - monto;
                        AltaMovimiento(c, "Retiro", monto);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Transferir(int cajaOrigenCBU, int cajaDestinoCBU, float monto)
        {
            foreach (CajaDeAhorro c in usuarioActual.cajas)
            {
                if (cajaOrigenCBU == c.cbu && c.saldo >= monto && cajaOrigenCBU != cajaDestinoCBU)
                {
                    c.saldo -= monto;
                    foreach (CajaDeAhorro ca in usuarioActual.cajas)
                    {
                        if (cajaDestinoCBU == ca.cbu)
                        {
                            ca.saldo += monto;
                            AltaMovimiento(ca, "Transferencia Recibida", monto);
                            AltaMovimiento(c, "Transferencia Enviada", monto);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // MOSTRAR DATOS - Nico
        // Cajas de ahorro usuario loguead
        public List<CajaDeAhorro> MostrarCajasDeAhorro()
        {
            return this.usuarioActual.cajas.ToList();
        }

        public List<CajaDeAhorro> MostrarTodasLasCajas()
        {
            return caja.ToList();
        }

        public List<Movimiento> MostrarMovimientos(int cajaID)
        {
            if (usuarioActual != null)
            {
                foreach (CajaDeAhorro c in usuarioActual.cajas)
                {
                    if (cajaID == c.id)
                    {
                        return c.movimientos.ToList();
                    }
                }
            }
            return null;
        }
        public List<Pago> MostrarPagos()
        {
            return usuarioActual.pagos.ToList();
        }

        public List<PlazoFijo> MostrarPlazoFijos()
        {
            return usuarioActual.pf.ToList();
        }

        public List<Usuario> MostrarUsuarios()
        {
            return usuario.ToList();
        }

        ///////////////////////////////////////////////////////////////////////
        
        public List<Movimiento> BuscarMovimiento(CajaDeAhorro cajas, String detalle, DateTime fecha, float monto)
        {
            List<Movimiento> nuevo = new List<Movimiento>();

            if (usuarioActual != null)
            {
                foreach (CajaDeAhorro usuarioInd in usuarioActual.cajas)
                {
                    if (usuarioInd.id == cajas.id)
                    {
                        foreach (Movimiento movi in usuarioInd.movimientos)
                        {
                            if ((movi.detalle == detalle) || (movi.fecha == fecha) || (movi.monto == monto))
                            {
                                nuevo.Add(movi);
                            }
                        }
                    }
                }
            }
            return nuevo;
        }

        public CajaDeAhorro buscarCaja(int id)
        {
            CajaDeAhorro res = null;
            foreach (CajaDeAhorro c in caja)
            {
                if (c.id == id)
                {
                    res = c;
                }
            }
            return res;
        }
    }
}
