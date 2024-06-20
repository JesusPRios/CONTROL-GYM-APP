using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class Rutina
    {
        public int id_rutina { get; set; }
        public string nombre_rutina { get; set; }
        public string duracion_rutina { get; set; }
        public Aprendiz oAprendiz { get; set; }
        public Ejercicio oEjercicio { get; set; }
        public List<Rutina_Maquina> oRutina_Maquina { get; set; } = new List<Rutina_Maquina>();
    }
}
