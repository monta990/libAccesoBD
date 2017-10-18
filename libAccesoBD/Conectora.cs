using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libAccesoBD
{
    public class Conectora
    {
        Idb Objmysql = new MySQL();
        Idb Objpostgresql = new PostgreSQL();
        Idb Objmssqlserver = new MSsqlServer();

        public void Insertar(string tabla, string campos, string valores)
        {
            Objmysql.Insertar(tabla, campos, valores);
            Objpostgresql.Insertar(tabla, campos, valores);
            Objmssqlserver.Insertar(tabla, campos, valores);
        }
        public void Leer(string campos, string tabla)
        {
            Objmysql.Leer(campos, tabla);
            Objpostgresql.Leer(campos, tabla);
            Objmssqlserver.Leer(campos, tabla);
        }
        public void Eliminar(string tabla, string donde, string id)
        {
            Objmysql.Eliminar(tabla, donde, id);
            Objpostgresql.Eliminar(tabla, donde, id);
            Objmssqlserver.Eliminar(tabla, donde, id);
        }
        public void Actualizar(string tabla, string campo, string id, string valorid)
        {
            Objmysql.Actualizar(tabla,campo,id,valorid);
            Objpostgresql.Actualizar(tabla, campo, id, valorid);
            Objmssqlserver.Actualizar(tabla, campo, id, valorid);
        }
    }
}
