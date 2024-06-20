using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class Registro
    {
        public int id_registro { get; set; }
        public DateTime fecha_registro { get; set; }
        public int MyProperty { get; set; }
        public Administrador oAdministrador { get; set; }
    }
}
