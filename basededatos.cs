using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace InterfazTP
{
    public class BaseDeDatos
    {
        private string connectionString;

        DateTime thisDay = DateTime.Today;

        public BaseDeDatos()
        {
            connectionString = Properties.Resources.connectionString;
        }

        public int crearUsuario(int Dni, string Nombre, string Apellido, string Mail, string usuario, string Password, bool admin, bool bloqueado)
        {
            //primero me aseguro que lo pueda agregar a la base
            int resultadoQuery;
            int idNuevoUsuario = -1;
            string connectionString = this.connectionString;
            string queryString = "INSERT INTO usuario ([dni],[nombre],[apellido],[mail],[usuario],[contrasenia],[intentos_fallidos],[bloqueado],[administrador]) VALUES (@dni,@nombre,@apellido,@mail,@usuario, @password,@fallidos,@bloqueado,@admin);";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@dni", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@nombre", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@apellido", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@mail", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@usuario", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@admin", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@fallidos", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@bloqueado", SqlDbType.Bit));

                command.Parameters["@dni"].Value = Dni;
                command.Parameters["@nombre"].Value = Nombre;
                command.Parameters["@apellido"].Value = Apellido;
                command.Parameters["@mail"].Value = Mail;
                command.Parameters["@usuario"].Value = usuario;
                command.Parameters["@password"].Value = Password;
                command.Parameters["@admin"].Value = 0;
                command.Parameters["@fallidos"].Value = 0;
                command.Parameters["@bloqueado"].Value = 0;
                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    resultadoQuery = command.ExecuteNonQuery();

                    //*******************************************
                    //Ahora hago esta query para obtener el ID
                    string ConsultaID = "SELECT MAX([usuario_id]) FROM usuario";
                    command = new SqlCommand(ConsultaID, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    idNuevoUsuario = reader.GetInt32(0);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return -1;
                }
                return idNuevoUsuario;
            }
        }

        public bool ModificarDatosUsuario(int id, string user, string password, string nombre, string apellido, string email)
        {
            string connectionString = this.connectionString;
            string queryString = "UPDATE [dbo].[usuario] SET [usuario] = @usuario, [contrasenia] = @password,[nombre] = @nombre, [apellido] = @apellido, [mail] = @email WHERE usuario_id=@id;";
            using (SqlConnection connection =
                 new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@usuario", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@nombre", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@apellido", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar));

                command.Parameters["@id"].Value = id;
                command.Parameters["@usuario"].Value = user;
                command.Parameters["@password"].Value = password;
                command.Parameters["@nombre"].Value = nombre;
                command.Parameters["@apellido"].Value = apellido;
                command.Parameters["@email"].Value = email;

                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query

                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        public bool Modificarbloqueo(int id)
        {
            string connectionString = this.connectionString;
            string queryString = "UPDATE [dbo].[usuario] SET bloqueado=@bloqueado, intentos_fallidos=@intentos WHERE usuario_id=@id;";
            using (SqlConnection connection =
                 new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@intentos", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@bloqueado", SqlDbType.Bit));


                command.Parameters["@id"].Value = id;
                command.Parameters["@intentos"].Value = 0;
                command.Parameters["@bloqueado"].Value = 0;


                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query

                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        public int crearCaja()
        {
            //primero me aseguro que lo pueda agregar a la base
            int resultadoQuery;
            int idCajaAhorro = -1;
            string queryString = "INSERT INTO [dbo].[caja_de_ahorro]([cbu],[saldo]) VALUES(@cbu,@saldo); ";
            using (SqlConnection connection =
                new SqlConnection(this.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@cbu", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@saldo", SqlDbType.Float));


                command.Parameters["@cbu"].Value = 0;
                command.Parameters["@saldo"].Value = 0;


                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    resultadoQuery = command.ExecuteNonQuery();

                    //*******************************************
                    //Ahora hago esta query para obtener el ID
                    string ConsultaID = "SELECT MAX([caja_id]) FROM caja_de_ahorro";
                    command = new SqlCommand(ConsultaID, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    idCajaAhorro = reader.GetInt32(0);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return -1;
                }
                return idCajaAhorro;
            }
        }

        public bool modificarCBUDeCaja(int id)
        {

            string connectionString = this.connectionString;
            string queryString = "UPDATE [dbo].[caja_de_ahorro] SET cbu = @cbu WHERE caja_id=@id;";
            using (SqlConnection connection =
                 new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@cbu", SqlDbType.Float));

                command.Parameters["@id"].Value = id;
                command.Parameters["@cbu"].Value = id;

                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query

                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

        }

        public int eliminarCaja(int idCaja)
        {
            string connectionString = Properties.Resources.connectionString;
            string queryString = "DELETE FROM caja_de_ahorro WHERE caja_id = @id";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters["@id"].Value = idCaja;
                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return 0;
                }
            }
        }

        public int eliminarCajaUsuario(int idCaja)
        {
            string connectionString = Properties.Resources.connectionString;
            string queryString = "DELETE FROM caja_usuario WHERE caja_fk = @id";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters["@id"].Value = idCaja;
                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    return command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        public int eliminarRelacionCajaYUsuario(int idCaja, int usuarios)
        {
            string connectionString = Properties.Resources.connectionString;
            string queryString = "DELETE FROM caja_usuario WHERE caja_fk = @id and usuario_fk=@usuario";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@usuario", SqlDbType.Int));
                command.Parameters["@id"].Value = idCaja;
                command.Parameters["@usuario"].Value = usuarios;
                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    return command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        public void VincularCuentas(int id_caja, int id_cliente)
        {
            //primero me aseguro que lo pueda agregar a la base
            int resultadoQuery;
            string queryString = "INSERT INTO caja_usuario ([caja_fk],[usuario_fk])VALUES (@cajafk ,@usuariofk); ";

            using (SqlConnection connection =
                new SqlConnection(this.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@cajafk", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@usuariofk", SqlDbType.Int));


                command.Parameters["@cajafk"].Value = id_caja;//verr
                command.Parameters["@usuariofk"].Value = id_cliente;


                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    resultadoQuery = command.ExecuteNonQuery();


                    //VincularCuentas();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }
        }

        public int crearTarjeta(int idUsuario, int numTarjeta, int codigoV)
        {
            //primero me aseguro que lo pueda agregar a la base
            int resultadoQuery;
            int idTarjeta = -1;

            var rand = new Random();

            string queryString = "INSERT INTO [dbo].[tarjeta_de_credito]([numero],[codigov],[limite],[consumos],[usuario_fk]) VALUES (@numero,@codigov,@limite,@consumos,@usuario_id);";
            using (SqlConnection connection =
                new SqlConnection(this.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@numero", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@codigov", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@limite", SqlDbType.Float));
                command.Parameters.Add(new SqlParameter("@consumos", SqlDbType.Float));
                command.Parameters.Add(new SqlParameter("@usuario_id", SqlDbType.Int));


                command.Parameters["@numero"].Value = numTarjeta;//deberia crear automatico
                command.Parameters["@codigov"].Value = codigoV;//deberia crear automatico
                command.Parameters["@limite"].Value = 100000;//monto fijo
                command.Parameters["@consumos"].Value = 0;//monto fijo
                command.Parameters["@usuario_id"].Value = idUsuario;
                //una vez creado deberiamos tener el id y vincular con el usuario iniciado 


                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    resultadoQuery = command.ExecuteNonQuery();

                    //*******************************************
                    //Ahora hago esta query para obtener el ID
                    string ConsultaID = "SELECT MAX([tarjeta_id]) FROM tarjeta_de_credito";
                    command = new SqlCommand(ConsultaID, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    idTarjeta = reader.GetInt32(0);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return -1;
                }
                return idTarjeta;
            }
        }

        public int eliminarTarjetaCredito(int idUsuario)
        {
            string connectionString = Properties.Resources.connectionString;
            string queryString = "DELETE FROM tarjeta_de_credito WHERE tarjeta_id = @id";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters["@id"].Value = idUsuario;
                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    return command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        public int crearPlazo(int idUsuario, float monto)
        {

            //primero me aseguro que lo pueda agregar a la base
            int resultadoQuery;
            int idPlazoFijo = -1;

            //string asd = "15/10/2022";
            //DateTime fecha = Convert.ToDateTime(asd, new CultureInfo("es-ES"));
            DateTime fechaCreacion = thisDay;
            DateTime fechaFinalizacion = fechaCreacion.AddDays(30);

            string connectionString = this.connectionString;
            string queryString = "INSERT INTO plazo_fijo ([monto],[fecha_ini],[fecha_fin],[tasa],[pagado],[usuario_fk]) VALUES (@monto,@fecha_ini,@fecha_fin,@tasa,@pagado,@usuarioid);";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@monto", SqlDbType.Float));
                command.Parameters.Add(new SqlParameter("@tasa", SqlDbType.Float));
                command.Parameters.Add(new SqlParameter("@fecha_ini", SqlDbType.Date));
                command.Parameters.Add(new SqlParameter("@fecha_fin", SqlDbType.Date));
                command.Parameters.Add(new SqlParameter("@pagado", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@usuarioid", SqlDbType.Int));

                command.Parameters["@monto"].Value = monto;
                command.Parameters["@tasa"].Value = 60;
                command.Parameters["@fecha_ini"].Value = fechaCreacion;
                command.Parameters["@fecha_fin"].Value = fechaFinalizacion;
                command.Parameters["@pagado"].Value = 0;//siempre en false
                command.Parameters["@usuarioid"].Value = idUsuario;
                //una vez creado deberiamos tener el id y vincular con el usuario iniciado 


                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    resultadoQuery = command.ExecuteNonQuery();

                    string ConsultaID = "SELECT MAX([plazo_id]) FROM plazo_fijo";
                    command = new SqlCommand(ConsultaID, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    idPlazoFijo = reader.GetInt32(0);
                    reader.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Console.WriteLine();
                    return -1;
                }
                return idPlazoFijo;
            }
        }

        public int crearPago(string nombre, int usuarioid, double monto)
        {
            //primero me aseguro que lo pueda agregar a la base
            int resultadoQuery;
            int idNuevoUsuario = -1;
            string queryString = "INSERT INTO pago ([monto],[pagado],[metodo],[nombre],[usuario_fk]) VALUES(@monto,@pagado,@metodo,@nombre,@usid);";
            using (SqlConnection connection =
                new SqlConnection(this.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@monto", SqlDbType.Float));
                command.Parameters.Add(new SqlParameter("@pagado", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@metodo", SqlDbType.VarChar));
                command.Parameters.Add(new SqlParameter("@nombre", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@usid", SqlDbType.Int));



                command.Parameters["@monto"].Value = monto;//hay que ver como ejecutar y preguntar si tiene plata jeje
                command.Parameters["@nombre"].Value = nombre;//ejemplo:Agua 
                command.Parameters["@metodo"].Value = "";//ver como ejecutar
                command.Parameters["@pagado"].Value = 0;
                command.Parameters["@usid"].Value = usuarioid;
                //una vez creado deberiamos tener el id y vincular con el usuario iniciado 


                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    resultadoQuery = command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return -1;
                }
                return idNuevoUsuario;
            }
        }

        public bool modificarPago(int idPago)
        {

            string connectionString = this.connectionString;
            string queryString = "UPDATE pago SET pagado = @pagado WHERE Pago_id=@id;";
            using (SqlConnection connection =
                 new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@pagado", SqlDbType.Bit));

                command.Parameters["@id"].Value = idPago;
                command.Parameters["@pagado"].Value = 1;

                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query

                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

        }

        public int eliminarPago(int idPago)
        {
            string connectionString = Properties.Resources.connectionString;
            string queryString = "DELETE FROM pago WHERE pago_id = @id";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters["@id"].Value = idPago;
                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    return command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        public int crearMovimiento(int id, string detalle, float monto)
        {
            //primero me aseguro que lo pueda agregar a la base
            int resultadoQuery;
            int idNuevoUsuario = -1;
            string queryString = "INSERT INTO movimiento([detalle],[monto],[fecha],[caja_fk])VALUES(@detalle,@monto,@fecha,@caja_id)";
            using (SqlConnection connection =
                new SqlConnection(this.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@detalle", SqlDbType.VarChar));
                command.Parameters.Add(new SqlParameter("@fecha", SqlDbType.Date));
                command.Parameters.Add(new SqlParameter("@monto", SqlDbType.Float));
                command.Parameters.Add(new SqlParameter("@caja_id", SqlDbType.Int));




                command.Parameters["@monto"].Value = monto;//hay que ver como ejecutar y preguntar si tiene plata jeje
                command.Parameters["@fecha"].Value = this.thisDay.ToString();
                command.Parameters["@detalle"].Value = detalle;
                command.Parameters["@caja_id"].Value = id;
                //una vez creado deberiamos tener el id y vincular con el usuario iniciado 


                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    resultadoQuery = command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                    return -1;
                }
                return idNuevoUsuario;
            }
        }

        public int eliminarMovimientoDeCaja(int idCaja)
        {
            string connectionString = Properties.Resources.connectionString;
            string queryString = "DELETE FROM movimiento WHERE caja_fk = @id";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters["@id"].Value = idCaja;
                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return 0;
                }
            }
        }

        public bool modificarSaldoDeCaja(float monto, int idCaja)
        {

            string connectionString = this.connectionString;
            string queryString = "UPDATE [dbo].[caja_de_ahorro] SET [saldo] = @saldo WHERE caja_id=@id;";
            using (SqlConnection connection =
                 new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@saldo", SqlDbType.Float));

                command.Parameters["@id"].Value = idCaja;
                command.Parameters["@saldo"].Value = monto;

                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query

                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

        }

        public bool modificarConsumoDeTarjeta(float monto, int tarjeta_id)
        {
            //hay que hacer otro para modicar el limite
            string connectionString = this.connectionString;
            string queryString = "UPDATE [dbo].[tarjeta_de_credito] SET [consumos] = @saldo WHERE tarjeta_id = @id;";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@saldo", SqlDbType.Float));

                command.Parameters["@id"].Value = tarjeta_id;
                command.Parameters["@saldo"].Value = monto;

                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

        }

        public bool modificarfinalizarPlazoFijo(int plazo_id)
        {

            string connectionString = this.connectionString;
            string queryString = "UPDATE [dbo].[plazo_fijo] SET [pagado] = @pago WHERE plazo_id = @id;";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@pago", SqlDbType.Bit));

                command.Parameters["@id"].Value = plazo_id;
                command.Parameters["@pago"].Value = true;
                //deberia llamar modicar modificarSaldoDeCaja
                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

        }


        //mostrar 

        //cliente
        public List<Usuario> inicializarUsuarios()
        {
            List<Usuario> usuariosss = new List<Usuario>();

            string queryString = "SELECT * from usuario";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    Usuario aux;

                    while (reader.Read())
                    {

                        aux = new Usuario(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetInt32(7), reader.GetBoolean(8), reader.GetBoolean(9));

                        usuariosss.Add(aux);
                    }

                    reader.Close();




                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return usuariosss;
        }
        //caja
        public List<CajaDeAhorro> mostrarCaja()
        {
            List<CajaDeAhorro> misCajasAhorro = new List<CajaDeAhorro>();

            string queryString = "SELECT * from caja_de_ahorro";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    CajaDeAhorro aux;
                    while (reader.Read())
                    {
                        aux = new CajaDeAhorro(reader.GetInt32(0), reader.GetInt32(1), reader.GetFloat(2));
                        misCajasAhorro.Add(aux);
                    }
                    reader.Close();



                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return misCajasAhorro;
        }
        //plazo
        public List<PlazoFijo> mostrarPlazoFijo()
        {
            List<PlazoFijo> misPlazoFijo = new List<PlazoFijo>();

            string queryString = "SELECT * from plazo_fijo";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    PlazoFijo aux;
                    while (reader.Read())
                    {
                        aux = new PlazoFijo(reader.GetInt32(0), reader.GetFloat(1), reader.GetDateTime(2), reader.GetDateTime(3), reader.GetFloat(4), reader.GetBoolean(5));
                        misPlazoFijo.Add(aux);
                    }
                    reader.Close();



                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            return misPlazoFijo;
        }
        //tarjeta
        public List<TarjetaDeCredito> mostrarTarjetaDeCredito()
        {
            List<TarjetaDeCredito> misTarjetas = new List<TarjetaDeCredito>();

            string queryString = "SELECT tarjeta_id,numero,codigov,limite,consumos from tarjeta_de_credito";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    TarjetaDeCredito aux;
                    while (reader.Read())
                    {
                        aux = new TarjetaDeCredito(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetFloat(3), reader.GetFloat(4));
                        misTarjetas.Add(aux);
                    }
                    reader.Close();



                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            return misTarjetas;
        }
        //movientos
        public List<Movimiento> mostrarMovimiento()
        {
            List<Movimiento> misMovimientos = new List<Movimiento>();

            string queryString = "SELECT * from movimiento";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    Movimiento aux;

                    while (reader.Read())
                    {
                        aux = new Movimiento(reader.GetInt32(0), reader.GetString(1), reader.GetFloat(2), reader.GetDateTime(3), reader.GetInt32(4)); 
                        misMovimientos.Add(aux);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            return misMovimientos;
        }
        //pago
        public List<Pago> mostrarPago()
        {
            List<Pago> misPagos = new List<Pago>();

            string queryString = "SELECT * from pago";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    Pago aux;

                    while (reader.Read())
                    {
                        // Revisar fecha

                        aux = new Pago(reader.GetInt32(0), reader.GetFloat(1), reader.GetString(2), reader.GetBoolean(3), reader.GetString(4));
                        misPagos.Add(aux);
                    }
                    reader.Close();



                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return misPagos;
        }



        public int numeroDeCaja()
        {
            int idNuevoUsuario;
            using (SqlConnection connection =
             new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query

                    //*******************************************
                    //Ahora hago esta query para obtener el ID
                    string ConsultaID = "SELECT MAX([caja_id]) FROM [dbo].[caja_de_ahorro]";
                    SqlCommand command = new SqlCommand(ConsultaID, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    idNuevoUsuario = reader.GetInt32(0);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }
                return idNuevoUsuario;


            }

        }

        //_______________________****************************____________________________________

        //muestra todos los usuarios que hay en la caja
        // hay que vincular con la clase Caja
        //CLASE PRINCIPAL CAJA_DE_AHORRO
        public List<Usuario> mostrarUsuarioEnCaja(int caja_id)
        {
            List<Usuario> usuar = new List<Usuario>();

            string queryString = "SELECT usuario_id,dni,nombre,apellido,mail,usuario,contrasenia,intentos_fallidos,bloqueado,administrador from usuario u inner join caja_usuario cu on(u.usuario_id=cu.usuario_fk) inner join caja_de_ahorro ca on( cu.caja_fk=ca.caja_id) where caja_id = @id; ";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters["@id"].Value = caja_id;

                try
                {

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    Usuario aux;

                    while (reader.Read())
                    {

                        aux = new Usuario(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetInt32(7), reader.GetBoolean(8), reader.GetBoolean(9));

                        usuar.Add(aux);
                    }

                    reader.Close();


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return usuar;
        }
        public List<Movimiento> mostrarMovimientoEnCaja(int caja_id)
        {
            List<Movimiento> usuar = new List<Movimiento>();

            string queryString = "SELECT movimiento_id,detalle,monto,fecha, caja_fk from caja_de_ahorro ca inner join movimiento mo on(ca.caja_id=mo.caja_fk)  where caja_id = @id; ";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters["@id"].Value = caja_id;

                try
                {

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    Movimiento aux;

                    while (reader.Read())
                    {

                        aux = new Movimiento(reader.GetInt32(0), reader.GetString(1), reader.GetFloat(2), reader.GetDateTime(3), reader.GetInt32(4));
                        usuar.Add(aux);
                    }

                    reader.Close();


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return usuar;
        }
        //








        //muestra todas las cajas que hay en el usuario
        //CLASE PRINCIPAL USUARIO
        public List<CajaDeAhorro> mostrarCajaEnUsuario(int caja_id)
        {
            List<CajaDeAhorro> usuar = new List<CajaDeAhorro>();

            string queryString = "SELECT caja_id,cbu,saldo from usuario u inner join caja_usuario cu on(u.usuario_id=cu.usuario_fk) inner join caja_de_ahorro ca on( cu.caja_fk=ca.caja_id) where usuario_id = @id; ";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters["@id"].Value = caja_id;

                try
                {

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    CajaDeAhorro aux;

                    while (reader.Read())
                    {

                        aux = new CajaDeAhorro(reader.GetInt32(0), reader.GetInt32(1), reader.GetFloat(2));

                        usuar.Add(aux);
                    }

                    reader.Close();


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return usuar;
        }
        public List<Pago> mostrarPagoEnUsuario(int usuario_id)
        {
            List<Pago> usuar = new List<Pago>();

            string queryString = "SELECT pago_id,monto,pago.nombre,pagado,metodo from usuario u inner join pago  on (u.usuario_id=pago.usuario_fk) where usuario_id = @id; ";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters["@id"].Value = usuario_id;

                try
                {

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    Pago aux;

                    while (reader.Read())
                    {

                        aux = new Pago(reader.GetInt32(0), reader.GetFloat(1), reader.GetString(2), reader.GetBoolean(3), reader.GetString(4));

                        usuar.Add(aux);
                    }

                    reader.Close();


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return usuar;
        }
        public List<TarjetaDeCredito> mostrarTarjetaEnUsuario(int usuario_id)
        {
            List<TarjetaDeCredito> usuar = new List<TarjetaDeCredito>();

            string queryString = "SELECT tarjeta_id,numero,codigov,limite,consumos from usuario u inner join tarjeta_de_credito ta  on (u.usuario_id=ta.usuario_fk) where usuario_id = @id;";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters["@id"].Value = usuario_id;

                try
                {

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    TarjetaDeCredito aux;

                    while (reader.Read())
                    {

                        aux = new TarjetaDeCredito(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetFloat(3), reader.GetFloat(4));

                        usuar.Add(aux);
                    }

                    reader.Close();


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return usuar;
        }

        public List<PlazoFijo> mostrarPlazoFijoEnUsuario(int usuario_id)
        {
            List<PlazoFijo> usuar = new List<PlazoFijo>();

            string queryString = "SELECT plazo_id,monto,fecha_ini,fecha_fin,tasa,pagado from usuario u inner join plazo_fijo pla  on (u.usuario_id=pla.usuario_fk) where usuario_id = @id; ";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters["@id"].Value = usuario_id;

                try
                {

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    PlazoFijo aux;

                    while (reader.Read())
                    {

                        aux = new PlazoFijo(reader.GetInt32(0), reader.GetFloat(1), reader.GetDateTime(2), reader.GetDateTime(3), reader.GetFloat(4), reader.GetBoolean(5));

                        usuar.Add(aux);
                    }

                    reader.Close();


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return usuar;
        }


    }
}
