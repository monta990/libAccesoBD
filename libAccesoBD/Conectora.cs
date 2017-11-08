using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace libAccesoBD
{
    /// <summary>
    /// Clase conectora para administrar tres bases de datos a la vez
    /// </summary>
    public class Conectora
    {
        /// <summary>
        /// Objeto de mysql
        /// </summary>
        Idb Objmysql = new MySQL();
        /// <summary>
        /// Objeto de PostgreSQL
        /// </summary>
        Idb Objpostgresql = new PostgreSQL();
        /// <summary>
        /// Objeto de MS SQL
        /// </summary>
        Idb Objmssqlserver = new MSsqlServer();
        /// <summary>
        /// Instanciación y parametrisación se semaforo
        /// </summary>
        static Semaphore sem = new Semaphore(1, 2); //para semaforo
        /// <summary>
        /// Objeto locker para monitor
        /// </summary>
        static object locker = new object(); //para monitor
        #region Variables de clase
        private string campos;
        private string tabla;
        private string valores;
        private string donde;
        private string id;
        private string valorid;
        #endregion Fin se variables de clase
        /// <summary>
        /// Guardar en un archivo de log el tiempo de inicio y fin de cada hilo con el nombre el mismo
        /// </summary>
        /// <param name="texto">Texto a escribir</param>
        /// <returns>True si se puedo escribir en archivo o False si no se puede escribir en el archivo</returns>
        public bool Stamp(string texto)
        {
            Monitor.Enter(locker);//inicio de sección critica
            bool status = false;
            using (StreamWriter sw = new StreamWriter(@"D:\logDB.txt", true))
            {
                sw.WriteLine(texto);
            }
            Monitor.Exit(locker);//fin de sección critica
            return status;
        }
        #region Leer desde bases de datos
        /// <summary>
        /// Leer MySQL
        /// </summary>
        private void LeerMySQL()
        {
            sem.WaitOne(10);
            Objmysql.Leer(campos, tabla);
            Stamp("Fin de lectura con: Hilo Leer en MySQL " + DateTime.Now.Hour+":"+DateTime.Now.Minute+":"+DateTime.Now.Second+":"+DateTime.Now.Millisecond);
            sem.Release();
        }
        /// <summary>
        /// Leer MSSQL
        /// </summary>
        private void LeerMSSSQL()
        {
            sem.WaitOne(20);
            Objmssqlserver.Leer(campos, tabla);
            Stamp("Fin de lectura con: Hilo Leer en MSSQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            sem.Release();
        }
        /// <summary>
        /// Leer PostgreSQL
        /// </summary>
        private void LeerPostgreSQL()
        {
            sem.WaitOne(30);
            Objpostgresql.Leer(campos, tabla);
            Stamp("Fin de lectura con: Hilo Leer en PostgreSQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            sem.Release();
        }
        public bool Leer(string campos, string tabla)
        {
            bool status = true;
            this.campos = campos;
            this.tabla = tabla;
            Stamp("");
            Stamp("Inicio de Log de Leer en BD");
            Thread HiloLeerMySQL = new Thread(new ThreadStart(LeerMySQL))
            {
                Name = "Hilo Leer en MySQL"
            };
            Thread HiloLeerMSSQL = new Thread(new ThreadStart(LeerMSSSQL))
            {
                Name = "Hilo Leer en MSSQL"
            };
            Thread HiloLeerPostgreSQL = new Thread(new ThreadStart(LeerPostgreSQL))
            {
                Name = "Hilo Leer en PostgreSQL"
            };
            Stamp("Inicio de lectura con: Hilo Leer en MySQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            HiloLeerMySQL.Start();
            Stamp("Inicio de lectura con: Hilo Leer en MSSQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            HiloLeerMSSQL.Start();
            HiloLeerMSSQL.Join();
            Stamp("Inicio de lectura con: Hilo Leer en PostgreSQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            HiloLeerPostgreSQL.Start();
            HiloLeerPostgreSQL.Join();
            //Objmysql.Leer(campos, tabla);
            //Objpostgresql.Leer(campos, tabla);
            //Objmssqlserver.Leer(campos, tabla);
            return status;
        }
        #endregion Fin de Leer
        #region Insertar a bases de datos
        /// <summary>
        /// Insertar MySQL
        /// </summary>
        public void InsertarMySQL()
        {
            sem.WaitOne(10);
            Objmysql.Insertar(tabla, campos, valores);
            Stamp("Fin de Escritura con: Hilo Escritura en MySQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            sem.Release();
        }
        /// <summary>
        /// Insertar MSSQL
        /// </summary>
        public void InsertarMSSSQL()
        {
            sem.WaitOne(20);
            Objmssqlserver.Insertar(tabla, campos, valores);
            Stamp("Fin de Escritura con: Hilo Escritura en MSSQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            sem.Release();
        }
        /// <summary>
        /// Leer PostgreSQL
        /// </summary>
        public void InsertarPostgreSQL()
        {
            sem.WaitOne(30);
            Objpostgresql.Insertar(tabla, campos, valores);
            Stamp("Fin de Escritura con: Hilo Escritura en PostgreSQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            sem.Release();
        }
        public bool Insertar(string tabla, string campos, string valores)
        {
            this.tabla = tabla;
            this.campos = campos;
            this.valores = valores;
            bool status = true;
            Stamp("");
            Stamp("Inicio de Log de Escritura en BD");
            Thread HiloEscrituraMySQL = new Thread(new ThreadStart(InsertarMySQL))
            {
                Name = "Hilo Escritura en MySQL"
            };
            Thread HiloEscrituraMSSQL = new Thread(new ThreadStart(InsertarMSSSQL))
            {
                Name = "Hilo Escritura en MSSQL"
            };
            Thread HiloEscrituraPostgreSQL = new Thread(new ThreadStart(InsertarPostgreSQL))
            {
                Name = "Hilo Escritura en PostgreSQL"
            };
            Stamp("Inicio de Escritura con: Hilo Escritura en MySQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            HiloEscrituraMySQL.Start();
            Stamp("Inicio de Escritura con: Hilo Escritura en MSSQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            HiloEscrituraMSSQL.Start();
            HiloEscrituraMSSQL.Join();
            Stamp("Inicio de Escritura con: Hilo Escritura en PostgreSQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            HiloEscrituraPostgreSQL.Start();
            HiloEscrituraPostgreSQL.Join();
            //Objmysql.Insertar(tabla, campos, valores);
            //Objpostgresql.Insertar(tabla, campos, valores);
            //Objmssqlserver.Insertar(tabla, campos, valores);
            return status;
        }
        #endregion Fin de insertar
        #region Eliminar en bases de datos
        /// <summary>
        /// Eliminar MySQL
        /// </summary>
        private void EliminarMySQL()
        {
            sem.WaitOne(10);
            Objmysql.Eliminar(tabla, donde, id);
            Stamp("Fin de Eliminar con: Hilo Eliminar en MySQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            sem.Release();
        }
        /// <summary>
        /// Leer MSSQL
        /// </summary>
        private void EliminarMSSSQL()
        {
            sem.WaitOne(20);
            Objmssqlserver.Eliminar(tabla, donde, id);
            Stamp("Fin de Eliminar con: Hilo Eliminar en MSSQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            sem.Release();
        }
        /// <summary>
        /// Eliminar PostgreSQL
        /// </summary>
        private void EliminarPostgreSQL()
        {
            sem.WaitOne(30);
            Objpostgresql.Eliminar(tabla, donde, id);
            Stamp("Fin de Eliminar con: Hilo Eliminar en PostgreSQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            sem.Release();
        }
        public bool Eliminar(string tabla, string donde, string id)
        {
            bool status = true;
            this.tabla = tabla;
            this.donde = donde;
            this.id = id;
            Stamp("");
            Stamp("Inicio de Log de Eliminar en BD");
            Thread HiloEliminarMySQL = new Thread(new ThreadStart(EliminarMySQL))
            {
                Name = "Hilo Eliminar en MySQL"
            };
            Thread HiloEliminarMSSQL = new Thread(new ThreadStart(EliminarMSSSQL))
            {
                Name = "Hilo Eliminar en MSSQL"
            };
            Thread HiloEliminarPostgreSQL = new Thread(new ThreadStart(EliminarPostgreSQL))
            {
                Name = "Hilo Eliminar en PostgreSQL"
            };
            Stamp("Inicio de Eliminar con: Hilo Eliminar en MySQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            HiloEliminarMySQL.Start();
            Stamp("Inicio de Eliminar con: Hilo Eliminar en MSSQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            HiloEliminarMSSQL.Start();
            HiloEliminarMSSQL.Join();
            Stamp("Inicio de Eliminar con: Hilo Eliminar en PostgreSQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            HiloEliminarPostgreSQL.Start();
            HiloEliminarPostgreSQL.Join();
            //Objmysql.Eliminar(tabla, donde, id);
            //Objpostgresql.Eliminar(tabla, donde, id);
            //Objmssqlserver.Eliminar(tabla, donde, id);
            return status;
        }
        #endregion Eliminar
        #region Actualizar en bases de datos
        /// <summary>
        /// Actualizar MySQL
        /// </summary>
        private void ActualizarMySQL()
        {
            sem.WaitOne(10);
            Objmysql.Actualizar(tabla, campos, id, valorid);
            Stamp("Fin de Actualizar con: Hilo Actualizar en MySQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            sem.Release();
        }
        /// <summary>
        /// Actualizar MSSQL
        /// </summary>
        private void ActualizarMSSSQL()
        {
            sem.WaitOne(20);
            Objmssqlserver.Actualizar(tabla, campos, id, valorid);
            Stamp("Fin de Actualizar con: Hilo Actualizar en MSSQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            sem.Release();
        }
        /// <summary>
        /// Leer PostgreSQL
        /// </summary>
        private void ActualizarPostgreSQL()
        {
            sem.WaitOne(30);
            Objpostgresql.Actualizar(tabla, campos, id, valorid);
            Stamp("Fin de Actualizar con: Hilo Actualizar en PostgreSQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            sem.Release();
        }
        public bool Actualizar(string tabla, string campos, string id, string valorid)
        {
            bool status = true;
            this.tabla = tabla;
            this.campos = campos;
            this.id = id;
            this.valorid = valorid;
            //Objmysql.Actualizar(tabla,campos,id,valorid);
            //Objpostgresql.Actualizar(tabla, campos, id, valorid);
            //Objmssqlserver.Actualizar(tabla, campos, id, valorid);
            Stamp("");
            Stamp("Inicio de Log de Actualizar en BD");
            Thread HiloActualizarMySQL = new Thread(new ThreadStart(ActualizarMySQL))
            {
                Name = "Hilo Actualizar en MySQL"
            };
            Thread HiloActualizarMSSQL = new Thread(new ThreadStart(ActualizarMSSSQL))
            {
                Name = "Hilo Actualizar en MSSQL"
            };
            Thread HiloActualizarPostgreSQL = new Thread(new ThreadStart(ActualizarPostgreSQL))
            {
                Name = "Hilo Actualizar en PostgreSQL"
            };
            Stamp("Inicio de Actualizar con: Hilo Actualizar en MySQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            HiloActualizarMySQL.Start();
            Stamp("Inicio de Actualizar con: Hilo Actualizar en MSSQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            HiloActualizarMSSQL.Start();
            HiloActualizarMSSQL.Join();
            Stamp("Inicio de Actualizar con: Hilo Actualizar en PostgreSQL " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            HiloActualizarPostgreSQL.Start();
            HiloActualizarPostgreSQL.Join();
            return status;
        }
        #endregion Actualizar
    }
}
