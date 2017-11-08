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
        private string campos;
        private string tabla;
        private string valores;
        /// <summary>
        /// Guardar en archivo
        /// </summary>
        /// <param name="texto">Texto a escribir</param>
        /// <returns>True o False</returns>
        public bool Stamp(string texto)
        {
            bool status= false;
            Thread.Sleep(5);
            using (StreamWriter sw = new StreamWriter(@"D:\logDB.txt", true))
            {
                sw.WriteLine(texto);
            }
            return status;
        }
        #region Insertar
        public bool Insertar(string tabla, string campos, string valores)
        {
            bool status = true;
            Objmysql.Insertar(tabla, campos, valores);
            Objpostgresql.Insertar(tabla, campos, valores);
            Objmssqlserver.Insertar(tabla, campos, valores);
            return status;
        }
        #endregion Fin de insertar
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
            //Thread.Sleep(10);
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
            //Thread.Sleep(20);
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
            //Thread.Sleep(30);
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
        public bool Eliminar(string tabla, string donde, string id)
        {
            bool status = true;
            Objmysql.Eliminar(tabla, donde, id);
            Objpostgresql.Eliminar(tabla, donde, id);
            Objmssqlserver.Eliminar(tabla, donde, id);
            return status;
        }
        public bool Actualizar(string tabla, string campo, string id, string valorid)
        {
            bool status = true;
            Objmysql.Actualizar(tabla,campo,id,valorid);
            Objpostgresql.Actualizar(tabla, campo, id, valorid);
            Objmssqlserver.Actualizar(tabla, campo, id, valorid);
            return status;
        }
    }
}
