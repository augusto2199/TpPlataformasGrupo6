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
        public List<Usuario> usuario
        {
            get; set;
        }
        private List<CajaDeAhorro> caja
        {
            get; set;
        }
        private List<PlazoFijo> plazosFijos
        {
            get; set;
        }
        private List<TarjetaDeCredito> tarjetas
        {
            get; set;
        }
        private List<Pago> pagos
        {
            get; set;
        }
        private List<Movimiento> movimientos
        {
            get; set;
        }
        public Usuario usuarioActual
        {
            get; set;
        }

        public int totalCaja;

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
            recorridoUsuio();

        }

        // Llama a todos los datos de la BD
        private void InicializarAtributos()
        {
            usuario = db.inicializarUsuarios();
            caja = db.mostrarCaja();
            plazosFijos = db.mostrarPlazoFijo();
            tarjetas = db.mostrarTarjetaDeCredito();
            movimientos = db.mostrarMovimiento();
            pagos = db.mostrarPago();
        }
        private void recorridoUsuio() { 
            foreach (Usuario usu in usuario)
            {
                usu.InicializarAtributos(usu.id);
            }        
        
        }
        /******* METODOS ABM *******/

        /*                                     USUARIO                                     */
        // Registrar Usuario
        public bool AltaUsuario(string user, string password, string nombre, string apellido, string dni, string email, bool esAdmin = false)
        {
            if (password.Length < 8 || user.Length < 8)
            {
                MessageBox.Show("Usuario y Contraseña deben tener minimo 8 caracteres.");
                return false;
            }
            else
            {
                int identificador = db.crearUsuario(Convert.ToInt32(dni), nombre, apellido, email, user, password, true, false);

                Usuario nuevoUsuario = new Usuario();
                nuevoUsuario.usuario = user;
                nuevoUsuario.password = password;
                nuevoUsuario.nombre = nombre;
                nuevoUsuario.apellido = apellido;
                nuevoUsuario.dni = Convert.ToInt32(dni);
                nuevoUsuario.email = email;
                nuevoUsuario.id = identificador;
                nuevoUsuario.administrador = esAdmin;

                usuario.Add(nuevoUsuario);

                return true;

            }
        }

        // Modificar datos de Usuario
        public bool ModificarUsuario(int id, string user, string password, string nombre, string apellido, string email)
        {
            bool result = false;

            foreach (var usuario in usuario)
            {
                if (usuario.id == id)
                {
                    usuario.nombre = user;
                    usuario.password = password;
                    usuario.nombre = nombre;
                    usuario.apellido = apellido;
                    usuario.email = email;
                    db.ModificarDatosUsuario(id, user, password, nombre, apellido, email);
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
                if (u.id == usuarioId)
                {
                    u.cajas.RemoveAll(c => c.id == usuarioId);
                    u.tarjetas.RemoveAll(c => c.id == usuarioId);
                    u.pf.RemoveAll(c => c.id == usuarioId);
                    u.pagos.RemoveAll(c => c.id == usuarioId);
                    this.usuario.Remove(u);
                }
            }
        }
        ////////////////////////////////////////////////////////////////////////////



        /*                                  CAJA DE AHORRO                                  */
        // Alta caja de ahorro
        public void AltaCajaAhorro()
        {
            if (usuarioActual != null)
            {
                // Usuario
                int idCajaAhorro = db.crearCaja();
                db.modificarCBUDeCaja(idCajaAhorro);

                CajaDeAhorro nuevaCajaAhorro = new CajaDeAhorro();
                nuevaCajaAhorro.saldo = 0;
                nuevaCajaAhorro.cbu = idCajaAhorro;
                nuevaCajaAhorro.titular.Add(usuarioActual);
                nuevaCajaAhorro.id = idCajaAhorro;

                usuarioActual.cajas.Add(nuevaCajaAhorro);

                db.VincularCuentas(idCajaAhorro, usuarioActual.id);

                // Banco
                caja.Add(nuevaCajaAhorro);

            }
        }

        // Baja caja de ahorro
        public bool BajaCajaAhorro(int idCaja)
        {
            if (usuarioActual != null)
            {
                foreach (var c in caja)
                {
                    if (c.id == idCaja)
                    {
                        if (c.saldo == 0)
                        {
                            usuarioActual.cajas.Remove(c);
                            caja.Remove(c);

                            db.eliminarCajaUsuario(idCaja);
                            db.eliminarMovimientoDeCaja(idCaja);
                            db.eliminarCaja(idCaja);
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

        // Deposita el monto recibido por parametro a la caja seleccionada por parametro y crea un movimiento
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

                        db.crearMovimiento(idCaja, "Deposito", monto);
                        db.modificarSaldoDeCaja(c.saldo, idCaja);
                        return true;
                    }
                }
            }
            return false;
        }

        // Retira monto recibido por parametro de la caja de ahorro seleccionada por parametro y crea un movimiento
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

                        db.crearMovimiento(CajaID, "Retiro", monto);
                        db.modificarSaldoDeCaja(c.saldo, CajaID);
                        return true;
                    }
                }
            }
            return false;
        }

        // Transfiere el monto recibido por parametro de una caja seleccionada por parametro a otra seleccionada por parametro
        public bool Transferir(int cajaOrigenCBU, int cajaDestinoCBU, float monto)
        {
            foreach (CajaDeAhorro c in usuarioActual.cajas)
            {
                if (cajaOrigenCBU == c.cbu && c.saldo >= monto && cajaOrigenCBU != cajaDestinoCBU)
                {
                    c.saldo -= monto;
                    foreach (CajaDeAhorro ca in caja)
                    {
                        if (cajaDestinoCBU == ca.cbu)
                        {
                            ca.saldo += monto;

                            AltaMovimiento(ca, "Transferencia Recibida", monto);
                            AltaMovimiento(c, "Transferencia Enviada", monto);

                            db.crearMovimiento(ca.id, "Transferencia Recibida", monto);
                            db.crearMovimiento(c.id, "Transferencia Enviada", monto);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // Devuelve datos necesarios para mostrar titular en forma de string(INTERFAZ)
        public List<Usuario> MostrarDatosTitular(int numCaja)
        {
            List<Usuario> titulares = new List<Usuario>();

            foreach(Usuario u in usuario)
            {
                foreach(CajaDeAhorro c in u.cajas)
                {
                    if(c.id == numCaja)
                    {
                        titulares.Add(u);
                    }
                }
            }

            return titulares.ToList();
        }

        // Devuelve datos necesarios para mostrar titulares disponibles para agregar(INTERFAZ)
        public List<Usuario> MostrarTitularesDisponibles(List<Usuario> usuarios)
        {
            List<Usuario> titulares = new List<Usuario>();
            titulares = MostrarUsuarios();
            foreach (Usuario u in this.usuario) { 
                foreach (Usuario ca in usuarios)
                {
                    if (u.id==ca.id)
                    {
                        titulares.Remove(ca);
                    }
                    
                    
                
                }
            }



            return titulares.ToList();
        }

        // Agrega titular a caja de ahorro
        public bool AgregarTitular(int idUsuario, int numCaja)
        {
            bool resultado = false;
            foreach (Usuario t in this.MostrarUsuarios())
            {
                if (t.dni == idUsuario)
                {
                    foreach (CajaDeAhorro c in this.MostrarTodasLasCajas())
                    {
                        if (c.cbu == numCaja)
                        {
                            t.cajas.Add(c);
                            c.titular.Add(t);
                            resultado = true;

                            db.VincularCuentas(numCaja, t.id);
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
                if (t.dni == idUsuario)
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
                                db.eliminarRelacionCajaYUsuario(numCaja, t.id);
                                resultado = true;
                            }
                        }
                    }
                }
            }
            return resultado;
        }
        ////////////////////////////////////////////////////////////////////////////



        /*                             MOVIMIENTO                                 */
        // Alta movimiento
        public void AltaMovimiento(CajaDeAhorro c, string detalle, float monto)
        {
            c.agregarMovimiento(new Movimiento(c, detalle, monto));
        }

        // Lista de movimientos de caja de ahorro seleccionada por parametro
        public List<Movimiento> BuscarMovimiento(CajaDeAhorro cajas, String detalle, DateTime fecha, float monto)
        {
            List<Movimiento> nuevo = new List<Movimiento>();

            if (usuarioActual != null)
            {
                foreach (CajaDeAhorro usuarioInd in caja)
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
        ////////////////////////////////////////////////////////////////////////////




        /*                                 PAGO                                    */
        // Alta pago
        public void AltaPago(Usuario usuario, string nombre, float monto)
        {
            Pago p = new Pago(usuario, nombre, monto);
            pagos.Add(p);
            usuarioActual.pagos.Add(p);

            db.crearPago(nombre, usuario.id, monto);
        }
        // Baja pago
        public bool BajaPago(int id)
        {
            foreach (var p in usuarioActual.pagos)
            {
                if (p.id == id && p.pagado == true)
                {
                    pagos.Remove(p);
                    usuarioActual.pagos.Remove(p);

                    db.eliminarPago(p.id);

                    return true;
                }
            }
            return false;
        }
        // Modificar pago
        public bool ModificarPago(int id, int identificador)
        {
            foreach (var p in usuarioActual.pagos)
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
                            db.crearMovimiento(c.id, "Pago", p.monto);
                            db.modificarSaldoDeCaja(c.saldo, c.id);
                            db.modificarPago(p.id);
                            return true;
                        }
                    }

                    foreach (TarjetaDeCredito t in tarjetas)
                    {
                        if (t.numero == identificador && (t.limite - t.consumos) >= p.monto)
                        {
                            p.pagado = true;
                            t.consumos += p.monto;

                            db.modificarConsumoDeTarjeta(p.monto, t.id);
                            db.modificarPago(p.id);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        ////////////////////////////////////////////////////////////////////////////



        /*                               PLAZO FIJO                              */
        // Alta plazo fijo
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
                    c.agregarMovimiento(new Movimiento(c, "Plazo Fijo", monto));
                    movimientos.Add(new Movimiento(c, "Plazo Fijo", monto));

                    db.crearMovimiento(c.id, "Plazo fijo", monto);
                    db.modificarSaldoDeCaja(c.saldo, c.id);
                    int idPlazoFijo = db.crearPlazo(u.id, monto);
                    pfj.id = idPlazoFijo;
                    return true;
                }
            }
            return false;
        }
        // Baja plazo fijo
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

                        db.modificarfinalizarPlazoFijo(id);
                        return true;
                    }
                }
            }
            return false;
        }
        // Interfaz
        public void cobrarPlazoFijo(int plazoFijoID)
        {
            foreach (PlazoFijo pf in plazosFijos)
            {
                if (pf.id == plazoFijoID)
                {
                    usuarioActual.cajas.First().saldo = pf.monto + (pf.monto * (pf.getTasa() / 365));
                    pf.pagado = true;

                    db.modificarfinalizarPlazoFijo(plazoFijoID);
                }
            }
        }
        ////////////////////////////////////////////////////////////////////////////



        /*                            TARJETA DE CREDITO                         */
        // Alta tarjeta de credito
        public void AltaTarjetaCredito()
        {
            var rand = new Random();

            TarjetaDeCredito t = new TarjetaDeCredito(usuarioActual);
            t.numero = rand.Next(10000000, 99999999);
            t.codigoV = rand.Next(100, 999);
            t.consumos = 0;
            t.limite = 100000;
            
            // Bd
            int idTarjetaCredito = db.crearTarjeta(usuarioActual.id, t.numero, t.codigoV);

            t.id = idTarjetaCredito;
            tarjetas.Add(t);
            usuarioActual.tarjetas.Add(t);

        }
        // Baja tarjeta de credito
        public bool BajaTarjetaCredito(int numero_tarjeta)
        {
            foreach (TarjetaDeCredito t in usuarioActual.tarjetas)
            {
                if (t.id == numero_tarjeta && t.consumos == 0)
                {
                    tarjetas.Remove(t);
                    usuarioActual.tarjetas.Remove(t);

                    db.eliminarTarjetaCredito(t.id);
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
                            c.movimientos.Add(new Movimiento(c, "Pago Tarjeta", t.consumos));

                            db.crearMovimiento(c.id, "Pago de tarjeta" + t.id, t.consumos);
                            db.modificarSaldoDeCaja(c.saldo, c.id);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        ////////////////////////////////////////////////////////////////////////////

        /*                              LOGIN                                     */
        // Login
        public bool IniciarSesion(string usuario, string contrasenia)
        {
            bool encontrado = false;
            try
            {
                foreach (Usuario usuarioInd in this.usuario)
                {

                    if (usuarioInd.usuario == usuario)
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
        // Cerrar Sesion
        public void CerrarSesion()
        {
            usuarioActual = null;
        }
        ////////////////////////////////////////////////////////////////////////////




        /*                               ADMIN                                         */
        public void AltaAdmin(string user, string password)
        {
            Usuario admin = new Usuario();


            admin.nombre = user;
            admin.password = password;
            admin.administrador = true;

            usuario.Add(admin);
        }

        public void desbloquearUsuario(int usuarioId)
        {
            foreach (Usuario u in usuario)
            {
                if (usuarioId == u.id && u.bloqueado == true)
                {
                    u.bloqueado = false;
                    db.Modificarbloqueo(usuarioId);
                }
            }
        }
        ///////////////////////////////////////////////////////////////////////



        /*                      MOSTRAR DATOS(Listas varias)                         */
        // Lista de las cajas de ahorro del usuario logueado
        public List<CajaDeAhorro> MostrarCajasDeAhorro()
        {
            return this.usuarioActual.cajas.ToList();
        }

        // Lista de todas las cajas de ahorro del banco
        public List<CajaDeAhorro> MostrarTodasLasCajas()
        {
            return caja.ToList();
        }

        // Lista de los movimientos de la caja de ahorro que se busca por id(parametro)
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

        // Lista de todos los pagos del usuario logueado
        public List<Pago> MostrarPagos()
        {
            return usuarioActual.pagos.ToList();
        }

        // Muestra todos los plazos fijos del usuario logueado
        public List<PlazoFijo> MostrarPlazoFijos()
        {
            return usuarioActual.pf.ToList();
        }

        // Lista de todos los usuarios del banco
        public List<Usuario> MostrarUsuarios()
        {
            return usuario.ToList();
        }

        // Lista de todas las tarjetas de credito del banco
        public List<TarjetaDeCredito> MostrarTarjetas()
        {
            return tarjetas.ToList();
        }

        ///////////////////////////////////////////////////////////////////////



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
