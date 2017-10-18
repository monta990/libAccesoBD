using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libAccesoBD
{
    interface Idb
    {
        /// <summary>
        /// Datos a leer de la base de datos
        /// </summary>
        /// <param name="campos">Campos a buscar</param>
        /// <param name="tabla">Tabla en la cual buscar</param>
        /// <returns>Resultados</returns>
        bool Leer(string campos, string tabla);
        /// <summary>
        /// Datos a eliminar de la base de datos
        /// </summary>
        /// <param name="tabla">De cual tabla</param>
        /// <param name="donde">Que eliminar</param>
        /// <param name="id">Id de donde eliminar</param>
        /// <returns></returns>
        bool Eliminar(string tabla, string donde, string id);
        /// <summary>
        /// Datos a actualizar en la tabla
        /// </summary>
        /// <param name="tabla">Tabla en donde actualizar</param>
        /// <param name="campo">Capos a actualizar</param>
        /// <param name="id">ID de campos a actualizar</param>
        /// <param name="valorid">En el registro</param>
        /// <returns></returns>
        bool Actualizar(string tabla, string campo, string id, string valorid);
        /// <summary>
        /// Agregar datos en la tabla de la base de datos
        /// </summary>
        /// <param name="tabla">Tabla donde insertar</param>
        /// <param name="campos">Campos en donde insertar</param>
        /// <param name="valores">Valores a insertar</param>
        /// <returns></returns>
        bool Insertar(string tabla, string campos, string valores);
    }
}
