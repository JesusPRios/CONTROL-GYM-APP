using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class Administrador
    {
        public int id_administrador { get; set; }
        public string tipo_id_admin { get; set; }
        public string nombre_administrador { get; set; }
        public int edad_administrador { get; set; }
        public string correo_admin { get; set; }
        public long telefono_admin { get; set; }
        public string contraseña_administrador { get; set; }
    }
}
