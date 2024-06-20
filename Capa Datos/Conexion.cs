using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Capa_Datos
{
    public class Conexion
    {
        //Se define la conexion con la base de datos, la cual esta declarada en el web config y se almacena en la variable "cadena"
        public static string cn = ConfigurationManager.ConnectionStrings["cadena"].ToString();
    }
}