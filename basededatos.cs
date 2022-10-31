using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

 namespace ConsoleApp1
{    
        public class BaseDeDatos
        {
            private string connectionString;
            DateTime thisDay = DateTime.Today;

            public BaseDeDatos()
            {
                this.connectionString = @"Data Source=GRAZIANO\SQLEXPRESS;Initial Catalog=Banco;Integrated Security=True";

            }


         
            public int crearUsuario(int Dni, string Nombre, string Apellido, string Mail, string Password, bool admin, bool bloqueado)
        {
            //primero me aseguro que lo pueda agregar a la base
            int resultadoQuery;
            int idNuevoUsuario = -1;
            string connectionString = this.connectionString;
            string queryString = "INSERT INTO usuario ([dni],[nombre],[apellido],[mail],[contrasenia],[intentos_fallidos],[bloqueado],[admin]) VALUES (@dni,@nombre,@apellido,@mail,@password,@fallidos,@bloqueado,@admin);";
            using SqlConnection connection =
                new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.Add(new SqlParameter("@dni", SqlDbType.Int));
            command.Parameters.Add(new SqlParameter("@nombre", SqlDbType.NVarChar));
            command.Parameters.Add(new SqlParameter("@apellido", SqlDbType.NVarChar));
            command.Parameters.Add(new SqlParameter("@mail", SqlDbType.NVarChar));
            command.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar));
            command.Parameters.Add(new SqlParameter("@admin", SqlDbType.Bit));
            command.Parameters.Add(new SqlParameter("@fallidos", SqlDbType.Bit));
            command.Parameters.Add(new SqlParameter("@bloqueado", SqlDbType.Bit));
            command.Parameters["@dni"].Value = Dni;
            command.Parameters["@nombre"].Value = Nombre;
            command.Parameters["@apellido"].Value = Apellido;
            command.Parameters["@mail"].Value = Mail;
            command.Parameters["@password"].Value = Password;
            command.Parameters["@admin"].Value = admin;
            command.Parameters["@fallidos"].Value = 0;
            command.Parameters["@bloqueado"].Value = bloqueado;
            try
            {
                connection.Open();
                //esta consulta NO espera un resultado para leer, es del tipo NON Query
                resultadoQuery = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            return idNuevoUsuario;
        }

            public int crearCaja( int caja_usuario_fk)
            {   
                //primero me aseguro que lo pueda agregar a la base
                int resultadoQuery;
                int idNuevoUsuario = -1;
                string queryString = "INSERT INTO [dbo].[caja_de_ahorro]([cbu],[saldo]) VALUES(@cbu,@saldo); ";
                using (SqlConnection connection =
                    new SqlConnection(this.connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add(new SqlParameter("@cbu", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@saldo", SqlDbType.Float));


                     command.Parameters["@cbu"].Value = 12345678;//verr
                     command.Parameters["@saldo"].Value = 0;
                
               
                    try
                    {
                        connection.Open();
                        //esta consulta NO espera un resultado para leer, es del tipo NON Query
                        resultadoQuery = command.ExecuteNonQuery();

                        
                        //VincularCuentas();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("HOLAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                        Console.WriteLine(ex.Message);
                        return -1;
                    }
                        return idNuevoUsuario;
                }
            }

            public static void VincularCuentas()
            {
                //hay que unir con la cuenta que esta "iniciado sesion" con la" Caja_Usario" y la cuenta de "caja de ahorro"


            }

            public int crearTarjeta()
        {
            //primero me aseguro que lo pueda agregar a la base
            int resultadoQuery;
            int idNuevoUsuario = -1;
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



                command.Parameters["@numero"].Value = 12345678;//deberia crear automatico
                command.Parameters["@codigov"].Value = 3;//deberia crear automatico
                command.Parameters["@limite"].Value = 10000;//monto fijo
                command.Parameters["@consumos"].Value = 0;//monto fijo
                command.Parameters["@usuario_id"].Value = 0;
                //una vez creado deberiamos tener el id y vincular con el usuario iniciado 


                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    resultadoQuery = command.ExecuteNonQuery();

                    //*******************************************
                    //Ahora hago esta query para obtener el ID
                    /*string ConsultaID = "SELECT MAX([id_caja_de_ahorro]) FROM [dbo].[caja_de_ahorro]";
                    command = new SqlCommand(ConsultaID, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    idNuevoUsuario = reader.GetInt32(0);
                    reader.Close();*/
                    //VincularCuentas();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("HOLAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                    Console.WriteLine(ex.Message);
                    return -1;
                }
                return idNuevoUsuario;
            }
        }


            public int crearPlazo()
            {
            //primero me aseguro que lo pueda agregar a la base
            int resultadoQuery;
            int idNuevoUsuario = -1;
            string queryString = "INSERT INTO [dbo].[plazo_fijo]([monto],[fecha_ini],[tasa],[pagado],[usuario_fk]) VALUES (@monto,@fecha_ini,@tasa,@pagado,@usuario_id);";
            using (SqlConnection connection =
                new SqlConnection(this.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@fecha_ini", SqlDbType.Date));
                command.Parameters.Add(new SqlParameter("@pagado", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@monto", SqlDbType.Float));
                command.Parameters.Add(new SqlParameter("@tasa", SqlDbType.Float));
                command.Parameters.Add(new SqlParameter("@usuario_id", SqlDbType.Int));



                command.Parameters["@monto"].Value = 12345678;//hay que ver como ejecutar
                command.Parameters["@tasa"].Value = 3;//tiene que ser siempre el mismo
                command.Parameters["@fecha_ini"].Value = this.thisDay.ToString(); 
                command.Parameters["@pagado"].Value = false;//siempre en false
                command.Parameters["@usuario_id"].Value = 0;
                //una vez creado deberiamos tener el id y vincular con el usuario iniciado 


                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    resultadoQuery = command.ExecuteNonQuery();

                    //*******************************************
                    //Ahora hago esta query para obtener el ID
                    /*string ConsultaID = "SELECT MAX([id_caja_de_ahorro]) FROM [dbo].[caja_de_ahorro]";
                    command = new SqlCommand(ConsultaID, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    idNuevoUsuario = reader.GetInt32(0);
                    reader.Close();*/
                    //VincularCuentas();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("HOLAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                    Console.WriteLine(ex.Message);
                    return -1;
                }
                return idNuevoUsuario;
            }
            }

            public int crearPago()
        {
            //primero me aseguro que lo pueda agregar a la base
            int resultadoQuery;
            int idNuevoUsuario = -1;
            string queryString = "INSERT INTO [dbo].[pago]([monto],[pagado],[metodo],[nombre],[usuario_fk]) VALUES(@monto,@pagado,@metodo,@nombre,@usuario_id);";
            using (SqlConnection connection =
                new SqlConnection(this.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@metodo", SqlDbType.VarChar));
                command.Parameters.Add(new SqlParameter("@pagado", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@monto", SqlDbType.Float));
                command.Parameters.Add(new SqlParameter("@nombre", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@usuario_id", SqlDbType.Int));



                command.Parameters["@monto"].Value = 12345678;//hay que ver como ejecutar y preguntar si tiene plata jeje
                command.Parameters["@nombre"].Value = "agua";//ejemplo:Agua 
                command.Parameters["@metodo"].Value = "";//ver como ejecutar
                command.Parameters["@pagado"].Value = true;//siempre en true
                command.Parameters["@usuario_id"].Value = 0;
                //una vez creado deberiamos tener el id y vincular con el usuario iniciado 


                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    resultadoQuery = command.ExecuteNonQuery();

                    //*******************************************
                    //Ahora hago esta query para obtener el ID
                    /*string ConsultaID = "SELECT MAX([id_caja_de_ahorro]) FROM [dbo].[caja_de_ahorro]";
                    command = new SqlCommand(ConsultaID, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    idNuevoUsuario = reader.GetInt32(0);
                    reader.Close();*/
                    //VincularCuentas();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("HOLAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                    Console.WriteLine(ex.Message);
                    return -1;
                }
                return idNuevoUsuario;
            }
        }


            public int crearMovimiento()
        {
            //primero me aseguro que lo pueda agregar a la base
            int resultadoQuery;
            int idNuevoUsuario = -1;
            string queryString = "INSERT INTO [dbo].[movimiento]([detalle],[monto],[fecha],[caja_fk])VALUES(@detalle,@monto,@fecha,@caja_id)";
            using (SqlConnection connection =
                new SqlConnection(this.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@detalle", SqlDbType.VarChar));
                command.Parameters.Add(new SqlParameter("@fecha", SqlDbType.Date));
                command.Parameters.Add(new SqlParameter("@monto", SqlDbType.Float));
                command.Parameters.Add(new SqlParameter("@caja_id", SqlDbType.Int));




                command.Parameters["@monto"].Value = 12345678;//hay que ver como ejecutar y preguntar si tiene plata jeje
                command.Parameters["@fecha"].Value = this.thisDay.ToString() ; 
                command.Parameters["@detalle"].Value = "Que onda";
                command.Parameters["@caja_id"].Value = 12345678;
                //una vez creado deberiamos tener el id y vincular con el usuario iniciado 


                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    resultadoQuery = command.ExecuteNonQuery();

                    //*******************************************
                    //Ahora hago esta query para obtener el ID
                    /*string ConsultaID = "SELECT MAX([id_caja_de_ahorro]) FROM [dbo].[caja_de_ahorro]";
                    command = new SqlCommand(ConsultaID, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    idNuevoUsuario = reader.GetInt32(0);
                    reader.Close();*/
                    //VincularCuentas();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("HOLAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                    Console.WriteLine(ex.Message);
                    return -1;
                }
                return idNuevoUsuario;
            }
        }


            public bool modificarSaldoDeCaja(float monto, int id)
            {

                string connectionString = this.connectionString;
                string queryString = "UPDATE [dbo].[caja_de_ahorro] SET [saldo] = @saldo WHERE caja_id=@id;";
                using (SqlConnection connection =
                     new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@saldo", SqlDbType.Float));

                    command.Parameters["@id"].Value = id;
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

               try {
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


             public bool modificarfinalizarPlazoFijo( int plazo_id)
             {

                string connectionString = this.connectionString;
                string queryString = "UPDATE [dbo].[plazo_fijo] SET [fecha_fin] = @fecha ,[pagado] = @pago WHERE plazo_id = @id;";
                
                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@fecha", SqlDbType.Date));
                    command.Parameters.Add(new SqlParameter("@pago", SqlDbType.Bit));
    
                    command.Parameters["@id"].Value = plazo_id;
                    command.Parameters["@fecha"].Value = this.thisDay.ToString();
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
            List<Usuario> misUsuarios = new List<Usuario>();

            //Defino el string con la consulta que quiero realizar
            string queryString = "SELECT * from dbo.Usuarios";

            // Creo una conexión SQL con un Using, de modo que al finalizar, la conexión se cierra y se liberan recursos
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                // Defino el comando a enviar al motor SQL con la consulta y la conexión
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    //Abro la conexión
                    connection.Open();
                    //mi objecto DataReader va a obtener los resultados de la consulta, notar que a comando se le pide ExecuteReader()
                    SqlDataReader reader = command.ExecuteReader();
                    Usuario aux;
                    //mientras haya registros/filas en mi DataReader, sigo leyendo
                    while (reader.Read())
                    {
                        aux = new Usuario(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetBoolean(5), reader.GetBoolean(6));
                        misUsuarios.Add(aux);
                    }
                    //En este punto ya recorrí todas las filas del resultado de la query
                    reader.Close();



                    //YA cargué todos los domicilios, sólo me resta vincular
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return misUsuarios;
        }
        //caja
        public List<Usuario> mostrarCaja()
        {
            List<Usuario> misUsuarios = new List<Usuario>();

            //Defino el string con la consulta que quiero realizar
            string queryString = "SELECT * from caja_de_ahorro";

            // Creo una conexión SQL con un Using, de modo que al finalizar, la conexión se cierra y se liberan recursos
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                // Defino el comando a enviar al motor SQL con la consulta y la conexión
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    //Abro la conexión
                    connection.Open();
                    //mi objecto DataReader va a obtener los resultados de la consulta, notar que a comando se le pide ExecuteReader()
                    SqlDataReader reader = command.ExecuteReader();
                    Usuario aux;
                    //mientras haya registros/filas en mi DataReader, sigo leyendo
                    while (reader.Read())
                    {
                        aux = new CajaDeAhorro(reader.GetInt32(0), reader.GetInt32(1), reader.GetFloat(2));
                        misUsuarios.Add(aux);
                    }
                    //En este punto ya recorrí todas las filas del resultado de la query
                    reader.Close();



                    //YA cargué todos los domicilios, sólo me resta vincular
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return misUsuarios;
        }
        //plazo
        public List<Usuario> mostrarCaja()
        {
            List<Usuario> misUsuarios = new List<Usuario>();

            //Defino el string con la consulta que quiero realizar
            string queryString = "SELECT * from movimiento";

            // Creo una conexión SQL con un Using, de modo que al finalizar, la conexión se cierra y se liberan recursos
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                // Defino el comando a enviar al motor SQL con la consulta y la conexión
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    //Abro la conexión
                    connection.Open();
                    //mi objecto DataReader va a obtener los resultados de la consulta, notar que a comando se le pide ExecuteReader()
                    SqlDataReader reader = command.ExecuteReader();
                    Usuario aux;
                    //mientras haya registros/filas en mi DataReader, sigo leyendo
                    while (reader.Read())
                    {
                        aux = new Movimiento(reader.GetInt32(0), reader.GetInt32(1), reader.GetFloat(2), reader.GetFloat(3), reader.GetFloat(4));
                        misUsuarios.Add(aux);
                    }
                    //En este punto ya recorrí todas las filas del resultado de la query
                    reader.Close();



                    //YA cargué todos los domicilios, sólo me resta vincular
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return misUsuarios;
        }
        //tarjeta
        //movientos
        //pago

































































































































    }



}