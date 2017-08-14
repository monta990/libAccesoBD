using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace libAccesoBD
{
    public class BD
    {
        MySqlConnection con; // mysql conexión
        MySqlCommand com; //comandos a realizar mysql
        public static string Error, Error2; //guarda el mensaje de error
        public static string nombre, ApellidoP, ApellidoM, nivel; //datos del usuario activo
        public static int valor; //nivel de acceso
        public static MySqlDataReader Lector; //lector mysql

        //Inicio Modulo MySQL
        //inicio Conexión BD MySQL
        public bool ConectaDB() //inicia conexión a la BD
        {
            bool res = false;
            try
            {
                con = new MySqlConnection("Server = 127.0.0.1;Database=prestamos;Uid=root;Pwd=alvarez");  //offline
                con.Open();
                res = true;
            }
            catch (MySqlException mse)
            {
                Error = "Error SQL al conectar. " + mse.Message;
            }
            catch (Exception general)
            {
                Error = "Error general al conectar. " + general.Message;
            }
            return res;
        }
        // final Conexión MySQL
        // inicio deconectar MySQL
        public bool DesconectarDB() //Desconecta de BD
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
            catch (MySqlException mse)
            {
                Error = "Error SQL al desconectar. " + mse.Message;
            }
            catch (Exception general)
            {
                Error = "Error general al desconectar. " + general.Message;
            }
            return res;
        }
        // final desconectar mysql
        // fin de manejo basico de base de datos
        // inicio modulo login
        public bool Login(string usuario, string pass) //Verifica acceso y mueve al form correcto
        {
            bool res = false;
            try
            {
                string query = "SELECT * FROM usuarios WHERE user = '" + usuario + "' AND pass = '" + pass + "'";
                com = new MySqlCommand();   //conexión arreglada inicio
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
            catch (MySqlException mse)
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
        // fin de modulo login
        // fin de conexión mysql
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        // inicio de querys genericas
        // inicio de leer
        public bool Leer(string campos, string tabla) //Leer Prestamos
        {
            bool res = false;
            try
            {
                string query = "SELECT " + campos + " FROM " + tabla + "";
                Error2 = query;
                com = new MySqlCommand();   //conexión arreglada inicio
                com.CommandText = query;
                ConectaDB();
                com.Connection = this.con;
                com.ExecuteNonQuery();      //conexión arreglada fin
                Lector = com.ExecuteReader();
                res = true;
            }
            catch (MySqlException mse)
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
        // fin de leer
        // inicio de eliminar
        public bool Eliminar(string tabla, string donde, string id)
        {
            bool res = false;
            try
            {
                string query = "DELETE FROM " + tabla + " WHERE " + donde + "= " + "'" + id + "'";
                Error2 = query;
                com = new MySqlCommand();   //conexión arreglada inicio
                com.CommandText = query;
                ConectaDB();
                com.Connection = this.con;
                com.ExecuteNonQuery();      //conexión arreglada fin
                res = true; //no lo cambia
            }
            catch (MySqlException msedel)
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
        // fin de eliminar
        // inicio de actualizar
        public bool Actualizar(string tabla, string campo, string id, string valorid)
        {
            bool res = false;
            string query = "UPDATE " + tabla + " SET " + campo + " WHERE " + id + " = " + valorid;
            try
            {
                com = new MySqlCommand();   //conexión arreglada inicio
                com.CommandText = query;
                ConectaDB();
                com.Connection = this.con;
                com.ExecuteNonQuery();      //conexión arreglada fin
                res = true;
            }
            catch (MySqlException mse)
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
        // fin de actualizar
        // inicio de insertar
        public bool Insertar(string tabla, string campos, string valores)
        {
            bool res = false;
            try
            {
                string query = "INSERT INTO " + tabla + " (" + campos + ") VALUES(" + valores + ")";
                com = new MySqlCommand();   //conexión arreglada inicio
                com.CommandText = query;
                ConectaDB();
                com.Connection = this.con;
                com.ExecuteNonQuery();      //conexión arreglada fin
                res = true;
            }
            catch (MySqlException mse)
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
        // fin de insertar
        // fin de querys genericas
        // fin de crear prestamo
    }
}