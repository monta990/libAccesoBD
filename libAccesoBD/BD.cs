using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace libAccesoBD
{
    public class BD
    {
        MySqlConnection con; // mysql conexión
        SqlConnection con2;  //sql conexión
        MySqlCommand com; //comandos a realizar mysql
        SqlCommand com2; //sql conexión
        public static string Error; //guarda el mensaje de erro
        public static string nombre, ApellidoP, ApellidoM, nivel; //datos del usuario activo
        public static int valor; //nivel de acceso
        //public static DataTable dt;
        public static MySqlDataReader Lector; //lector mysql
        public static SqlDataReader Lector2; //lector sql

        //Inicio Modulo MySQL
        //inicio Conexión BD MySQL
        public bool ConectaDB() //inicia conexión a la BD
        {
            bool res = false;
            try
            {
                //                         Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;
                //con = new MySqlConnection("Server = MYSQL5013.SmarterASP.NET;Database=db_a14f18_proguth;Uid=a14f18_proguth;Pwd=pr0gCon0");  //online
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
        // incio de modulo usuarios
        // cargar lista de usuarios
        public bool LeerUsuarios() //Leer usuarios
        {
            bool res = false;
            try
            {
                string query = "SELECT * FROM usuarios";
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
                Error = "El usuario no existe: " + general.Message;
            }
            finally
            {
                //DesconectarDB();
            }
            return res;
        }
        // fin de cargar lista de usuarios
        // inicio querys modulo usuarios
        public bool CrearUsuario(string nivel, string usuario, string pass, string nombre, string ap1, string ap2, string email, string estado) //Crea usuario
        {
            bool res = false;
            try
            {
                string query = "INSERT INTO `usuarios` (`nivel`, `user`, `pass`, `nombre`, `ap1`, `ap2`, `email`, `estado`) VALUES ('" + nivel + "', '" + usuario + "', '" + pass + "', '" + nombre + "', '" + ap1 + "', '" + ap2 + "', '" + email + "', '" + estado + "')";
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
        // fin de crear usuario
        // inicio de eliminar usuario
        public bool EliminarUsuario(string usuario) //Elimina usuario
        {
            bool res = false;
            try
            {
                string query = "DELETE FROM usuarios WHERE user = '" + usuario + "'";
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
                Error = "El usuario no existe o fue eliminado anteriormente: " + generaldel.Message;
            }
            finally
            {
                DesconectarDB();
            }
            return res;
        }
        // fin de eliminar usuario
        // incio de editar usuario
        public bool EditarUsuario(string nivel, string usuario, string pass, string nombre, string ap1, string ap2, string email, string estado) //Edita Usuario
        {
            bool res = false;
            try
            {
                string query = "UPDATE usuarios SET nivel = '" + nivel + "', pass = '" + pass + "', nombre = '" + nombre + "', ap1 = '" + ap1 + "', ap2 = '" + ap2 + "', email = '" + email + "', estado = '" + estado + "' WHERE user = '" + usuario + "'";
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
                Error = "El usuario no existe o fue eliminado anteriormente: " + generaldel.Message;
            }
            finally
            {
                DesconectarDB();
            }
            return res;
        }
        // fin de editar usuario
        // fin de modulo usuario
        // inicio de modulo deudor
        // cargar lista de deudores
        public bool LeerDeudores() //Leer usuarios
        {
            bool res = false;
            try
            {
                string query = "SELECT * FROM deudores";
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
                Error = "El usuario no existe: " + general.Message;
            }
            finally
            {
                //DesconectarDB();
            }
            return res;
        }
        // fin de cargar lista de deudores
        // inicio de crear deudor
        public bool CrearDeudor(string nombre, string ap1, string ap2, string ine, string calle, string nodom, string colonia, string ciudad, string codpostal, string estado, string tel, string AvalNombre, string AvalTelefono, string email) //Agrega un material a una carrera
        {
            bool res = false;
            try
            {
                string query = "INSERT INTO `deudores` (`nombre`, `ap1`, `ap2`, `ine`, `calle`, `nodom`, `colonia`, `ciudad`, `codpostal`, `estado`, `tel`, `AvalNombre`, `AvalTelefono`, `email`) VALUES ('" + nombre + "', '" + ap1 + "', '" + ap2 + "', '" + ine + "', '" + calle + "', '" + nodom + "', '" + colonia + "', '" + ciudad + "', '" + codpostal + "', '" + estado + "', '" + tel + "', '" + AvalNombre + "', '" + AvalTelefono + "', '" + email + "')";
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
        // fin de crear deudor
        // inicio de eliminar deudor
        public bool EliminarDeudor(string id) //Elimina usuario
        {
            bool res = false;
            try
            {
                string query = "DELETE FROM deudores WHERE id = '" + id + "'";
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
                Error = "El usuario no existe o fue eliminado anteriormente: " + generaldel.Message;
            }
            finally
            {
                DesconectarDB();
            }
            return res;
        }
        // fin de eliminar deudor
        // incio de editar deudor
        public bool EditarDeudor(string id, string nombre, string ap1, string ap2, string ine, string calle, string nodom, string colonia, string ciudad, string codpostal, string estado, string tel, string AvalNombre, string AvalTelefono, string email) //Edita Usuario
        {
            bool res = false;
            try
            {
                string query = "UPDATE deudores SET nombre = '" + nombre + "', ap1 = '" + ap1 + "', ap2 = '" + ap2 + "', ine = '" + ine + "', calle = '" + calle + "', nodom = '" + nodom + "', colonia = '" + colonia + "', ciudad = '" + ciudad + "', codpostal = '" + codpostal + "', estado = '" + estado + "', tel = '" + tel + "', AvalNombre = '" + AvalNombre + "', AvalTelefono = '" + AvalTelefono + "', email = '" + email + "' WHERE id = '" + id + "'";
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
                Error = "El usuario no existe o fue eliminado anteriormente: " + generaldel.Message;
            }
            finally
            {
                DesconectarDB();
            }
            return res;
        }
        // fin de editar deudor
        // leer DuedorID
        public bool LeerDuedorID(String id) //Leer Prestamos
        {
            bool res = false;
            try
            {
                string query = "SELECT * FROM deudores WHERE id = '" + id + "'";
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
                Error = "No existen Prestamos: " + general.Message;
            }
            finally
            {
                //DesconectarDB();
            }
            return res;
        }
        // fin leer DeudorID
        // fin de modulo deudor
        // modulo prestamos
        // leer prestamos
        public bool LeerPrestamos() //Leer Prestamos
        {
            bool res = false;
            try
            {
                string query = "SELECT * FROM prestamos";
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
                Error = "SQL: " + mse.Message;
            }
            catch (Exception general)
            {
                Error = "No existen Prestamos: " + general.Message;
            }
            finally
            {
                //DesconectarDB();
            }
            return res;
        }
        // fin leer prestamos
        // inicio de crear prestamo
        public bool CrearPrestamo(string id_deudor, string monto, string plazo, string id_prenda, string nom_prenda, string nom_deudor) //crea prestamo
        {
            bool res = false;
            try
            {
                string query = "INSERT INTO `prestamos` (`id_deudor`, `monto`, `plazo`, `id_prenda`, `nom_prenda`, `nom_deudor`) VALUES ('" + id_deudor + "', '" + monto + "', '" + plazo + "', '" + id_prenda + "', '" + nom_prenda + "', '" + nom_deudor + "')";
                com = new MySqlCommand();   //conexión arreglada inicio
                com.CommandText = query;
                ConectaDB();
                com.Connection = this.con;
                com.ExecuteNonQuery();      //conexión arreglada fin
                res = true;
            }
            catch (MySqlException mse)
            {
                Error = "SQL: " + mse.Message;
            }
            catch (Exception general)
            {
                Error = "No se creo el prestamo: " + general.Message;
            }
            finally
            {
                DesconectarDB();
            }
            return res;
        }
        // fin de crear prestamo
        // incio de editar prestamo
        public bool EditarPrestamo(string id, string id_deudor, string monto, string plazo, string id_prenda, string nom_prenda, string nom_deudor) //Edita Usuario
        {
            bool res = false;
            try
            {
                string query = "UPDATE prestamos SET id_deudor = '" + id_deudor + "', monto = '" + monto + "', plazo = '" + plazo + "', id_prenda = '" + id_prenda + "', nom_prenda = '" + nom_prenda + "', nom_deudor = '" + nom_deudor + "' WHERE id = '" + id + "'";
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
                Error = "El Prestamo no existe: " + generaldel.Message;
            }
            finally
            {
                DesconectarDB();
            }
            return res;
        }
        // fin de editar prestamo
        // inicio de eliminar prestamo
        public bool EliminarPrestamo(string id) //Elimina usuario
        {
            bool res = false;
            try
            {
                string query = "DELETE FROM prestamos WHERE id = '" + id + "'";
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
                Error = "El prestamo no existe o fue eliminado anteriormente: " + generaldel.Message;
            }
            finally
            {
                DesconectarDB();
            }
            return res;
        }
        // fin de eliminar prestamo
        // fin de modulo prestamo
        // inicio modulo prenda
        // leer prenda
        public bool LeerPrendas() //Leer Prestamos
        {
            bool res = false;
            try
            {
                string query = "SELECT * FROM prenda";
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
                Error = "No existen Prestamos: " + general.Message;
            }
            finally
            {
                //DesconectarDB();
            }
            return res;
        }
        // fin leer prenda
        // leer prenda
        public bool LeerPrendaID(string id) //Leer Prestamos
        {
            bool res = false;
            try
            {
                string query = "SELECT * FROM prenda WHERE id = '" + id + "'";
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
                Error = "No existen Prestamos: " + general.Message;
            }
            finally
            {
                //DesconectarDB();
            }
            return res;
        }
        // fin leer prenda
        // fin de modulo prenda
        // fin de conexión mysql
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        // inicio modulo sql
        // fin modulo sql
    }
}