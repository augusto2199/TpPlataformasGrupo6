using Microsoft.VisualBasic.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using InterfazTP.Data;
using Microsoft.EntityFrameworkCore;

namespace InterfazTP
{
    public class Banco
    {
        public Usuario usuarioActual
        {
            get; set;
        }

        private MyContext contexto;

        public Banco()
        {
            InicializarAtributos();
            OBSPLazosFijos();
        }

        // Llama a todos los datos de la BD
        private void InicializarAtributos()
        {
            try
            {
                contexto = new MyContext();
                contexto.usuarios.Include(u => u.cajas).Include(u => u.pf).Include(u => u.tarjetas).Include(u => u.pagos).Load();
                contexto.cajaAhorro.Include(c => c.Titulares).Include(c => c.movimientos).Load();
                contexto.pago.Load();
                contexto.tarjetaCredito.Load();
                contexto.movimiento.Load();
                contexto.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /******* METODOS ABM *******/

        /*                                     USUARIO                                     */
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
                try
                {
                    Usuario nuevoUsuario = new Usuario(Convert.ToInt32(dni), nombre, apellido, email, user, password, 0, false, false);

                    contexto.usuarios.Add(nuevoUsuario);
                    contexto.SaveChanges();

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }


        // Modificar datos de Usuario
        public bool ModificarUsuario(int id, string user, string password, string nombre, string apellido, string email)
        {
            bool result = false;

            Usuario u = contexto.usuarios.Where(u => u.id_usuario == id).FirstOrDefault();

            if (u != null)
            {
                u.nombre = user;
                u.password = password;
                u.nombre = nombre;
                u.apellido = apellido;
                u.email = email;

                contexto.usuarios.Update(u);
                contexto.SaveChanges();
                result = true;
            }
            return result;
        }

        // Eliminar Usuario
        public void EliminarUsuario(int usuarioId)
        {
            try
            {
                foreach (Usuario u in contexto.usuarios)
                {
                    if (u.id_usuario == usuarioId)
                    {
                        contexto.usuarios.Remove(u);
                        contexto.SaveChanges();
                    }
                }
            }
            catch (Exception ex) 
            {

            }
        
        }
        ////////////////////////////////////////////////////////////////////////////



        /*                                  CAJA DE AHORRO                                  */
        // Alta caja de ahorro
        public void AltaCajaAhorro()
        {
            CajaDeAhorro evenNumQuery = contexto.cajaAhorro.OrderByDescending(c => c.id).FirstOrDefault();
            int nuevoCbu = 0;
            if (evenNumQuery != null)
            {
                nuevoCbu = evenNumQuery.id;
            }

            try
            {

                CajaDeAhorro cajaAhorro = new CajaDeAhorro((nuevoCbu + 1), 0);

                if (usuarioActual != null)
                {
                    usuarioActual.cajas.Add(cajaAhorro);
                    contexto.usuarios.Update(usuarioActual);
                    contexto.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
        }

        // Baja caja de ahorro
        public bool BajaCajaAhorro(int idCaja)
        {
            try
            { 
            if (usuarioActual != null)
            {
                foreach (var c in MostrarTodasLasCajas())
                {
                    if (c.id == idCaja)
                    {
                        if (c.saldo == 0)
                        {
                            contexto.cajaAhorro.Remove(c);
                            contexto.SaveChanges();

                            return true;
                        }
                    }
                }
            }
            }
            catch (Exception e)
            {

            }
            return false;
        }

        // Deposita el monto recibido por parametro a la caja seleccionada por parametro y crea un movimiento
        public bool Depositar(int idCaja, float monto)
        {
            try
            {

            
            CajaDeAhorro ca = contexto.cajaAhorro.Where(ca => ca.id == idCaja).FirstOrDefault();

            if (ca != null)
            {
                ca.saldo = monto + ca.saldo;
                AltaMovimiento(ca, "Deposito", monto);
                contexto.cajaAhorro.Update(ca);
                contexto.SaveChanges();

                return true;
            }
            }
            catch (Exception e)
            {

            }

            return false;
        }

        // Retira monto recibido por parametro de la caja de ahorro seleccionada por parametro y crea un movimiento
        public bool Retirar(int CajaID, float monto)
        {
            try
            {
            CajaDeAhorro ca = contexto.cajaAhorro.Where(ca => ca.id == CajaID).FirstOrDefault();

            if (ca != null && ca.saldo > 0)
            {
                ca.saldo = ca.saldo - monto;
                AltaMovimiento(ca, "Retiro", monto);
                contexto.cajaAhorro.Update(ca);
                contexto.SaveChanges();

                return true;
            }
            }
            catch (Exception e)
            {

            }
            return false;
        }

        // Transfiere el monto recibido por parametro de una caja seleccionada por parametro a otra seleccionada por parametro
        public bool Transferir(int cajaOrigenCBU, int cajaDestinoCBU, float monto)
        {
            try
            {
            foreach (CajaDeAhorro c in MostrarTodasLasCajas())
            {
                if (cajaOrigenCBU == c.cbu && c.saldo >= monto && cajaOrigenCBU != cajaDestinoCBU)
                {
                    c.saldo -= monto;
                    foreach (CajaDeAhorro ca in MostrarTodasLasCajas())
                    {
                        if (cajaDestinoCBU == ca.cbu)
                        {
                            ca.saldo += monto;

                            AltaMovimiento(ca, "Transferencia Recibida", monto);
                            AltaMovimiento(c, "Transferencia Enviada", monto);

                            contexto.cajaAhorro.Update(ca);
                            contexto.cajaAhorro.Update(c);

                            contexto.SaveChanges();

                            return true;
                        }
                    }
                }
            }
            }
            catch (Exception e)
            {

            }
            return false;
        }

        // Devuelve datos necesarios para mostrar titular en forma de string(INTERFAZ)
        public List<Usuario> MostrarDatosTitular(int numCaja)
        {
            List<Usuario> titulares = new List<Usuario>();

                foreach (CajaDeAhorro c in contexto.cajaAhorro)
                {
                    if (c.cbu == numCaja)
                    {
                        foreach (Usuario u in c.Titulares)
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
            foreach (Usuario u in MostrarUsuarios())
            {
                foreach (Usuario ca in usuarios)
                {
                    if (u.id_usuario == ca.id_usuario)
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
                            c.Titulares.Add(t);
                            contexto.cajaAhorro.Update(c);
                            contexto.SaveChanges();
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
                if (t.dni == idUsuario)
                {
                    foreach (CajaDeAhorro c in this.MostrarTodasLasCajas())
                    {
                        if (c.cbu == numCaja)
                        {
                            if (c.Titulares.Count == 1)
                            {
                                return resultado;
                            }
                            else
                            {
                                t.cajas.Remove(c);
                                c.Titulares.Remove(t);
                                contexto.cajaAhorro.Update(c);
                                contexto.SaveChanges();
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
            c.agregarMovimiento(new Movimiento(detalle, monto, DateTime.Now, c));
        }

        // Lista de movimientos de caja de ahorro seleccionada por parametro
        public List<Movimiento> BuscarMovimiento(CajaDeAhorro cajas, String detalle, DateTime fecha, float monto)
        {
            List<Movimiento> nuevo = new List<Movimiento>();

            if (usuarioActual != null)
            {
                foreach (CajaDeAhorro usuarioInd in contexto.cajaAhorro)
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
            try
            {
                if (usuario != null)
                {
                    Pago pago = new Pago(monto, nombre, false);
                    //usuario.pagos.Add(pago);
                    usuarioActual.pagos.Add(pago);
                    contexto.usuarios.Update(usuarioActual);
                    contexto.SaveChanges();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        // Baja pago
        public bool BajaPago(int id)
        {
            try
            {
                foreach (var p in usuarioActual.pagos)
                {
                    if (p.id == id && p.pagado == true)
                    {
                        contexto.pago.Remove(p);
                        contexto.SaveChanges();

                        return true;
                    }
                }
            }
            catch
            {

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
                            c.agregarMovimiento(new Movimiento("Pago", p.monto, DateTime.Now, c));

                            contexto.pago.Update(p);
                            contexto.cajaAhorro.Update(c);
                            contexto.SaveChanges();

                            return true;
                        }
                    }

                    foreach (TarjetaDeCredito t in MostrarTarjetas())
                    {
                        if (t.numero == identificador && (t.limite - t.consumos) >= p.monto)
                        {
                            p.pagado = true;
                            t.consumos += p.monto;

                            contexto.pago.Update(p);
                            contexto.tarjetaCredito.Update(t);
                            contexto.SaveChanges();

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
            DateTime fechaCreacion = DateTime.Now;
            DateTime fechaFinalizacion = fechaCreacion.AddDays(30);

            foreach (CajaDeAhorro c in MostrarCajasDeAhorro())
            {
                if (c.cbu == cbuDestino && c.saldo >= monto && monto >= 1000)
                {
                    PlazoFijo pfj = new PlazoFijo(monto, fechaCreacion, fechaFinalizacion, 60, false);
                    usuarioActual.pf.Add(pfj);
                    contexto.usuarios.Update(usuarioActual);
                    contexto.plazoFijo.Add(pfj);
                    c.saldo -= pfj.monto;
                    c.agregarMovimiento(new Movimiento("Plazo Fijo", monto, DateTime.Now, c));
                    contexto.cajaAhorro.Update(c);
                    contexto.SaveChanges();

                    // pfj.id = idPlazoFijo;
                    return true;
                }
            }
            return false;
        }

        // Baja plazo fijo
        public bool BajaPlazoFijo(int id)
        {
            foreach (var p in MostrarPlazoFijos())
            {
                if (p.id == id)
                {
                    if (p.pagado == true && (DateTime.Now - p.fechaFin).TotalDays > 30)
                    {
                        usuarioActual.pf.Remove(p);
                        contexto.usuarios.Update(usuarioActual);
                        contexto.SaveChanges();

                        return true;
                    }
                }
            }
            return false;
        }

        // Interfaz
        public void cobrarPlazoFijo(int plazoFijoID)
        {
            foreach (PlazoFijo pf in MostrarPlazoFijos())
            {
                if (pf.id == plazoFijoID)
                {
                    usuarioActual.cajas.First().saldo = pf.monto + (pf.monto * (pf.getTasa() / 365));
                    pf.pagado = true;
                    contexto.plazoFijo.Update(pf);
                    contexto.SaveChanges();

                }
            }
        }

        // Cobrar plazos fijos si se cumplio la fecha
        private void OBSPLazosFijos()
        {
            foreach (PlazoFijo p in MostrarPlazoFijos())
            {
                if (DateTime.Now >= p.fechaFin && p.pagado != true)
                {
                    cobrarPlazoFijo(p.id);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////



        /*                            TARJETA DE CREDITO                         */
        // Alta tarjeta de credito
        public void AltaTarjetaCredito()
        {
            TarjetaDeCredito evenNumQuery = contexto.tarjetaCredito.OrderByDescending(t => t.id).FirstOrDefault();
            int nuevoCbu = 0;
            if (evenNumQuery != null)
            {
                nuevoCbu = evenNumQuery.id;
            }

            TarjetaDeCredito t = new TarjetaDeCredito((int.Parse("80000".ToString() + nuevoCbu.ToString()) + 1), (nuevoCbu + 1), 200000, 0);
            usuarioActual.tarjetas.Add(t);
            contexto.usuarios.Update(usuarioActual);
            contexto.SaveChanges();

        }
        // Baja tarjeta de credito
        public bool BajaTarjetaCredito(int numero_tarjeta)
        {
            foreach (TarjetaDeCredito t in MostrarTarjetas())
            {
                if (t.id == numero_tarjeta && t.consumos == 0)
                {
                    contexto.tarjetaCredito.Remove(t);
                    contexto.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool pagarTarjeta(int tarjeta, int cbu)
        {
            foreach (TarjetaDeCredito t in MostrarTarjetas())
            {
                if (t.id == tarjeta)
                {
                    foreach (CajaDeAhorro c in MostrarTodasLasCajas())
                    {
                        if (t.consumos <= c.saldo)
                        {
                            AltaMovimiento(c, "Pago Tarjeta", t.consumos);
                            c.saldo -= t.consumos;
                            t.limite += t.consumos;
                            t.consumos = 0;
                            contexto.cajaAhorro.Update(c);
                            contexto.tarjetaCredito.Update(t);
                            contexto.SaveChanges();

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
                foreach (Usuario usuarioInd in MostrarUsuarios())
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
                                    contexto.usuarios.Update(usuarioInd);
                                    contexto.SaveChanges();
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

        }

        public void desbloquearUsuario(int usuarioId)
        {
            try
            {
                foreach (Usuario u in MostrarUsuarios())
                {
                    if (usuarioId == u.id_usuario && u.bloqueado == true)
                    {
                        u.bloqueado = false;
                        u.intentosFallidos = 0;
                        contexto.usuarios.Update(u);
                        contexto.SaveChanges();
                    }
                }
            }
            catch(Exception i)
            {
                MessageBox.Show(i + "");
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
            return contexto.cajaAhorro.ToList();
        }

        // Lista de los movimientos de la caja de ahorro que se busca por id(parametro)
        public List<Movimiento> MostrarMovimientos(int cajaID)
        {
            List<Movimiento> movimientos = new List<Movimiento>();
            if (usuarioActual != null)
            {
                foreach (CajaDeAhorro c in MostrarTodasLasCajas())
                {
                    if (cajaID == c.id)
                    {
                      movimientos = c.movimientos.ToList();
                    }
                }
            }
            return movimientos;
        }

        // Lista de todos los pagos del usuario logueado
        public List<Pago> MostrarPagos()
        {
            return contexto.pago.ToList();
        }

        // Muestra todos los plazos fijos del usuario logueado
        public List<PlazoFijo> MostrarPlazoFijos()
        {
            return contexto.plazoFijo.ToList();
        }

        // Lista de todos los usuarios del banco
        public List<Usuario> MostrarUsuarios()
        {
            return contexto.usuarios.ToList();
        }

        // Lista de todas las tarjetas de credito del banco
        public List<TarjetaDeCredito> MostrarTarjetas()
        {
            return contexto.tarjetaCredito.ToList();
        }

        ///////////////////////////////////////////////////////////////////////



        public CajaDeAhorro buscarCaja(int id)
        {
            CajaDeAhorro res = null;
            foreach (CajaDeAhorro c in contexto.cajaAhorro)
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
