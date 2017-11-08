using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace libAccesoBD
{
    public class Conectora
    {
        Idb Objmysql = new MySQL();
        Idb Objpostgresql = new PostgreSQL();
        Idb Objmssqlserver = new MSsqlServer();
        static Semaphore sem = new Semaphore(1, 2); //para semaforo
        static object locker = new object(); //para monitor
        private string campos;
        private string tabla;
        private string valores;
        private string donde;
        private string id;
        private string valorid;
        /// <summary>
        /// Guardar en archivo
        /// </summary>
        /// <param name="texto">Texto a escribir</param>
        /// <returns>True o False</returns>
        public bool Stamp(string texto)
        {
            Monitor.Enter(locker);
            bool status = false;
            using (StreamWriter sw = new StreamWriter(@"D:\logDB.txt", true))
            {
                sw.WriteLine(texto);
            }
            Monitor.Exit(locker);
            return status;
        }
        #region Leer
        /// <summary>
        /// Leer MySQL
        /// </summary>
        public void LeerMySQL()
        {
            sem.WaitOne(10);
            Objmysql.Leer(campos, tabla);
            Stamp("Fin de lectura con: Hilo Leer en MySQL " + DateTime.Now.ToLongTimeString());
            sem.Release();
        }
        /// <summary>
        /// Leer MSSQL
        /// </summary>
        public void LeerMSSSQL()
        {
            sem.WaitOne(20);
            Objmssqlserver.Leer(campos, tabla);
            Stamp("Fin de lectura con: Hilo Leer en MSSQL " + DateTime.Now.ToLongTimeString());
            sem.Release();
        }
        /// <summary>
        /// Leer PostgreSQL
        /// </summary>
        public void LeerPostgreSQL()
        {
            sem.WaitOne(30);
            Objpostgresql.Leer(campos, tabla);
            Stamp("Fin de lectura con: Hilo Leer en PostgreSQL " + DateTime.Now.ToLongTimeString());
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
            Stamp("Inicio de lectura con: Hilo Leer en MySQL " + DateTime.Now.ToLongTimeString());
            HiloLeerMySQL.Start();
            Stamp("Inicio de lectura con: Hilo Leer en MSSQL " + DateTime.Now.ToLongTimeString());
            HiloLeerMSSQL.Start();
            HiloLeerMSSQL.Join();
            Stamp("Inicio de lectura con: Hilo Leer en PostgreSQL " + DateTime.Now.ToLongTimeString());
            HiloLeerPostgreSQL.Start();
            HiloLeerPostgreSQL.Join();
            //Objmysql.Leer(campos, tabla);
            //Objpostgresql.Leer(campos, tabla);
            //Objmssqlserver.Leer(campos, tabla);
            return status;
        }
        #endregion Fin de Leer
        #region Insertar
        /// <summary>
        /// Insertar MySQL
        /// </summary>
        public void InsertarMySQL()
        {
            sem.WaitOne(10);
            Objmysql.Insertar(tabla, campos, valores);
            Stamp("Fin de Escritura con: Hilo Escritura en MySQL " + DateTime.Now.ToLongTimeString());
            sem.Release();
        }
        /// <summary>
        /// Leer MSSQL
        /// </summary>
        public void InsertarMSSSQL()
        {
            sem.WaitOne(20);
            Objmssqlserver.Insertar(tabla, campos, valores);
            Stamp("Fin de Escritura con: Hilo Escritura en MSSQL " + DateTime.Now.ToLongTimeString());
            sem.Release();
        }
        /// <summary>
        /// Leer PostgreSQL
        /// </summary>
        public void InsertarPostgreSQL()
        {
            sem.WaitOne(30);
            Objpostgresql.Insertar(tabla, campos, valores);
            Stamp("Fin de Escritura con: Hilo Escritura en PostgreSQL " + DateTime.Now.ToLongTimeString());
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
            Stamp("Inicio de Escritura con: Hilo Escritura en MySQL " + DateTime.Now.ToLongTimeString());
            HiloEscrituraMySQL.Start();
            Stamp("Inicio de Escritura con: Hilo Escritura en MSSQL " + DateTime.Now.ToLongTimeString());
            HiloEscrituraMSSQL.Start();
            HiloEscrituraMSSQL.Join();
            Stamp("Inicio de Escritura con: Hilo Escritura en PostgreSQL " + DateTime.Now.ToLongTimeString());
            HiloEscrituraPostgreSQL.Start();
            HiloEscrituraPostgreSQL.Join();
            //Objmysql.Insertar(tabla, campos, valores);
            //Objpostgresql.Insertar(tabla, campos, valores);
            //Objmssqlserver.Insertar(tabla, campos, valores);
            return status;
        }
        #endregion Fin de insertar
        #region Eliminar
        /// <summary>
        /// Eliminar MySQL
        /// </summary>
        public void EliminarMySQL()
        {
            sem.WaitOne(10);
            Objmysql.Eliminar(tabla, donde, id);
            Stamp("Fin de Eliminar con: Hilo Eliminar en MySQL " + DateTime.Now.ToLongTimeString());
            sem.Release();
        }
        /// <summary>
        /// Leer MSSQL
        /// </summary>
        public void EliminarMSSSQL()
        {
            sem.WaitOne(20);
            Objmssqlserver.Eliminar(tabla, donde, id);
            Stamp("Fin de Eliminar con: Hilo Eliminar en MSSQL " + DateTime.Now.ToLongTimeString());
            sem.Release();
        }
        /// <summary>
        /// Eliminar PostgreSQL
        /// </summary>
        public void EliminarPostgreSQL()
        {
            sem.WaitOne(30);
            Objpostgresql.Eliminar(tabla, donde, id);
            Stamp("Fin de Eliminar con: Hilo Eliminar en PostgreSQL " + DateTime.Now.ToLongTimeString());
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
            Stamp("Inicio de Eliminar con: Hilo Eliminar en MySQL " + DateTime.Now.ToLongTimeString());
            HiloEliminarMySQL.Start();
            Stamp("Inicio de Eliminar con: Hilo Eliminar en MSSQL " + DateTime.Now.ToLongTimeString());
            HiloEliminarMSSQL.Start();
            HiloEliminarMSSQL.Join();
            Stamp("Inicio de Eliminar con: Hilo Eliminar en PostgreSQL " + DateTime.Now.ToLongTimeString());
            HiloEliminarPostgreSQL.Start();
            HiloEliminarPostgreSQL.Join();
            //Objmysql.Eliminar(tabla, donde, id);
            //Objpostgresql.Eliminar(tabla, donde, id);
            //Objmssqlserver.Eliminar(tabla, donde, id);
            return status;
        }
        #endregion Eliminar
        #region Actualizar
        /// <summary>
        /// Actualizar MySQL
        /// </summary>
        public void ActualizarMySQL()
        {
            sem.WaitOne(10);
            Objmysql.Actualizar(tabla, campos, id, valorid);
            Stamp("Fin de Actualizar con: Hilo Actualizar en MySQL " + DateTime.Now.ToLongTimeString());
            sem.Release();
        }
        /// <summary>
        /// Actualizar MSSQL
        /// </summary>
        public void ActualizarMSSSQL()
        {
            sem.WaitOne(20);
            Objmssqlserver.Actualizar(tabla, campos, id, valorid);
            Stamp("Fin de Actualizar con: Hilo Actualizar en MSSQL " + DateTime.Now.ToLongTimeString());
            sem.Release();
        }
        /// <summary>
        /// Leer PostgreSQL
        /// </summary>
        public void ActualizarPostgreSQL()
        {
            sem.WaitOne(30);
            Objpostgresql.Actualizar(tabla, campos, id, valorid);
            Stamp("Fin de Actualizar con: Hilo Actualizar en PostgreSQL " + DateTime.Now.ToLongTimeString());
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
            Stamp("Inicio de Actualizar con: Hilo Actualizar en MySQL " + DateTime.Now.ToLongTimeString());
            HiloActualizarMySQL.Start();
            Stamp("Inicio de Actualizar con: Hilo Actualizar en MSSQL " + DateTime.Now.ToLongTimeString());
            HiloActualizarMSSQL.Start();
            HiloActualizarMSSQL.Join();
            Stamp("Inicio de Actualizar con: Hilo Actualizar en PostgreSQL " + DateTime.Now.ToLongTimeString());
            HiloActualizarPostgreSQL.Start();
            HiloActualizarPostgreSQL.Join();
            return status;
        }
        #endregion Actualizar
    }
}
