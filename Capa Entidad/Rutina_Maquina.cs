using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class Rutina_Maquina
    {
        public int id_rutina { get; set; }
        public Rutina oRutina { get; set; }
        public int id_maquina { get; set; }
        public Maquinas oMaquina { get; set; }
    }
}
