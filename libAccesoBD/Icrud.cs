using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libAccesoBD
{
    /// <summary>
    /// Interface de conexión multiple
    /// </summary>
    public interface Icrud
    {
        /// <summary>
        /// Conecta a BD
        /// </summary>
        /// <returns>True o False</returns>
        bool ConectaBD();
        /// <summary>
        /// Desconecta de BD
        /// </summary>
        /// <returns>True o False</returns>
        bool DesconectaBD();
        /// <summary>
        /// Inicia sesión, recibe nombre de usuario y contraseña
        /// </summary>
        /// <param name="usuario">Nombre Usuarios</param>
        /// <param name="pass">Contraseña Usuario</param>
        /// <returns>True o False</returns>
        bool Login(string usuario, string pass);
        /// <summary>
        /// Consulta Select, indicar campos y tabla
        /// </summary>
        /// <param name="campos">Campos a leer</param>
        /// <param name="tabla">Tabla a leer</param>
        /// <returns>True o False</returns>
        bool Leer(string campos, string tabla);
        /// <summary>
        /// Eliminar datos, indicando tabla, WHERE e id
        /// </summary>
        /// <param name="tabla">Tabla donde se va eliminar</param>
        /// <param name="donde">Que eliminar</param>
        /// <param name="id">Identificador</param>
        /// <returns>True o False</returns>
        bool Eliminar(string tabla, string donde, string id);
        /// <summary>
        /// Actualiza datos, necesita tabla, campo, id y valorID
        /// </summary>
        /// <param name="tabla">En la tabla ha actualizar</param>
        /// <param name="campo">El campo a actualizar</param>
        /// <param name="id">En el id a acualizar</param>
        /// <param name="valorid">Valor a </param>
        /// <returns>True o False</returns>
        bool Actualizar(string tabla, string campo, string id, string valorid);
        /// <summary>
        /// Inserta datos, requiere tabla, campos y los valores
        /// </summary>
        /// <param name="tabla">Tabla a donde insertar</param>
        /// <param name="campos">Campos a donde insertar</param>
        /// <param name="valores">Valor a insertar en los campos</param>
        /// <returns>Regresa True o False</returns>
        bool Insertar(string tabla, string campos, string valores);
    }
}