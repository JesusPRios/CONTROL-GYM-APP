using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class Maquinas
    {
        public int id_maquina { get; set; }
        public string nombre_maquina { get; set; }
        public string tipo_maquina { get; set; }
        public int cantidad_maquinas { get; set; }
        public string estado_maquina { get; set; }
        public List<Rutina_Maquina> oRutina_Maquina { get; set; } = new List<Rutina_Maquina>();
    }
}