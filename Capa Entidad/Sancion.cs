using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class Sancion
    {
        public int id_sancion { get; set; }
        public string tipo_sancion { get; set; }
        public int id_aprendiz { get; set; }
        public string nombre_aprendiz { get; set; }
        public Administrador oAdministrador { get; set; }
    }
}
