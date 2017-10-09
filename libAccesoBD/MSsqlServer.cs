using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using LibArchivo;

namespace libAccesoBD
{
    class MSsqlServer
    {
        SqlConnection con; // SQL Server conexión
        SqlCommand com; //comandos a realizar SQL Server
        public static string Error, Error2; //guarda el mensaje de error
        public static string nombre, ApellidoP, ApellidoM, nivel; //datos del usuario activo
        public static int valor; //nivel de acceso
        public static SqlDataReader Lector; //lector SQL Server
        private string sqlcon, archivoconfig = "mssqlserver.ini";
        ArchivosBD Files = new ArchivosBD(); //leer archivo de configuración
        /// <summary>
        /// Conecta BD SQL Server
        /// </summary>
        /// <returns>True o False</returns>
        public bool ConectaDB()
        {
            bool res = false;
            if (Files.MSSQLConnectionRead(archivoconfig) == true)
            {
                this.sqlcon = Files.mssqlserver;
                try
                {
                    con = new SqlConnection(sqlcon);
                    con.Open();
                    res = true;
                }
                catch (SqlException mse)
                {
                    Error = "Error SQL al conectar. " + mse.Message;
                }
                catch (Exception general)
                {
                    Error = "Error general al conectar. " + general.Message;
                }
            }
            else
            {
                Error = "Fallo leer archivo de configuración BD de MySQL";
            }

            return res;
        }
        /// <summary>
        /// Desconecta de BD SQL Server
        /// </summary>
        /// <returns>True o False</returns>
        public bool DesconectarDB()
        {
            bool res = false;
            try
            {
                if (con.State == System.Data.ConnectionState.Open) //verifica conexión abierta
                {
                    con.Close();
                    res = true;
                }
            }
            catch (SqlException mse)
            {
                Error = "Error SQL al desconectar. " + mse.Message;
            }
            catch (Exception general)
            {
                Error = "Error general al desconectar. " + general.Message;
            }
            return res;
        }
        /// <summary>
        /// Inicia sesión usando SQL Server, recibe nombre de usuario y contraseña
        /// </summary>
        /// <param name="usuario">Nombre Usuarios</param>
        /// <param name="pass">Contraseña Usuario</param>
        /// <returns>True o False</returns>
        public bool Login(string usuario, string pass)
        {
            bool res = false;
            try
            {
                string query = "SELECT * FROM usuarios WHERE user = '" + usuario + "' AND pass = '" + pass + "'";
                com = new SqlCommand();   //conexión arreglada inicio
                com.CommandText = query;
                ConectaDB();
                com.Connection = this.con;
                com.ExecuteNonQuery();      //conexión arreglada fin
                Lector = com.ExecuteReader();
                if (!Lector.HasRows)
                {
                    Error = "Usuario y contraseña incorrectos. ";
                    res = false;
                }
                else
                {
                    while (Lector.Read())
                    {
                        if (Lector.GetString(7) == "No") //verifica si usuario esta activo
                        {
                            Error = "Usuario Inactivo. ";
                            res = false;
                        }
                        else
                        {
                            nombre = Lector.GetString(3);
                            ApellidoP = Lector.GetString(4);
                            ApellidoM = Lector.GetString(5);
                            nivel = Lector.GetString(0);
                            if (Lector.GetString(0) == "Administrador") //verifica si es admin
                            {
                                valor = 0;
                                res = true;
                            }
                            if (Lector.GetString(0) == "Cobrador") //verifica si es cobrador
                            {
                                valor = 1;
                                res = true;
                            }
                        }

                    }
                }
            }
            catch (SqlException mse)
            {
                Error = "Error SQL al Seleccionar. " + mse.Message;
            }
            catch (Exception gen)
            {
                Error = "Error de conexión a la BD. " + gen.Message;
            }
            finally
            {
                DesconectarDB();
            }
            return res;
        }
        /// <summary>
        /// Consulta Select SQL Server, indicar campos y tabla
        /// </summary>
        /// <param name="campos">Campos a leer</param>
        /// <param name="tabla">Tabla a leer</param>
        /// <returns>True o False</returns>
        public bool Leer(string campos, string tabla)
        {
            bool res = false;
            try
            {
                string query = "SELECT " + campos + " FROM " + tabla + "";
                Error2 = query;
                com = new SqlCommand();   //conexión arreglada inicio
                com.CommandText = query;
                ConectaDB();
                com.Connection = this.con;
                com.ExecuteNonQuery();      //conexión arreglada fin
                Lector = com.ExecuteReader();
                res = true;
            }
            catch (SqlException mse)
            {
                Error = "Error SQL: " + mse.Message;
            }
            catch (Exception general)
            {
                Error = "Error: " + general.Message;
            }
            finally
            {
                //DesconectarDB();
            }
            return res;
        }
        /// <summary>
        /// Eliminar datos SQL Server, indicando tabla, WHERE e id
        /// </summary>
        /// <param name="tabla">Tabla donde se va eliminar</param>
        /// <param name="donde">Que eliminar</param>
        /// <param name="id">Identificador</param>
        /// <returns>True o False</returns>
        public bool Eliminar(string tabla, string donde, string id)
        {
            bool res = false;
            try
            {
                string query = "DELETE FROM " + tabla + " WHERE " + donde + "= " + "'" + id + "'";
                Error2 = query;
                com = new SqlCommand();   //conexión arreglada inicio
                com.CommandText = query;
                ConectaDB();
                com.Connection = this.con;
                com.ExecuteNonQuery();      //conexión arreglada fin
                res = true; //no lo cambia
            }
            catch (SqlException msedel)
            {
                Error = "Error SQL: " + msedel.Message;
            }
            catch (Exception generaldel)
            {
                Error = "Se elimino anteriormente: " + generaldel.Message;
            }
            finally
            {
                DesconectarDB();
            }
            return res;
        }
        /// <summary>
        /// Actualiza datos SQL Server, necesita tabla, campo, id y valorID
        /// </summary>
        /// <param name="tabla">En la tabla ha actualizar</param>
        /// <param name="campo">El campo a actualizar</param>
        /// <param name="id">En el id a acualizar</param>
        /// <param name="valorid">Valor a </param>
        /// <returns>True o False</returns>
        public bool Actualizar(string tabla, string campo, string id, string valorid)
        {
            bool res = false;
            string query = "UPDATE " + tabla + " SET " + campo + " WHERE " + id + " = " + valorid;
            try
            {
                com = new SqlCommand();   //conexión arreglada inicio
                com.CommandText = query;
                ConectaDB();
                com.Connection = this.con;
                com.ExecuteNonQuery();      //conexión arreglada fin
                res = true;
            }
            catch (SqlException mse)
            {
                Error = "Error SQL: " + mse.Message;
            }
            catch (Exception general)
            {
                Error = "Error al actualizar: " + general.Message;
            }
            finally
            {
                DesconectarDB();
            }
            return res;
        }
        /// <summary>
        /// Inserta datos SQL Server, requiere tabla, campos y los valores
        /// </summary>
        /// <param name="tabla">Tabla a donde insertar</param>
        /// <param name="campos">Campos a donde insertar</param>
        /// <param name="valores">Valor a insertar en los campos</param>
        /// <returns>True o False</returns>
        public bool Insertar(string tabla, string campos, string valores)
        {
            bool res = false;
            try
            {
                string query = "INSERT INTO " + tabla + " (" + campos + ") VALUES (" + valores + ")";
                com = new SqlCommand();   //conexión arreglada inicio
                com.CommandText = query;
                ConectaDB();
                com.Connection = this.con;
                com.ExecuteNonQuery();      //conexión arreglada fin
                res = true;
            }
            catch (SqlException mse)
            {
                Error = "Error SQL: " + mse.Message;
            }
            catch (Exception general)
            {
                Error = "El usuario no existe: " + general.Message;
            }
            finally
            {
                DesconectarDB();
            }
            return res;
        }
    }
}