using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libAccesoBD
{
    public interface Icrud
    {
        bool ConectaBD();
        bool DesconectaBD(string tabla, string condicion);
        bool Login(string usuario, string pass);
        bool Leer(string campos, string tabla);
        bool Eliminar(string tabla, string donde, string id);
        bool Actualizar(string tabla, string campo, string id, string valorid);
        bool Insertar(string tabla, string campos, string valores);
    }
}
